using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiusaIdle : StateMachineBehaviour
{
    Giusa boss;
    public float time;
    float elapsedTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Giusa>();
        boss.SetState(State.idle);
        if (boss.rb.bodyType != RigidbodyType2D.Static)
            boss.rb.velocity = Vector2.zero;
        boss.rb.bodyType = RigidbodyType2D.Static;
        elapsedTime = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= time)
        {
            animator.SetTrigger("chase");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.hasPresented = true;
        boss.rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
