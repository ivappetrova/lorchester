using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class ShootingObstacle : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform bulletSocket;
    [SerializeField] private float shootInterval = 2.0f;

    private float shootTimer = 0.0f;

    [SerializeField] private UnityEvent _shootEvent = null;
    private void Update()
    {
        shootTimer -= Time.deltaTime;

        //When timer reaches 0, fire a projectile
        if (shootTimer <= 0.0f)
        {
            ShootProjectile();
            shootTimer = shootInterval; // Reset timer
        }
    }

    //Method to shoot a projectile
    private void ShootProjectile()
    {
        if (projectilePrefab != null && bulletSocket != null)
        {
            _shootEvent?.Invoke();
            GameObject projectile = Instantiate(projectilePrefab, bulletSocket.position, bulletSocket.rotation);

        }
    }


}