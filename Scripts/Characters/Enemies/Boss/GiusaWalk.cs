using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiusaWalk : StateMachineBehaviour
{
    Giusa boss;
    public AudioClip giusaFight;
    public AudioClip giusaAttack;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.gameObject.GetComponent<Giusa>();
        boss.rb.bodyType = RigidbodyType2D.Dynamic;
        boss.SetState(State.chase);

        if (boss.soundtrack.GetComponent<AudioSource>().clip != giusaFight)
        {
            boss.soundtrack.GetComponent<AudioSource>().clip = giusaFight;
            boss.soundtrack.GetComponent<AudioSource>().Play();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.PlayerInRange(boss.attackRange) && boss.attackReady)
        {
            ChooseAttack(animator);
        }
        else
        {
            boss.MoveTo(boss.target.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    void ChooseAttack(Animator animator)
    {
        if (boss.health < boss.data.maxHealth * (1.0f / 3.0f)) // Se la vita è inferiore a 1/3
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    boss.SoundEffect(giusaAttack);
                    animator.SetTrigger("attack");
                    break;
                case 1:
                    animator.SetTrigger("charge");
                    break;
                case 2:
                    animator.SetTrigger("angry");
                    break;
            }
        }
        else if (boss.health < boss.data.maxHealth * (2.0f / 3.0f)) // Se la vita è inferiore a 2/3
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    boss.SoundEffect(giusaAttack);
                    animator.SetTrigger("attack");
                    break;
                case 1:
                    animator.SetTrigger("charge");
                    break;
            }
        }
        else
        {
            boss.SoundEffect(giusaAttack);
            animator.SetTrigger("attack");
        }
    }
}
