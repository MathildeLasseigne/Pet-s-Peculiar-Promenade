using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMoveManager : MonoBehaviour
{
    public float Speed = 0.3f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    public HandTracking handTracking;
    public float handDetectionRange = 3;
    public float handDetectionRangeMin = 0.3f;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


        _inputs = Vector3.zero;

        moveTowardHand();

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
        if (_inputs != Vector3.zero)
            transform.forward = _inputs;

        //_body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
        MoveToTarget(_inputs);
    }

    private void moveTowardHand()
    {

        _inputs = Vector3.zero;

        var FingerLeft = handTracking.getIndexObject(false);
        var FingerRight = handTracking.getIndexObject(true);

        bool hasFingers = true;

        Vector3 goal = Vector3.zero;
        if (! handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight))
        {
            goal = FingerRight.transform.TransformPoint(FingerRight.transform.position);
        } else if (handTracking.isFingerEnabled(FingerLeft) && !handTracking.isFingerEnabled(FingerRight))
        {
            goal = FingerLeft.transform.TransformPoint(FingerLeft.transform.position);
        } else if (handTracking.isFingerEnabled(FingerLeft) && handTracking.isFingerEnabled(FingerRight))
        {
            var FingerLeftWorldRelative = FingerLeft.transform.TransformPoint(FingerLeft.transform.position);
            var FingerRightWorldRelative = FingerRight.transform.TransformPoint(FingerRight.transform.position);

            //Choose the closest finger
            goal = Vector3.Distance(transform.position, FingerLeftWorldRelative) < Vector3.Distance(transform.position, FingerRightWorldRelative) ? FingerLeftWorldRelative : FingerRightWorldRelative;

        } else
        {
            hasFingers = false;
        }
        goal.y = 0;
        if (hasFingers)
        {
            if (Vector3.Distance(transform.position, goal) < handDetectionRange && Vector3.Distance(transform.position, goal) > handDetectionRangeMin)
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
    private void MoveToTarget(Vector3 targetPosition)
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
}
