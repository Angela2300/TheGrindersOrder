using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 move =
            (Keyboard.current.dKey.isPressed ? Vector2.right : Vector2.zero) +
            (Keyboard.current.aKey.isPressed ? Vector2.left : Vector2.zero) +
            (Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero) +
            (Keyboard.current.sKey.isPressed ? Vector2.down : Vector2.zero);

        rb.MovePosition(rb.position + move.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}