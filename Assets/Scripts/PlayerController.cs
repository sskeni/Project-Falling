using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Rigidbody2D Body;
    private SpriteRenderer SRenderer;
    public float moveSpeed;
    public float jumpSpeed;
    public float pickupBoostSpeed;
    private float jumpMultiplier;

    private float groundCheckRadius;
    public Transform GroundCheckTransform;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool isFlying = false;

    public float milestoneThreshold;
    public float milestoneChangeMultiplier;
    public float milestoneDragChange;
    public float milestoneJumpMultiplierChange;
    private float milestoneCounter;

    public Sprite PlayerFalling;
    public Sprite PlayerFlying;

    private void Awake()
    {
        CheckSingleton();
    }

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        SRenderer = GetComponent<SpriteRenderer>();
        SRenderer.sprite = PlayerFalling;
        milestoneCounter = milestoneThreshold;
        groundCheckRadius = 0.2f;
        jumpMultiplier = 1;
    }

    void Update()
    {
        // Adjust fall speed based on the Milestone
        if (transform.position.y < -milestoneCounter) IncrementMilestone();

        // Check ground
        isGrounded = Physics2D.OverlapCircle(GroundCheckTransform.position, groundCheckRadius, groundLayer);

        if (!GameManager.instance.GameOver())
        {
            // Update jump based on jump milestone
            if (Body.velocity.y < 0) jumpMultiplier += milestoneJumpMultiplierChange * Time.deltaTime;

            // Jump once hit platform
            if (isGrounded) Jump();

            // Make Player fall once game over
            if (Body.velocity.y < 0 && isFlying) GameOver();

            // Move the player
            if (Input.GetMouseButton(0)) Move();
        }

        // Flip player based on movement
        if (Body.velocity.x < 0)
        {
            transform.rotation = new Quaternion(transform.rotation.x, 0f, transform.rotation.z, transform.rotation.w);
        }
        if (Body.velocity.x > 0)
        {
            transform.rotation = new Quaternion(transform.rotation.x, 180f, transform.rotation.z, transform.rotation.w);
        }
    }

    private void Move()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert mouse coords to world space
        float moveVelocity = mouseWorldPosition.x - transform.position.x;
        Body.velocity = new Vector2(moveVelocity * moveSpeed, Body.velocity.y);
    }

    private void Jump()
    {
        SRenderer.sprite = PlayerFlying;
        Body.velocity = new Vector2(Body.velocity.x, jumpSpeed * jumpMultiplier);
        pickupBoostSpeed = jumpSpeed * jumpMultiplier * 0.5f;
        Body.drag = 1f;
        Body.gravityScale = 0.1f;
        isFlying = true;
    }

    private void IncrementMilestone()
    {
        milestoneCounter += milestoneThreshold;
        milestoneThreshold *= milestoneChangeMultiplier;
        Body.drag *= milestoneDragChange;
    }

    public float getJumpMultiplier()
    {
        return jumpMultiplier;
    }

    public bool getIsFlying()
    {
        return isFlying;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickup" && !GameManager.instance.GameOver())
        {
            Body.velocity = new Vector2(Body.velocity.x, pickupBoostSpeed);
            pickupBoostSpeed *= 0.9f; // Lessen the jump after consecutive pickups
        }
    }

    private void GameOver()
    {
        SRenderer.sprite = PlayerFalling;
        Body.gravityScale = 1f;
        GameManager.instance.OnGameOver();
    }

    private void CheckSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
}
