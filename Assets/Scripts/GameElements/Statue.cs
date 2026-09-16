using UnityEngine;

namespace GameElements
{
    public class Statue : MonoBehaviour
    {
        public GameObject hiddenKey;  
        private bool _isBroken;

        public void BreakStatue()
        {
            if (!_isBroken)
            {
                _isBroken = true;
                Debug.Log("Statue broken!");

                // Reveal the hidden key
                if (hiddenKey != null)
                {
                    hiddenKey.SetActive(true);
                    Debug.Log("Key revealed!");
                }

                // Optional: Add visual effects or sound here
                Destroy(gameObject);  // Destroys the statue object
            }
        }
    }
}
