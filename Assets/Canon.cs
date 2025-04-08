using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 1f;
    private float _nextFireTime;

    public enum Direction { Left, Right }
    public Direction fireDirection = Direction.Left;

    private void Update()
    {
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 20)
        {
            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void Fire()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        Vector2 direction = (fireDirection == Direction.Left) ? -firePoint.right : firePoint.right;
        bullet.GetComponent<Bullet>().Launch(direction);

        VolumeManager.TriggerSound("CanonFire");
    }
}