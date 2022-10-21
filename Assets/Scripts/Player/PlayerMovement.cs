using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField, Range(0f, 0.5f)] private float groundCheckArea;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform[] leftPoints;
    [SerializeField] private Transform[] rightPoints;
    [SerializeField, Range(0f, 0.5f)] private float rayDistance;
    private bool grounded;

    [Header("Movement")]
    [SerializeField, Range(0f, 10f)] private float speed;
    private float moveDir;

    [Header("Jump")]
    [SerializeField, Range(2f, 6f)] private float minStrength;
    [SerializeField, Range(6f, 15f)] private float maxStrength;
    [SerializeField, Range(0f, 4f)] private float increaseRatio;
    [SerializeField, Range(0f, 2f)] private float speedMultiplier;
    [SerializeField, Range(0f, 2f)] private float div;
    private float forceToApply = 0f;
    private float jumpInput;
    private bool charging;

    public void MoveInput(InputAction.CallbackContext value) => moveDir = value.ReadValue<float>();

    public void JumpInput(InputAction.CallbackContext value) => jumpInput = value.ReadValue<float>();

    private void FixedUpdate()
    {
        checkGround();
        if (!grounded)
        {
            checkWalls();
            return;
        }

        if (jumpInput == 1)
            chargeJump();
        else if ((jumpInput == 0 && charging))
            jump();
        else
            move();
    }

    private void checkGround() => grounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckArea, groundMask);

    private void checkWalls()
    {
        bool hit = false;
        foreach (Transform point in leftPoints)
            hit = Physics2D.Raycast(point.position, Vector2.left, rayDistance, groundMask) || hit;
        foreach (Transform point in rightPoints)
            hit = Physics2D.Raycast(point.position, Vector2.right, rayDistance, groundMask) || hit;
        if (hit)
            rb.velocity = new Vector2(-rb.velocity.x / div, (rb.velocity.y > 0f)? rb.velocity.y / div : rb.velocity.y);
    }

    private void chargeJump()
    {
        charging = true;
        if (forceToApply == 0f)
            forceToApply = minStrength;
        else
            forceToApply = Mathf.Clamp(forceToApply + increaseRatio * Time.fixedDeltaTime, minStrength, maxStrength);
        
        if (forceToApply == maxStrength)
            jumpInput = 0;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void jump()
    {
        charging = false;
        rb.velocity = new Vector2(speed *  moveDir * speedMultiplier, forceToApply);
        forceToApply = 0f;
    }

    private void move()
    {
        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }
}