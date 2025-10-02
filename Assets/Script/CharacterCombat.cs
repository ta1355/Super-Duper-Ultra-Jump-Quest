using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public Weapon weapon;
    private Animator animator;
    private bool isAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetBool("isAttacking", true);


        if (weapon != null)
        {
            weapon.Activate();
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;

        animator.SetBool("isAttacking", false);

        if (weapon != null)
        {
            weapon.Deactivate();
        }
    }
}
