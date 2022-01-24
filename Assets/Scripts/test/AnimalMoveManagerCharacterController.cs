using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adapted from https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483
/// </summary>
public class AnimalMoveManagerCharacterController : MonoBehaviour
{
    public float Speed = 6f;
    public float JumpHeight = 2f;
    public float Gravity = -9.81f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    public Vector3 Drag;

    public HandTracking handTracking;
    public float handDetectionRange = 3;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded = true;
    private Transform _groundChecker;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        //_groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        //Stop increasing gravity when the object is grounded
        //_isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        moveTowardHand();

        /*Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * Time.deltaTime * Speed);
        if (move != Vector3.zero)
            transform.forward = move;
        */
        /*
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dash");
            _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));
        }
        */
        /*

        _velocity.y += Gravity * Time.deltaTime; //Add gravity

        //Add friction
        _velocity.x /= 1 + Drag.x * Time.deltaTime;
        _velocity.y /= 1 + Drag.y * Time.deltaTime;
        _velocity.z /= 1 + Drag.z * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
        */
    }

    private void moveTowardHand()
    {
        var FingerLeft = handTracking.getIndexObject(false).transform;
        var FingerRight = handTracking.getIndexObject(true).transform;

        //Debug.Log("Transform finger left : " + FingerLeft.position + "Transform finger right  : " + FingerRight.position);

        /*Transform goal;
        if(Vector3.Distance(gameObject.transform.position, FingerLeft.transform.position) < Vector3.Distance(gameObject.transform.position, FingerRight.transform.position))
        {
            goal = FingerLeft.transform;
        } else
        {
            goal = FingerRight.transform;
        }*/

        var FingerLeftRelative = IntoLocalCoord(transform, FingerLeft);
        var FingerRightRelative = IntoLocalCoord(transform, FingerRight);

        //Choose the closest finger
        var goal = Vector3.Distance(transform.position, FingerLeftRelative) < Vector3.Distance(transform.position, FingerRightRelative) ? FingerLeftRelative : FingerRightRelative;


        if(Vector3.Distance(transform.position, goal) < handDetectionRange && Vector3.Distance(transform.position, goal) > 0.3)
        {
            goal.y = 0;
            MoveToPoint(goal);
            //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            /*Debug.Log("Transform finger left : " + FingerLeft.position + "Transform finger right  : " + FingerRight.position);/
            Vector3 move = Vector3.MoveTowards(transform.position, goal, Time.deltaTime * Speed);
                move.y = 0;
            Debug.Log("Move Toward " + goal + "Actual move : " + move);
            //_controller.Move(move * Time.deltaTime * Speed);
            _controller.Move(Vector3.Normalize(move));
            if (move != Vector3.zero)
                    transform.forward = move;
            */
        }

        
    }

    /// <summary>
    /// From https://answers.unity.com/questions/599028/how-to-move-character-controller-from-point-to-poi.html
    /// </summary>
    /// <param name="targetPosition"></param>
    void MoveToPoint(Vector3 targetPosition)
    {
        if (targetPosition == transform.position)
            return;

        Vector3 moveDiff = targetPosition - transform.position;
        Vector3 moveDir = moveDiff.normalized * 15f * Time.deltaTime;
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            _controller.Move(moveDir * Speed);
        }
        else
        {
            _controller.Move(moveDiff * Speed);
        }
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
        Vector3 WorldPosition = otherObjLocaltransform.TransformDirection(otherObjLocaltransform.position);
        return localTransform.InverseTransformDirection(WorldPosition);
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
        return = localTransform.InverseTransformDirection(pointWorldSpace);
        //return in pointObjectSpace is now (2f, 0.5f, 0f)
        */
    }
}
