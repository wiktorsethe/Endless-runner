using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D _rb;
    [SerializeField] private ParticleSystem particles;
    public static event Action OnBulletTouched;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector3 direction)
    {
        gameObject.SetActive(true);
        _rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")) OnBulletTouched?.Invoke();
        Instantiate(particles, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
