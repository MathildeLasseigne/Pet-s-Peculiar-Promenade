using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Source : https://answers.unity.com/questions/362629/how-can-i-check-if-an-animation-is-being-played-or.html
/// </summary>
public static class AnimationUtilities
{
    /// <summary>
    /// Check if the layer is playing
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    public static bool IsPlayingLayer(this Animator animator, int layerIndex)
    {
        return animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime % 1.0f < 1.0f;
    }

    public static bool IsPlayingLayer(this Animator animator, string layerName)
    {
        return animator.IsPlayingLayer(animator.GetLayerIndex(layerName));
    }

    /// <summary>
    /// Check if a certain animation is playing
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animationName"></param>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    public static bool IsPlayingAnimation(this Animator animator, string animationName, int layerIndex)
    {
        return animator.IsPlayingLayer(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationName);
    }

    public static bool IsPlayingAnimation(this Animator animator, string animationName, string layerName)
    {
        return animator.IsPlayingAnimation(animationName, animator.GetLayerIndex(layerName));
    }

    public static bool IsPlayingAnyLayer(this Animator animator)
    {
        for (int i = 0; i < animator.layerCount; i++)
            if (animator.IsPlayingLayer(i)) return true;
        return false;
    }
}
