using UnityEngine;
using Weapon;

namespace Player
{
    public class AttackBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject gunTemplate;  
        [SerializeField] private GameObject socket; 

        private BasicWeapon _weapon;
        private Transform _weaponTransform;  

        // Expose the weapon's transform for other scripts to access
        public Transform WeaponTransform => _weaponTransform;

        void Awake()
        {
            // Spawn the weapon
            if (gunTemplate != null && socket != null)
            {
                var gunObject = Instantiate(gunTemplate, socket.transform, true);
                gunObject.transform.localPosition = Vector3.zero;
                gunObject.transform.localRotation = Quaternion.identity;

                _weapon = gunObject.GetComponent<BasicWeapon>();
                _weaponTransform = gunObject.transform;  
            }
        }

        public void Attack()
        {
            if (_weapon != null)
            {
                _weapon.Fire();
            }
        }
    }
}