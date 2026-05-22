using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator animator;
    [SerializeField] Transform GameOverDropLocation;
    [SerializeField] Transform cameraTransform;
    [Header("Settings")]
    [SerializeField] float followSpeed = 5f;
    [SerializeField] float maxXVelocity = 4f;
    [SerializeField] float gravity = 1f;
    [SerializeField] float groundedDistance = 0.1f;
    [SerializeField] float maxYVelocity = 6f;
    [SerializeField] float jumpVelocity = 6f;
    [SerializeField] float bounceVelocity = 6f;
    [SerializeField] float screenShakeDuration = .2f;
    [SerializeField] float screenShakeAmount = .05f;
    Vector2 velocity = Vector2.zero;
    bool isGrounded = true;
    bool needsToJump = false;
    float scaleX;
    bool jumpEnabled = false;
    bool shouldCameraShake = false;


    // Start is called before the first frame update
    void Start()
    {
        scaleX = Mathf.Abs(transform.localScale.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool wasGroundedBeforeFrame = isGrounded;
        isGrounded = IsGrounded();

        if (isGrounded != wasGroundedBeforeFrame && isGrounded && shouldCameraShake)
        {
            StartCoroutine(CameraShake());
        }

        if (!isGrounded && !needsToJump)
        {
            DoGravity();
        }
        else
        {
            if (!needsToJump)
                velocity.y = 0f;
            else if (jumpEnabled)
                Jump();
        }

        FollowCursor();
        Move();
    }

    private void Update()
    {
        SetAnimatorParameters();

        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            needsToJump = true;
        }
    }

    void DoGravity()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -maxYVelocity, maxYVelocity);
        
    }

    void Jump()
    {
        velocity.y = jumpVelocity;
        needsToJump = false;
        animator.SetTrigger("Jump");
    }

    void FollowCursor()
    {
        float cursorPosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float playerPosX = transform.position.x;
        float direction = cursorPosX - playerPosX;
        float xVelocity = direction/2 * followSpeed;
        xVelocity = Mathf.Clamp(xVelocity, -maxXVelocity, maxXVelocity);
        if(xVelocity > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = scaleX;
            transform.localScale = scale;
        }
        if(xVelocity < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -scaleX;
            transform.localScale = scale;
        }

        velocity.x = xVelocity;
    }

    void Move()
    {
        rb.linearVelocity = velocity;
        needsToJump = false;
    }

    bool IsGrounded()
    {
        float checkDistance = collider.bounds.extents.y + groundedDistance;
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, Vector2.down, checkDistance, groundLayer);
        Debug.DrawRay(collider.bounds.center, Vector2.down * checkDistance, Color.red);
        
        return hit.collider != null;
    }

    IEnumerator CameraShake()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = cameraTransform.position;

        while (elapsedTime < screenShakeDuration)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f) * screenShakeAmount,
                Random.Range(-1f, 1f) * screenShakeAmount,
                0f
            );
            cameraTransform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
        shouldCameraShake = false;
    }

    public void Bounce()
    {
        velocity.y = bounceVelocity;
        animator.SetTrigger("Jump");
    }

    void SetAnimatorParameters()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(velocity.x) / 20f);
        animator.SetFloat("VerticalSpeed", Mathf.Abs(velocity.y) / 20f);
    }

    public void CheckStateAndDisableControls()
    {
        jumpEnabled = GameManager.Instance.CurrentState == GameManager.State.Playing;
    }

    public void MovePlayerOnGameOver()
    {

        if (GameManager.Instance.CurrentState == GameManager.State.GameOver)
        {
            if (transform.position.y > GameOverDropLocation.position.y)
            {
                transform.position = GameOverDropLocation.position;
                shouldCameraShake = true;
            }
        }
    }
}
