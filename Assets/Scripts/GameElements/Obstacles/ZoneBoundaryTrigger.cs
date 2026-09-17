using UnityEngine;

namespace GameElements.Obstacles
{
    public enum BoundaryRole { Entrance, Exit }

    [RequireComponent(typeof(Collider))]
    public class ZoneBoundaryTrigger : MonoBehaviour
    {
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private BoundaryRole role;

        private void Awake()
        {
            if (zoneController == null)
                zoneController = GetComponentInParent<ZoneController>();

            Debug.Log($"[{name}] Awake - role={role}, zoneController found: {zoneController != null}");

            var col = GetComponent<Collider>();
            if (col != null && !col.isTrigger)
            {
                Debug.LogWarning($"{name}: Is Trigger is off - forcing it on.", this);
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (zoneController == null) return;

            if (role == BoundaryRole.Entrance)
                zoneController.ActivateZone();
            else // Exit
                zoneController.DeactivateZone();
        }
    }
}