using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 30.0f;
    [SerializeField] private float _lifeTime = 10.0f;

    private Vector3 _moveDirection;

    void Awake()
    {
        gameObject.tag = "Bullet"; 
        Invoke("Kill", _lifeTime); // Destroy the bullet after its lifetime

        _moveDirection = transform.up; 
    }

    void FixedUpdate()
    {
        //Move the bullet
        transform.position += _moveDirection * Time.deltaTime * _speed; 
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
        else if (other.CompareTag("Wall") || other.CompareTag("Door") || other.CompareTag("Ground") || other.CompareTag("ButtonDoor"))
        {
            Kill(); 
        }

        // Prevent the projectile from hitting the companion
        else if (other.CompareTag("Player"))
        {
            if (other.gameObject.name == "Companion") 
            {
                return;  // Do nothing if it's the companion
            }
           
        }
    }

    void Kill()
    {
        Destroy(gameObject); // Destroy the bullet object
    }
}

