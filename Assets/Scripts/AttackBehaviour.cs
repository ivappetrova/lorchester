using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _gunTemplate = null;  
    [SerializeField]
    private GameObject _socket = null; 

    private BasicWeapon _weapon = null;
    private Transform _weaponTransform;  

    // Expose the weapon's transform for other scripts to access
    public Transform WeaponTransform
    {
        get { return _weaponTransform; }
    }

    void Awake()
    {
        // Spawn the weapon
        if (_gunTemplate != null && _socket != null)
        {
            var gunObject = Instantiate(_gunTemplate, _socket.transform, true);
            gunObject.transform.localPosition = Vector3.zero;
            gunObject.transform.localRotation = Quaternion.identity;

            _weapon = gunObject.GetComponent<BasicWeapon>();
            _weaponTransform = gunObject.transform;  
        }
    }

    public void Attack()
    {
        if (_weapon != null)
            _weapon.Fire();
    }
}
