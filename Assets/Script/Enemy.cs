using UnityEngine;

// TODO: 몹은 디스트로이를 사용하여 사망 객체 삭제하는 방식을 사용 | 몹이 사용하는 무기 들은 오프젝트 풀링을 사용하여 활성화 -> 비활성화 식으로 성능 최적화(ex 벽에 화살 닿으면 비활성화 하고 다시 몹이 공격하면 몹에서 나오면서 활성화)
public class Enemy : MonoBehaviour
{
    public float health = 100f; // 몹의 체력
    public float damageThreshold = 1f; // 플레이어가 밟을 때 주는 최소 피해 크기
    public bool isDead = false; // 몹이 죽었는지 여부

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 플레이어가 내려서 밟았을 때
            if (collision.relativeVelocity.y < -damageThreshold)
            {
                // 플레이어가 떨어져서 밟은 경우
                Die();
            }
        }
    }

    // 몹이 죽을 때 호출되는 함수 
    private void Die()
    {
        isDead = true;
        // 사망 애니메이션 등 추가 작업
        Destroy(gameObject); // 몹 객체 삭제
    }

    // 몹에게 피해를 주는 함수 (아직 미사용)
    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
