using System.Collections.Generic;
using Utils;
using UnityEngine;

namespace Weapon
{
    public class BasicWeapon : MonoBehaviour
    {
        [SerializeField] private GameObject bulletTemplate;
        [SerializeField] private float fireRate = 25.0f;
        [SerializeField] private List<Transform> fireSockets = new List<Transform>();
        [SerializeField] private SoundEmitter soundEmitter;

        private bool _triggerPulled;
        private float _fireTimer;

        private void Awake()
        {
            if (soundEmitter == null)
                soundEmitter = GetComponent<SoundEmitter>();
        }

        private void Update()
        {
            if (_fireTimer > 0.0f) _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0.0f && _triggerPulled) FireProjectile();

            _triggerPulled = false;
        }

        private void FireProjectile()
        {
            if (bulletTemplate == null) return;

            foreach (var t in fireSockets)
            {
                Instantiate(bulletTemplate, t.position, t.rotation);
            }

            _fireTimer += 1.0f / fireRate;

            soundEmitter?.PlaySound();
        }

        public void Fire()
        {
            _triggerPulled = true;
        }
    }
}