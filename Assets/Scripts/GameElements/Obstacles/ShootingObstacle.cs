using Utils;
using UnityEngine;

namespace GameElements.Obstacles
{
    public class ShootingObstacle : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform bulletSocket;
        [SerializeField] private float shootInterval = 2.0f;
        [SerializeField] private SoundEmitter soundEmitter;

        private float _shootTimer;

        private void Awake()
        {
            if (soundEmitter == null)
                soundEmitter = GetComponent<SoundEmitter>();
        }

        private void Update()
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer <= 0.0f)
            {
                ShootProjectile();
                _shootTimer = shootInterval;
            }
        }

        private void ShootProjectile()
        {
            if (projectilePrefab != null && bulletSocket != null)
            {
                Instantiate(projectilePrefab, bulletSocket.position, bulletSocket.rotation);
                soundEmitter?.PlaySound();
            }
        }
    }
}