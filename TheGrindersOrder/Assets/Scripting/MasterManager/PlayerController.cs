using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;
    public float rotationOffset = -90f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerStats playerStats;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        ReadMovementInput();
        RotateToMouse();
    }

    void FixedUpdate()
    {
        float finalSpeed = moveSpeed;

        if (playerStats != null)
        {
            finalSpeed *= playerStats.speedMultiplier;
        }

        rb.MovePosition(rb.position + moveInput * finalSpeed * Time.fixedDeltaTime);
    }

    void ReadMovementInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        moveInput = moveInput.normalized;
    }

    void RotateToMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorld.z = 0;

        Vector2 direction = (Vector2)mouseWorld - rb.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;

        rb.rotation = angle;
    }
}