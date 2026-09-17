using System.Collections.Generic;
using UnityEngine;

namespace GameElements.Obstacles
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private List<ShootingObstacle> shooters = new List<ShootingObstacle>();

        public void ActivateZone()
        {
            Debug.Log($"[{name}] ActivateZone - {shooters.Count} shooters in list");
            foreach (var shooter in shooters)
            {
                if (shooter != null)
                    shooter.SetActive(true);
                else
                    Debug.LogWarning($"[{name}] Null entry in shooters list");
            }
        }

        public void DeactivateZone()
        {
            Debug.Log($"[{name}] DeactivateZone - {shooters.Count} shooters in list");
            foreach (var shooter in shooters)
            {
                if (shooter != null)
                    shooter.SetActive(false);
            }
        }
    }
}