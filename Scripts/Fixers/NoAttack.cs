using UnityEngine;

public class NoAttack : MonoBehaviour
{
    void Start()
    {
        if (!PlayerController.Instance.inventory.hasSword)
        {
            AttackDisable();
        }
        Destroy(gameObject);
    }

    void AttackDisable()
    {
        PlayerController.Instance.DisableAttack();
        PlayerController.Instance.weapon.color = new Color(1, 1, 1, 0);
    }
}