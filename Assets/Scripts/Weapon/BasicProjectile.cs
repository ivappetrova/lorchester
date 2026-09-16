using GameElements;
using UnityEngine;

namespace Weapon
{
    public class BasicProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 30.0f;
        [SerializeField] private float lifeTime = 10.0f;

        private Vector3 _moveDirection;

        void Awake()
        {
            gameObject.tag = "Bullet"; 
            Invoke(nameof(Kill), lifeTime); // Destroy the bullet after its lifetime

            _moveDirection = transform.up; 
        }

        void FixedUpdate()
        {
            //Move the bullet
            transform.position += _moveDirection * (Time.deltaTime * speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Destructible"))
            {
                var destructible = other.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.Break();
                    Kill(); // Destroy the bullet
                }
            }

            // If the bullet hits a wall, door, or ground, destroy it
            else if (other.CompareTag("Wall") || other.CompareTag("Door") || other.CompareTag("Ground"))
            {
                Kill(); 
            }

            else if (other.CompareTag("Player") && other.gameObject.name == "Companion")
            {
                // Intentionally do nothing
            }
        }

        void Kill()
        {
            Destroy(gameObject); // Destroy the bullet object
        }
    }
}
