using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;

enum Hand2 { Left, Right, None }
enum RelativePosition2 { Close, InRange, Far, Other }
class HandSelection2
{
    public HandTracking handTracking { get; }
    public Hand handedness = Hand.None;
    private bool isRightHanded = false;

    public RelativePosition2 rangePosition = RelativePosition2.Other;

    public HandSelection2(HandTracking handTracking)
    {
        this.handTracking = handTracking;
    }

    public bool HasHands()
    {
        return handedness != Hand.None;
    }

    public bool isRightHand()
    {
        return handedness == Hand.Right;
    }
}

public class AnimalMoveWithBodyAndCController : MonoBehaviour
{
    [SerializeField]
    private GameObject AnimalBody;

    public float Speed = 1f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    public HandTracking handTracking;
    public Vector2 handDetectionRange = new Vector2(0.5f, 3.5f);

    [Header("CharacterController")]
    private CharacterController _controller;
    private Vector3 _velocity;
    public Vector3 Drag;
    public float Gravity = -9.81f;

    private HandSelection2 handSelection;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);
        if (_body && _controller)
        {
            _controller = null;
        }
        handSelection = new HandSelection2(handTracking);
    }

    void Update()
    {
        CalculateNearestHand();
        if (_body)
        {
            _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        }
        else
        {
            _isGrounded = _controller.isGrounded;
        }


        if (_body)
        {
            _inputs = Vector3.zero;

            moveBodyTowardHand();

        }
        else if (_controller)
        {
            _isGrounded = _controller.isGrounded;
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0f;

            moveControllerTowardHand();
            AddGravityAndFriction(false);
        }



        /*_inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
        */

        /*if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
            _body.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
        */
    }


    void FixedUpdate()
    {

        if (_body)
        {
            if (_inputs != Vector3.zero)
                transform.forward = _inputs;

            //_body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
            MoveBodyToTarget(_inputs);
        }

    }


    //______________________________________________________________________ Rigid Body _______________________________________________________________________________________

    private void moveBodyTowardHand()
    {

        _inputs = Vector3.zero;

        var FingerLeft = handTracking.getIndexObject(false);
        var FingerRight = handTracking.getIndexObject(true);

        bool hasFingers = true;

        Vector3 goal = Vector3.zero;
        if (!handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight))
        {
            goal = FingerRight.transform.TransformPoint(FingerRight.transform.position);
            //goal = FingerRight.transform.position;
        }
        else if (handTracking.isFingerEnabled(FingerLeft) && !handTracking.isFingerEnabled(FingerRight))
        {
            goal = FingerLeft.transform.TransformPoint(FingerLeft.transform.position);
            //goal = FingerLeft.transform.position;
        }
        else if (handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight))
        {
            var FingerLeftWorldRelative = FingerLeft.transform.TransformPoint(FingerLeft.transform.position);
            var FingerRightWorldRelative = FingerRight.transform.TransformPoint(FingerRight.transform.position);

            /*var FingerLeftWorldRelative = FingerLeft.transform.position;
            var FingerRightWorldRelative = FingerRight.transform.position;
            */

            //Choose the closest finger
            goal = Vector3.Distance(transform.position, FingerLeftWorldRelative) < Vector3.Distance(transform.position, FingerRightWorldRelative) ? FingerLeftWorldRelative : FingerRightWorldRelative;

        }
        else
        {
            hasFingers = false;
        }
        goal.y = 0;
        if (hasFingers)
        {
            if (Vector3.Distance(transform.position, goal) < handDetectionRange.y && Vector3.Distance(transform.position, goal) > handDetectionRange.x)
            {
                Debug.Log("Check passed");
                _inputs = goal;
            }
            else
            {
                //Debug.Log("Check not passed");
            }
        }

        if (_inputs != Vector3.zero)
            transform.forward = _inputs;
    }

    /// <summary>
    /// Physic related
    /// </summary>
    private void MoveBodyToTarget(Vector3 targetPosition)
    {
        if (targetPosition == transform.position)
            return;
        if (targetPosition == Vector3.zero)
            return;

        Vector3 moveDiff = targetPosition - transform.position;
        Vector3 moveDir = moveDiff.normalized;
        _body.MovePosition(_body.position + moveDir * Speed * Time.fixedDeltaTime);

        /*Vector3 moveDiff = targetPosition - transform.position;
        Vector3 moveDir = moveDiff.normalized * 15f;
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            _body.MovePosition(_body.position + moveDir * Speed * Time.fixedDeltaTime);
        }
        else
        {
            _body.MovePosition(_body.position + moveDiff * Speed * Time.fixedDeltaTime);
        }
        */
    }








    //------------------------------------------------------------CHARACTER CONTROLLER----------------------------------------------------------------------------------------------------------//








    private void moveControllerTowardHand()
    {


        var FingerLeft = handTracking.getIndexObject(false);
        var FingerRight = handTracking.getIndexObject(true);

        bool hasFingers = true;

        Vector3 goal = Vector3.zero;
        if (!handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight)) //Only Right hand
        {
            goal = IntoLocalCoord(transform, FingerRight.transform);
        }
        else if (handTracking.isFingerEnabled(FingerLeft) && !handTracking.isFingerEnabled(FingerRight)) //Only left hand
        {
            goal = IntoLocalCoord(transform, FingerLeft.transform);
        }
        else if (handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight)) //Both hands
        {
            var FingerLeftRelative = IntoLocalCoord(transform, FingerLeft.transform);
            var FingerRightRelative = IntoLocalCoord(transform, FingerRight.transform);

            //Choose the closest finger
            goal = Vector3.Distance(transform.position, FingerLeftRelative) < Vector3.Distance(transform.position, FingerRightRelative) ? FingerLeftRelative : FingerRightRelative;

        }
        else
        {
            hasFingers = false;
        }
        goal.y = 0;
        if (hasFingers)
        {
            if (Vector3.Distance(transform.position, goal) < handDetectionRange.y && Vector3.Distance(transform.position, goal) > handDetectionRange.x)
            {
                MoveControllerToPoint(goal);

            }
            else
            {
                //Debug.Log("Check not passed");
            }
        }


    }

    /// <summary>
    /// From https://answers.unity.com/questions/599028/how-to-move-character-controller-from-point-to-poi.html
    /// </summary>
    /// <param name="targetPosition"></param>
    void MoveControllerToPoint(Vector3 targetPosition)
    {
        if (targetPosition == transform.position)
            return;

        Vector3 moveDiff = targetPosition - transform.position;
        //Vector3 moveDir = moveDiff.normalized * 15f * Time.deltaTime;
        Vector3 moveDir = moveDiff.normalized * Time.deltaTime;
        Vector3 move;
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            move = moveDir * Speed;
            //Debug.Log("Choose dir");
        }
        else
        {
            move = moveDiff * Speed;
            Debug.Log("Choose diff");
        }

        _controller.Move(move);
        if (AnimalBody)
        {
            if (move != Vector3.zero)
            {
                move.y = 0;
                AnimalBody.transform.forward = move;
            }
        }

    }

    private void AddGravityAndFriction(bool addFriction)
    {
        _velocity.y += Gravity * Time.deltaTime; //Add gravity

        if (addFriction)
        {
            //Add friction
            _velocity.x /= 1 + Drag.x * Time.deltaTime;
            _velocity.y /= 1 + Drag.y * Time.deltaTime;
            _velocity.z /= 1 + Drag.z * Time.deltaTime;
        }
        _controller.Move(_velocity * Time.deltaTime);
    }


    public void JumpController()
    {
        if (_isGrounded)
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);


    }




    /// <summary>
    /// Calculate the Vector3 of otherObjLocaltransform relative to localTransform
    /// https://answers.unity.com/questions/154176/transformtransformpoint-vs-transformdirection.html
    /// </summary>
    /// <param name="localTransform"></param>
    /// <param name="otherObjLocaltransform"></param>
    /// <returns></returns>
    protected Vector3 IntoLocalCoord(Transform localTransform, Transform otherObjLocaltransform)
    {
        /*Vector3 WorldPosition = otherObjLocaltransform.TransformDirection(otherObjLocaltransform.position);
        return localTransform.InverseTransformDirection(WorldPosition);
        */


        Vector3 pointWorldSpace = otherObjLocaltransform.TransformDirection(otherObjLocaltransform.position);
        pointWorldSpace += otherObjLocaltransform.position;
        pointWorldSpace -= localTransform.position;
        return localTransform.InverseTransformDirection(pointWorldSpace);



        /*Vector3 WorldPosition = otherObjLocaltransform.TransformPoint(otherObjLocaltransform.position);
        return localTransform.InverseTransformPoint(WorldPosition);
        */


        //TransformDirection() is used here to transform a position from object space to world space.
        //The scale of your game object is not taken into account.
        //Vector3 pointObjectSpace = new Vector3(2f, 0.5f, 0f);
        /*Vector3 pointWorldSpace = otherObjLocaltransform.TransformDirection(otherObjLocaltransform.position);
        //pointWorldSpace is now (0.8, 1.9, 0)
        pointWorldSpace += otherObjLocaltransform.position; //Add this to fix the lack of position.
                                                          //pointWorldSpace is now (5, 5, 0)

        //InverseTransformDirection() is used here to transform a position from world space to object space.
        //The scale of your game object is not taken into account.
        //pointWorldSpace = new Vector3(5f, 5f, 0f);
        pointWorldSpace -= localTransform.position; //Add this to fix the lack of position.
                                                          //pointWorldSpace is now (0.8, 1.9, 0)
        return localTransform.InverseTransformDirection(pointWorldSpace);
        //return in pointObjectSpace is now (2f, 0.5f, 0f)
        */
    }

    private void CalculateNearestHand()
    {
        var FingerLeft = this.handSelection.handTracking.getIndexObject(false);
        var FingerRight = this.handSelection.handTracking.getIndexObject(true);

        bool hasFingers = true;

        Vector3 goal = Vector3.zero;
        if (!this.handSelection.handTracking.isFingerEnabled(FingerLeft) && this.handSelection.handTracking.isFingerEnabled(FingerRight)) //Only Right hand
        {
            this.handSelection.handedness = Hand.Right;
            goal = IntoLocalCoord(transform, FingerRight.transform);
        }
        else if (this.handSelection.handTracking.isFingerEnabled(FingerLeft) && !this.handSelection.handTracking.isFingerEnabled(FingerRight)) //Only left hand
        {
            this.handSelection.handedness = Hand.Left;
            goal = IntoLocalCoord(transform, FingerLeft.transform);
        }
        else if (this.handSelection.handTracking.isFingerEnabled(FingerLeft) && this.handSelection.handTracking.isFingerEnabled(FingerRight)) //Both hands
        {
            var FingerLeftRelative = IntoLocalCoord(transform, FingerLeft.transform);
            var FingerRightRelative = IntoLocalCoord(transform, FingerRight.transform);

            //Choose the closest finger
            if (Vector3.Distance(transform.position, FingerLeftRelative) < Vector3.Distance(transform.position, FingerRightRelative))
            {
                this.handSelection.handedness = Hand.Left;
                goal = IntoLocalCoord(transform, FingerLeft.transform);
            }
            else
            {
                this.handSelection.handedness = Hand.Right;
                goal = IntoLocalCoord(transform, FingerRight.transform);
            }

        }
        else
        {
            this.handSelection.handedness = Hand.None;
            this.handSelection.rangePosition = RelativePosition2.Other;
            hasFingers = false;
        }
        if (hasFingers)
        {
            if (Vector3.Distance(transform.position, goal) < handDetectionRange.x)
            {
                Debug.Log("Close");
                this.handSelection.rangePosition = RelativePosition2.Close;
            }
            else if (Vector3.Distance(transform.position, goal) > handDetectionRange.y)
            {
                Debug.Log("Far");
                this.handSelection.rangePosition = RelativePosition2.Far;
            }
            else
            {
                this.handSelection.rangePosition = RelativePosition2.InRange;
            }
        }
        Debug.Log("Hand : " + this.handSelection.handedness + " Position : " + this.handSelection.rangePosition);
    }
}
