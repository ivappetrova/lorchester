using UnityEngine;
using Utils;

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

            // Dormant until a ZoneController explicitly activates this shooter - no
            // per-frame work happens until then.
            enabled = false;
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

        // Called by ZoneController when the player crosses this zone's entrance/exit.
        public void SetActive(bool active)
        {
            Debug.Log($"[{name}] SetActive({active}) called - enabled was {enabled}");
            enabled = active;

            if (active)
            {
                _shootTimer = 0f;
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