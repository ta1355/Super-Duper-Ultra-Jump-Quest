using UnityEngine;

public class Warrior : MonoBehaviour
{
    private MyCharacterController controller; 
    private CharacterCombat combat;

    void Awake()
    {
        controller = GetComponent<MyCharacterController>();
        combat = GetComponent<CharacterCombat>();
    }

    void Update()
    {
        // 입력 처리
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        controller.Move(input);

        if (Input.GetKeyDown(KeyCode.Space)) 
            controller.Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
            controller.Dash();

        if (Input.GetKeyDown(KeyCode.Z)) 
            combat.Attack();
    }
}
