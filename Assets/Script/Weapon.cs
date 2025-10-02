using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;
    private bool canHit = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canHit) return;

        CharacterStats stats = collision.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.TakeDamage(damage);
            canHit = false; // 여러 번 맞는 것 방지
        }
    }

    public void Activate()
    {
        canHit = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        canHit = false;
        gameObject.SetActive(false);
    }
}
