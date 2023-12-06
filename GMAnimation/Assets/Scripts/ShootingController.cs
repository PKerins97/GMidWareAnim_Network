using System.Collections;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunTransform;
    public float bulletSpeed = 10f;
    public float shootDelay = 0.5f; // Set the delay before shooting

    private bool canShoot = true; // Variable to control shooting

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            StartCoroutine(ShootWithDelay());
        }
    }

    IEnumerator ShootWithDelay()
    {
        canShoot = false; // Disable shooting temporarily

        // Wait for the specified delay before shooting
        yield return new WaitForSeconds(shootDelay);

        Shoot();

        canShoot = true; // Enable shooting again
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            // Use the gun's rotation to determine the bullet's direction
            bulletRb.velocity = gunTransform.up * bulletSpeed;
        }

        StartCoroutine(DestroyBulletAfterDelay(bullet, 2f));
    }

    IEnumerator DestroyBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
