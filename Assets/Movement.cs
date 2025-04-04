using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private float moveSpeed = 2f;
    private float maxJumpForce = 10f;
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

    private bool isDashing = false;
    private bool canDash = true;
    private float dashCharge;

    [SerializeField] Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
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
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
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

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
    
    public void ResetSliders()
    {
        jumpCharge = 0f;
        dashCharge = dashCooldown;
        canDash = true;

        if (jumpChargeSlider != null)
        {
            jumpChargeSlider.value = 0f;
        }

        if (dashChargeSlider != null)
        {
            dashChargeSlider.value = dashCooldown;
        }
    }

}
