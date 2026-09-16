using Player;
using UnityEngine;
using Utils;

namespace GameElements.Obstacles
{
    public class ThornObstacle : MonoBehaviour
    {
        public Transform spawnPoint; 
        public CharacterControlSwitcher controlSwitcher; 
        public Health sharedHealth;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.name == "Player")
            {
                if (sharedHealth != null)
                {
                    Debug.Log("Player took dmg");
                    sharedHealth.TakeDamage(1);
                }
                collision.transform.position = spawnPoint.position;
               
            }
            else if (collision.name == "Companion")
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

            }
        }
    }
}
