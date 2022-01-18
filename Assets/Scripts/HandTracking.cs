using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

/// <summary>
/// Source : https://xrlab.dev/custom-hand-joint-tracking-for-hololens/
/// </summary>
public class HandTracking : MonoBehaviour
{
    public GameObject sphereMarker;

    GameObject thumbObjectRight;
    GameObject indexObjectRight;
    GameObject middleObjectRight;
    GameObject ringObjectRight;
    GameObject pinkyObjectRight;

    GameObject thumbObjectLeft;
    GameObject indexObjectLeft;
    GameObject middleObjectLeft;
    GameObject ringObjectLeft;
    GameObject pinkyObjectLeft;

    MixedRealityPose pose;

    private bool showObjects = true;

    void Start()
    {
        thumbObjectRight = Instantiate(sphereMarker, this.transform);
        indexObjectRight = Instantiate(sphereMarker, this.transform);
        middleObjectRight = Instantiate(sphereMarker, this.transform);
        ringObjectRight = Instantiate(sphereMarker, this.transform);
        pinkyObjectRight = Instantiate(sphereMarker, this.transform);

        thumbObjectLeft = Instantiate(sphereMarker, this.transform);
        indexObjectLeft = Instantiate(sphereMarker, this.transform);
        middleObjectLeft = Instantiate(sphereMarker, this.transform);
        ringObjectLeft = Instantiate(sphereMarker, this.transform);
        pinkyObjectLeft = Instantiate(sphereMarker, this.transform);
    }

    void Update()
    {

        thumbObjectRight.GetComponent<Renderer>().enabled = false;
        indexObjectRight.GetComponent<Renderer>().enabled = false;
        middleObjectRight.GetComponent<Renderer>().enabled = false;
        ringObjectRight.GetComponent<Renderer>().enabled = false;
        pinkyObjectRight.GetComponent<Renderer>().enabled = false;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Right, out pose))
        {
            if (showObjects)
            {
                thumbObjectRight.GetComponent<Renderer>().enabled = true;
            }
            thumbObjectRight.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out pose))
        {
            if (showObjects)
                indexObjectRight.GetComponent<Renderer>().enabled = true;
            indexObjectRight.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Right, out pose))
        {
            if (showObjects)
                middleObjectRight.GetComponent<Renderer>().enabled = true;
            middleObjectRight.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Right, out pose))
        {
            if (showObjects)
                ringObjectRight.GetComponent<Renderer>().enabled = true;
            ringObjectRight.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Right, out pose))
        {
            if (showObjects)
                pinkyObjectRight.GetComponent<Renderer>().enabled = true;
            pinkyObjectRight.transform.position = pose.Position;
        }


        thumbObjectLeft.GetComponent<Renderer>().enabled = false;
        indexObjectLeft.GetComponent<Renderer>().enabled = false;
        middleObjectLeft.GetComponent<Renderer>().enabled = false;
        ringObjectLeft.GetComponent<Renderer>().enabled = false;
        pinkyObjectLeft.GetComponent<Renderer>().enabled = false;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Left, out pose))
        {
            if (showObjects)
                thumbObjectLeft.GetComponent<Renderer>().enabled = true;
            thumbObjectLeft.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left, out pose))
        {
            if (showObjects)
                indexObjectLeft.GetComponent<Renderer>().enabled = true;
            indexObjectLeft.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Left, out pose))
        {
            if (showObjects)
                middleObjectLeft.GetComponent<Renderer>().enabled = true;
            middleObjectLeft.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Left, out pose))
        {
            if (showObjects)
                ringObjectLeft.GetComponent<Renderer>().enabled = true;
            ringObjectLeft.transform.position = pose.Position;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Left, out pose))
        {
            if (showObjects)
                pinkyObjectLeft.GetComponent<Renderer>().enabled = true;
            pinkyObjectLeft.transform.position = pose.Position;
        }

    }

    public GameObject getThumbObject(bool rightHand)
    {
        if (rightHand)
        {
            return thumbObjectRight;
        } else
        {
            return thumbObjectLeft;
        }
    }

    public GameObject getIndexObject(bool rightHand)
    {
        if (rightHand)
        {
            return indexObjectRight;
        }
        else
        {
            return indexObjectLeft;
        }
    }

    public GameObject getMiddleObject(bool rightHand)
    {
        if (rightHand)
        {
            return middleObjectRight;
        }
        else
        {
            return middleObjectLeft;
        }
    }

    public GameObject getRingObject(bool rightHand)
    {
        if (rightHand)
        {
            return ringObjectRight;
        }
        else
        {
            return ringObjectLeft;
        }
    }

    public GameObject getPinkyObject(bool rightHand)
    {
        if (rightHand)
        {
            return pinkyObjectRight;
        }
        else
        {
            return pinkyObjectLeft;
        }
    }

    public void showObject(bool show)
    {
        this.showObjects = show;
    }
}
