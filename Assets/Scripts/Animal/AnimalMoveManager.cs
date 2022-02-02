using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;

enum Hand { Left, Right, None}
enum RangeComparator { Close, InRange, Far, Other }
class HandSelection
{
    public HandTracking handTracking { get; }
    public Hand handedness { get; private set; }

    public RangeComparator rangePosition = RangeComparator.Other;

    public HandSelection(HandTracking handTracking)
    {
        this.handTracking = handTracking;
        handedness = Hand.None;
    }

    /// <summary>
    /// Check if a hand is currently selected
    /// </summary>
    /// <returns></returns>
    public bool HasHands()
    {
        return handedness != Hand.None;
    }

    /// <summary>
    /// Check if the current hand selected is the right one.
    /// </summary>
    /// <returns>False if it is the left hand or no hand is selected</returns>
    public bool isRightHand()
    {
        return handedness == Hand.Right;
    }

    /// <summary>
    /// Set the handedness. If is None, set the rangePosition to Other
    /// </summary>
    /// <param name="handedness"></param>
    public void setHandedness(Hand handedness)
    {
        if(handedness == Hand.None)
        {
            rangePosition = RangeComparator.Other;
        }
        this.handedness = handedness;
    }
}

[RequireComponent(typeof(CharacterController))]
public class AnimalMoveManager : MonoBehaviour
{
    public bool debug = false;

    [SerializeField]
    private GameObject AnimalBody;
    [SerializeField]
    private Animator animator;

    [Header("Move properties")]
    public float Speed = 1f;
    public float JumpHeight = 2f;
    public float DashDistance = 5f;
    

    private Vector3 _velocity;
    public Vector3 Drag;
    public float Gravity = -9.81f;

    [Header("Ground checker")]
    [SerializeField]
    ///Object at the bottom of the character
    private Transform _groundChecker;
    public LayerMask Ground;
    public float GroundDistance = 0.2f;

    private bool _isGrounded = true;

    [Header("Hand properties")]
    public HandTracking handTracking;
    public Vector2 handDetectionRange = new Vector2(0.5f, 3.5f);
    private HandSelection handSelection;

    private Vector3 currentTarget = Vector3.zero; //The position of the index of the current hand selected. Is Vector3.zero if no target is selected

    [Header("CharacterController")]
    private CharacterController _controller;

    [Header("Sitting area")]
    private bool isSitting = false;
    private bool sittingLock = false;

    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        handSelection = new HandSelection(handTracking);
    }

    void Update()
    {
        CalculateNearestHand();

        if (_groundChecker)
        {
            _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        } else
        {
            _isGrounded = _controller.isGrounded;
        }
        

        if (_controller)
        {
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0f;

            moveControllerTowardHand();
            AddGravityAndFriction(false);
        }

        

    }




    //------------------------------------------------------------CHARACTER CONTROLLER----------------------------------------------------------------------------------------------------------//








    private void moveControllerTowardHand()
    {

        Vector3 goal = this.currentTarget;
        goal.y = 0;
        if (this.handSelection.HasHands() && goal != Vector3.zero)
        {
            if (this.handSelection.rangePosition == RangeComparator.InRange)
            {
                GetUp(false);
                if(!isSitting)
                {
                    Debug.Log("isSitting = " + isSitting + "Animator sitting : " + animator.GetBool("isSitting"));
                    MoveControllerToPoint(goal);
                }

            }
            else
            {
                Sit(false);
            }
        } else {
            Sit(false);
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
            move.y = 0;
            if (move != Vector3.zero)
            {
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


    public void Jump()
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
            this.handSelection.setHandedness(Hand.Right);
            goal = IntoLocalCoord(transform, FingerRight.transform);
        }
        else if (this.handSelection.handTracking.isFingerEnabled(FingerLeft) && !this.handSelection.handTracking.isFingerEnabled(FingerRight)) //Only left hand
        {
            this.handSelection.setHandedness(Hand.Left);
            goal = IntoLocalCoord(transform, FingerLeft.transform);
        }
        else if (this.handSelection.handTracking.isFingerEnabled(FingerLeft) && this.handSelection.handTracking.isFingerEnabled(FingerRight)) //Both hands
        {
            var FingerLeftRelative = IntoLocalCoord(transform, FingerLeft.transform);
            var FingerRightRelative = IntoLocalCoord(transform, FingerRight.transform);

            //Choose the closest finger
            if(Vector3.Distance(transform.position, FingerLeftRelative) < Vector3.Distance(transform.position, FingerRightRelative)) {
                this.handSelection.setHandedness(Hand.Left);
                goal = IntoLocalCoord(transform, FingerLeft.transform);
            } else {
                this.handSelection.setHandedness(Hand.Right);
                goal = IntoLocalCoord(transform, FingerRight.transform);
            }

        }
        else
        {
            this.handSelection.setHandedness(Hand.None);
            hasFingers = false;
        }
        if (hasFingers)
        {
            if (Vector3.Distance(transform.position, goal) < handDetectionRange.x)
            {
                this.handSelection.rangePosition = RangeComparator.Close;
            } else if(Vector3.Distance(transform.position, goal) > handDetectionRange.y)
            {
                this.handSelection.rangePosition = RangeComparator.Far;
            } else
            {
                this.handSelection.rangePosition = RangeComparator.InRange;
            }
        }
        if(debug)
            Debug.Log("Hand : " + this.handSelection.handedness + " Position : " + this.handSelection.rangePosition);

        this.currentTarget = goal;
    }

    public void Sit(bool lockSit)
    {
        if (!isSitting)
        {
            if (lockSit)
            {
                this.sittingLock = true;
            }
            this.isSitting = true;
            if (animator)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isSitting", true);
            }
        }
    }

    public void GetUp(bool unlockSit)
    {
        if (isSitting)
        {
            if (unlockSit)
            {
                this.sittingLock = false;
            }
            if (!this.sittingLock)
            {
                if (animator)
                {
                    /*animator.SetBool("isSitting", false);
                    animator.SetBool("isWalking", true);
                    */
                    StartCoroutine(GetUpRoutine());
                } else
                {
                    this.isSitting = false;
                }
            }
        }
    }

    private IEnumerator GetUpRoutine()
    {
        animator.SetBool("isSitting", false);
        animator.SetBool("isWalking", true);

        yield return new WaitWhile(() => animator.IsPlayingAnimation("Base Layer.GetUp", 0) || animator.IsPlayingAnimation("Base Layer.Sitting", 0) || animator.IsPlayingAnimation("Base Layer.Sit", 0) || animator.IsInTransition(0));

        this.isSitting = false;
    }

}
