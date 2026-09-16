using Movables;
using UnityEngine;

namespace GameElements
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] private GameObject keyPrefab;
        [SerializeField] private Transform keySpawnLocation;
        [SerializeField] private GameObject breakEffectPrefab;
        [SerializeField] private Door targetDoor;

        public void Break()
        {

            if (breakEffectPrefab != null)
            {
                Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);

            }

            if (keyPrefab != null && keySpawnLocation != null)
            {
                GameObject spawnedKey = Instantiate(keyPrefab, keySpawnLocation.position, Quaternion.identity);

                // Set the target door for the spawned key
                Key keyScript = spawnedKey.GetComponent<Key>();
                if (keyScript != null && targetDoor != null)
                {
                    keyScript.targetDoor = targetDoor;
                }
                else
                {
                    Debug.Log("Key or Target Door is missing!");
                }
            }

            // Destroy the statue
            Destroy(gameObject);
        }
    }
}
