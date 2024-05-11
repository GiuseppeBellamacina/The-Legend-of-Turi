using UnityEngine;

public class Breakable : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Smash()
    {
        animator.SetBool("isSmashed", true);
        Destroy(gameObject, 0.5f);
    }
}
