using Player;
using UnityEngine;
using Utils;

namespace GameElements.Obstacles
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20.0f;
        [SerializeField] private float lifeTime = 5.0f;

        private Vector3 _moveDirection;
        private Rigidbody _rb;
        private CharacterControlSwitcher _controlSwitcher;
        public Health sharedHealth;

        void Awake()
        {
            sharedHealth = FindAnyObjectByType<Health>();
            gameObject.tag = "EnemyBullet";
            Invoke(nameof(Kill), lifeTime);

            _controlSwitcher = FindAnyObjectByType<CharacterControlSwitcher>();
            _moveDirection = transform.up;

            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _moveDirection * (Time.fixedDeltaTime * speed));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "Player")
            {
                if (sharedHealth != null)
                {
                    Debug.Log("Player took dmg");
                    sharedHealth.TakeDamage(1);
                }
                Kill();
            }
            else if (other.name == "Companion")
            {
                if (sharedHealth != null)
                {
                    sharedHealth.TakeDamage(1);
                    Debug.Log("Companion took dmg");
                    if (_controlSwitcher != null)
                    {
                        _controlSwitcher.AutoSwitchToPlayer();
                    }
                }
                Kill();
            }
            else if (other.CompareTag("Wall") || other.CompareTag("Door") || other.CompareTag("Ground") || other.CompareTag("Door"))
            {
                Kill();
            }
        }

        void Kill()
        {
            Destroy(gameObject);
        }
    }
}