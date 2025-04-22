using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float startSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    private readonly float _accelerationTime = 150f; // Czas (s) potrzebny do osiągnięcia maxSpeed
    private float _currentSpeed;
    private float _accelerationTimer;

    [Header("Jump Settings")]
    [SerializeField] private float maxJumpForce = 12f;
    [SerializeField] private float minJumpForce = 4f;
    [SerializeField] private float jumpChargeTime = 0.5f;
    [SerializeField] private float maxJumpCharge = 1f;
    private float _jumpCharge;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 2f;
    private bool _isDashing = false;
    private bool _canDash = true;
    private float _dashCharge;

    [Header("Fast Fall Settings")]
    [SerializeField] private float fastFallSpeed = 20f;
    private bool _didFastFall = false;

    [Header("UI Sliders")]
    [SerializeField] private Slider jumpChargeSlider;
    [SerializeField] private Slider dashChargeSlider;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private CameraController camController;

    private Rigidbody2D _rb;
    private bool _isGrounded;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentSpeed = startSpeed;
        _accelerationTimer = 0f;

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

        _dashCharge = dashCooldown;
    }

    private void Update()
    {
        if (!_isDashing)
        {
            // Stopniowe zwiększanie prędkości ruchu
            if (_currentSpeed < maxSpeed)
            {
                _accelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_accelerationTimer / _accelerationTime);
                _currentSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);
            }

            _rb.velocity = new Vector2(_currentSpeed, _rb.velocity.y);
        }

        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Odliczanie czasu do ponownego dashowania
        if (!_canDash)
        {
            _dashCharge += Time.deltaTime;
            if (_dashCharge >= dashCooldown)
            {
                _dashCharge = dashCooldown;
                _canDash = true;
            }

            if (dashChargeSlider != null)
                dashChargeSlider.value = _dashCharge;
        }

        // Ładowanie skoku
        if (Input.GetButton("Jump") && _isGrounded)
        {
            _jumpCharge += Time.deltaTime / jumpChargeTime;
            _jumpCharge = Mathf.Clamp(_jumpCharge, 0f, maxJumpCharge);

            if (jumpChargeSlider != null)
                jumpChargeSlider.value = _jumpCharge;
        }

        // Wykonanie skoku
        if (Input.GetButtonUp("Jump") && _isGrounded)
        {
            float jumpPower = Mathf.Lerp(minJumpForce, maxJumpForce, _jumpCharge);
            _rb.velocity = new Vector2(3, jumpPower);
            _jumpCharge = 0f;

            if (jumpChargeSlider != null)
                jumpChargeSlider.value = 0f;

            VolumeManager.TriggerSound("Jump");
        }

        // Dash w powietrzu
        if (!_isGrounded && _canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(PerformDash());
        }

        // Szybkie opadanie
        if (!_isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -fastFallSpeed);
            _didFastFall = true;
        }

        // Animacja po szybkim opadaniu
        if (_isGrounded && _didFastFall)
        {
            camController.ShakeCamera(0.1f, 6);
            _didFastFall = false;
        }

        animator.SetFloat("JumpForce", _rb.velocity.y);
        animator.SetBool("IsGrounded", _isGrounded);
    }

    /// <summary>
    /// Coroutine odpowiedzialna za wykonywanie dasha
    /// </summary>
    private IEnumerator PerformDash()
    {
        _isDashing = true;
        _canDash = false;
        _dashCharge = 0f;

        if (dashChargeSlider != null)
            dashChargeSlider.value = 0f;

        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(dashSpeed, 0f);

        VolumeManager.TriggerSound("Dash");
        GhostEffect.StartEffect();

        yield return new WaitForSeconds(dashDuration);

        _rb.gravityScale = originalGravity;
        _isDashing = false;
        GhostEffect.StopEffect();
    }

    /// <summary>
    /// Resetuje stany i suwaki do wartości początkowych
    /// </summary>
    public void ResetSliders()
    {
        _jumpCharge = 0f;
        _dashCharge = dashCooldown;
        _canDash = true;
        _currentSpeed = startSpeed;
        _accelerationTimer = 0f;

        if (jumpChargeSlider != null)
            jumpChargeSlider.value = 0f;

        if (dashChargeSlider != null)
            dashChargeSlider.value = dashCooldown;

        _rb.velocity = Vector2.zero;
    }
}

