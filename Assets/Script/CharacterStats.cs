using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("CharacterStats 초기화 완료");
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("무적 상태라 데미지 무시됨");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"피격! 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    public void SetInvincible(bool value)
    {
        isInvincible = value;
        Debug.Log($"무적 상태: {(value ? "ON" : "OFF")}");
    }

    void Die()
    {
        Debug.Log("캐릭터 사망 처리 실행");
        // 사망 관련 처리 (예: 게임오버, 리스폰 등)
    }
}
