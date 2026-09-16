using UnityEngine;

namespace Player
{
        public class CharacterControlSwitcher : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter player;     
        [SerializeField] private CompanionFollow companion;  

        private void Update()
        {
        
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToPlayer();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToCompanion();
            }
        }

        private void SwitchToPlayer()
        {
            if (player != null && companion != null)
            {
                player.EnableControl();
                companion.SetPlayer(player.transform);  
                companion.SetFollowMode();              
                Debug.Log("Switched control to the player.");
            }
        }

        private void SwitchToCompanion()
        {
            if (player != null && companion != null)
            {
                companion.SetFlyingMode();
                player.DisableControl();
                Debug.Log("Switched control to the companion.");
            }
        }

        // Method to auto-switch control to the player
        public void AutoSwitchToPlayer()
        {
            Debug.Log("Automatically switching control back to the player.");
            SwitchToPlayer();
        }
}
}

