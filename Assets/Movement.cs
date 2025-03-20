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
    private float maxJumpCharge = 1f; // Maksymalna wartość ładowania skoku
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Slider jumpChargeSlider;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpCharge;
    
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
    }

    private void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            isJumping = false;
        }

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
            isJumping = true;
            if (jumpChargeSlider != null)
            {
                jumpChargeSlider.value = 0f;
            }
        }
        
        animator.SetFloat("JumpForce", rb.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
    }
}
