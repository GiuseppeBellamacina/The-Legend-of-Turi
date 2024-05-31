using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiusaStagger : StateMachineBehaviour
{
    Giusa boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Giusa>();
        if (boss.rb.bodyType != RigidbodyType2D.Static)
            boss.rb.velocity = Vector2.zero;
        boss.rb.bodyType = RigidbodyType2D.Static;
        boss.isInvulnerable = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1)
        {
            animator.SetTrigger("chase");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.rb.bodyType = RigidbodyType2D.Dynamic;
        boss.isInvulnerable = false;
    }
}
