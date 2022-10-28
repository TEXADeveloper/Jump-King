using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField, Range(0f, 0.5f)] private float groundCheckArea;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform[] leftPoints;
    [SerializeField] private Transform[] rightPoints;
    [SerializeField] private Transform[] bottomPoints;
    [SerializeField, Range(0f, 0.5f)] private float rayDistance;
    private bool grounded;
    private bool inGame;

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

    private void canStart() => inGame = true;

    void Start()
    {
        Introduction.StartGame += canStart;
    }

    private void FixedUpdate()
    {
        if (!inGame)
            return;

        checkGround();
        if (!grounded)
        {
            checkWalls();
            anim.SetBool("Walking", false);
            anim.SetBool("Charging", false);
            return;
        }

        if (jumpInput == 1)
            chargeJump();
        else if ((jumpInput == 0 && charging))
            jump();
        else
            move();
    }

    private void checkGround()
    {
        bool hit = false;
        foreach (Transform point in bottomPoints)
            hit = Physics2D.Raycast(point.position, Vector2.down, groundCheckArea, groundMask) || hit;
        grounded = hit;
        anim.SetBool("InAir", !grounded);
    }

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

        anim.SetBool("Walking", false);
        anim.SetBool("Charging", true);

        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void jump()
    {
        charging = false;
        rb.velocity = new Vector2(speed *  moveDir * speedMultiplier, forceToApply);
        forceToApply = 0f;
        anim.SetBool("Charging", false);
    }

    private void move()
    {
        anim.SetBool("Walking", (moveDir != 0));
        sr.flipX = moveDir == -1;
        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {

        foreach (Transform point in leftPoints)
            Gizmos.DrawLine(point.position, point.position + new Vector3(-rayDistance, 0f, 0f));
        foreach (Transform point in rightPoints)
            Gizmos.DrawLine(point.position, point.position + new Vector3(rayDistance, 0f, 0f));
        foreach (Transform point in bottomPoints)
            Gizmos.DrawLine(point.position, point.position + new Vector3(0f, -groundCheckArea, 0f));
    }

    void OnDisable()
    {
        Introduction.StartGame -= canStart;
    }
}