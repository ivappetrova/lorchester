using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 20.0f;  
    [SerializeField] private float _lifeTime = 5.0f;

    private Vector3 _moveDirection;
    private CharacterControlSwitcher controlSwitcher;
    public Health sharedHealth;

    void Awake()
    {
        sharedHealth = FindObjectOfType<Health>();
        gameObject.tag = "EnemyBullet"; 
        Invoke("Kill", _lifeTime);

        controlSwitcher = FindObjectOfType<CharacterControlSwitcher>();
        _moveDirection = transform.up; 
    }

    void Update()
    {
        transform.position += _moveDirection * Time.deltaTime * _speed; 
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
            
           
            // Handle companion collision with thorns
            if (sharedHealth != null)
            {
                sharedHealth.TakeDamage(1);
                Debug.Log("Companion took dmg");
                if (controlSwitcher != null)
                {
                    controlSwitcher.AutoSwitchToPlayer();
                }

            }
            Kill();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Door") || other.CompareTag("Ground") || other.CompareTag("ButtonDoor"))
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject); 
    }
}
