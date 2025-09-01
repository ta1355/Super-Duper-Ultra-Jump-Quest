using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int damage = 1; // 뎀지(나중에 강화나 이런 요소 있을 수 있어서 일단 1 나중에 추가 수치 하기 or 무기 변경 다른 무기 사용하게)
    private bool canHit = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canHit)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                canHit = false; // 한 번 히트하면 다시 히트 못하게 설정(여러번 히트 불가) <- 이거 안하면 너무 사기...
            }
        }
    }

    public void Activate()
    {
        canHit = true;
        Debug.Log("Weapon GameObject 활성화됨");
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        canHit = false;
        gameObject.SetActive(false);
    }

}
