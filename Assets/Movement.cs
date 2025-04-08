using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float startSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private float accelerationTime = 150f; // czas w sekundach do osiągnięcia maxSpeed

    private float currentSpeed;
    private float accelerationTimer;

    private float maxJumpForce = 12f;
    private float minJumpForce = 4f;
    private float jumpChargeTime = 0.5f;
    private float maxJumpCharge = 1f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Slider jumpChargeSlider;
    [SerializeField] private Slider dashChargeSlider;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float jumpCharge;

    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float fastFallSpeed = 20f;


    private bool isDashing = false;
    private bool canDash = true;
    private float dashCharge;

    [SerializeField] private Animator animator;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentSpeed = startSpeed;
        accelerationTimer = 0f;

        if (jumpChargeSlider != null)
        {
            jumpChargeSlider.minValue = 0f;
            jumpChargeSlider.maxValue = maxJumpCharge;
            jumpChargeSlider.value = 0f;
        }

        if (dashChargeSlider != null)
        {
            dashChargeSlider.minValue = 0f;
            dashChargeSlider.maxValue = dashCooldown;
            dashChargeSlider.value = dashCooldown;
        }

        dashCharge = dashCooldown;
    }

    private void Update()
    {
        if (!isDashing)
        {
            // Zwiększanie prędkości z czasem
            if (currentSpeed < maxSpeed)
            {
                accelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(accelerationTimer / accelerationTime);
                currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);
            }

            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Ładowanie dasha
        if (!canDash)
        {
            dashCharge += Time.deltaTime;
            if (dashCharge >= dashCooldown)
            {
                dashCharge = dashCooldown;
                canDash = true;
            }

            if (dashChargeSlider != null)
            {
                dashChargeSlider.value = dashCharge;
            }
        }

        // Ładowanie skoku
        if (Input.GetButton("Jump") && isGrounded)
        {
            jumpCharge += Time.deltaTime / jumpChargeTime;
            jumpCharge = Mathf.Clamp(jumpCharge, 0f, maxJumpCharge);

            if (jumpChargeSlider != null)
            {
                jumpChargeSlider.value = jumpCharge;
            }
        }

        if (Input.GetButtonUp("Jump") && isGrounded)
        {
            float jumpPower = Mathf.Lerp(minJumpForce, maxJumpForce, jumpCharge);
            rb.velocity = new Vector2(3, jumpPower);
            jumpCharge = 0f;

            if (jumpChargeSlider != null)
            {
                jumpChargeSlider.value = 0f;
            }

            VolumeManager.TriggerSound("Jump");
        }

        // Dash
        if (!isGrounded && canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(PerformDash());
        }
        
        if (!isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
        }



        animator.SetFloat("JumpForce", rb.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;
        dashCharge = 0f;

        if (dashChargeSlider != null)
        {
            dashChargeSlider.value = 0f;
        }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashSpeed, 0f);

        VolumeManager.TriggerSound("Dash");
        GhostEffect.StartEffect();
        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        GhostEffect.StopEffect();
    }

    public void ResetSliders()
    {
        jumpCharge = 0f;
        dashCharge = dashCooldown;
        canDash = true;
        currentSpeed = startSpeed;
        accelerationTimer = 0f;

        if (jumpChargeSlider != null)
        {
            jumpChargeSlider.value = 0f;
        }

        if (dashChargeSlider != null)
        {
            dashChargeSlider.value = dashCooldown;
        }
        
        rb.velocity = new Vector2(0, 0);
    }
}

