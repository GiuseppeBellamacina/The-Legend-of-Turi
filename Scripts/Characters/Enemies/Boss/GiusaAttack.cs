using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiusaAttack : StateMachineBehaviour
{
    Giusa boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Giusa>();
        boss.attackReady = false;
        boss.isInvulnerable = true;
        if (boss.rb.bodyType != RigidbodyType2D.Static)
            boss.rb.velocity = Vector2.zero;
        boss.rb.bodyType = RigidbodyType2D.Static;
        boss.SetState(State.attack);
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
        boss.AttackCooldown();
        boss.isInvulnerable = false;
        boss.rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
