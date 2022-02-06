using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://www.youtube.com/watch?v=9cEiueuwGuM&list=PLmBprbjofP9hR67LdjKaJqKWOIwBr9a9s&index=18
/// </summary>
public class JumpManager : MonoBehaviour
{

    private enum TriggerEnum
    {
        None,
        prepJump,
        upJump,
        downJump,
        hitJump,
    }

    private AnimalMoveManager moveManager;

    public bool isJumping { get; private set; }

    [SerializeField]
    private Animator animator;

    private TriggerEnum currentTrigger = TriggerEnum.None;


    // Start is called before the first frame update
    void Start()
    {
        moveManager = GetComponent<AnimalMoveManager>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping)
        {
            if (currentTrigger == TriggerEnum.upJump && moveManager.getVelocity().y < 0f)
            {
                //Down
                //Debug.Log("down jump");
                currentTrigger = TriggerEnum.None;
                AnimTrigger(TriggerEnum.downJump);
                currentTrigger = TriggerEnum.downJump;
            }

            if (currentTrigger == TriggerEnum.downJump && moveManager._isGrounded) //touch the ground
            {
                isJumping = false;
                //Debug.Log("hit jump");
                currentTrigger = TriggerEnum.None;
                AnimTrigger(TriggerEnum.hitJump);
                //ResetTriggers();

            }
        }

    }

    public void PrepJump()
    {
        if(isJumping == false)
        {
            //Debug.Log("Prep jump");
            isJumping = true;
            currentTrigger = TriggerEnum.prepJump;
            AnimTrigger(TriggerEnum.prepJump);
            /*ResetTriggers();
            animator.SetTrigger("prepJump");*/
            animator.SetBool("isSitting", false);
        }
        
    }

    public void Jump()
    {
        if (currentTrigger == TriggerEnum.prepJump && isJumping)
        {
            //Debug.Log("up jump");
            currentTrigger = TriggerEnum.upJump;
            AnimTrigger(TriggerEnum.upJump);
            /*ResetTriggers();
            animator.SetTrigger("upJump");*/
        }
    }


    private void AnimTrigger(TriggerEnum code)
    {
        ResetTriggers();
        animator.SetTrigger(code.ToString());

    }

    private void ResetTriggers()
    {
        animator.ResetTrigger("prepJump");
        animator.ResetTrigger("upJump");
        animator.ResetTrigger("downJump");
        animator.ResetTrigger("hitJump");
    }
}
