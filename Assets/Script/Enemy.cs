using UnityEngine;

public class Enemy : MonoBehaviour
{
    private MyCharacterController controller;
    private CharacterCombat combat;
    private Transform player;

    public float chaseRange = 5f;

    void Awake()
    {
        controller = GetComponent<MyCharacterController>();
        combat = GetComponent<CharacterCombat>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < chaseRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            controller.Move(new Vector2(Mathf.Sign(dir.x), 0));

            if (dist < 1.5f)
                combat.Attack();
        }
    }
}
