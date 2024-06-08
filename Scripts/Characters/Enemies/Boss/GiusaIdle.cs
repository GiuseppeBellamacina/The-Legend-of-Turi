using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GiusaIdle : StateMachineBehaviour
{
    Giusa boss;
    public BoolValue hasPresented;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Giusa>();
        if (boss.isDead.value)
            return;

        boss.SetState(State.idle);
        if (boss.rb.bodyType != RigidbodyType2D.Static)
            boss.rb.velocity = Vector2.zero;
        boss.rb.bodyType = RigidbodyType2D.Static;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.isDead.value)
            return;

        if (hasPresented.value)
        {
            boss.spriteRenderer.flipX = !boss.spriteRenderer.flipX;
            animator.SetTrigger("chase");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.isDead.value)
            return;
            
        boss.rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
