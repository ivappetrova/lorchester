using UnityEngine.SceneManagement;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int startLives = 3;
        private int _currentLives;

        public int StartHealth => startLives;
        public int CurrentHealth => _currentLives;
        public int CurrentLives => _currentLives;
        public bool IsGameOver => _isGameOver;

        public delegate void HealthChange(float startHealth, float currentHealth);
        public event HealthChange OnHealthChanged;

        private PlayerCharacter _player;

        [SerializeField] private Renderer bodyRenderer;
        [SerializeField] private Renderer shoulderRenderer;
        [SerializeField] private Renderer companionRenderer;

        [SerializeField] private Color damageColor = Color.red;

        private Color _originalBodyColor;
        private Color _originalShoulderColor;
        private Color _originalCompanionColor;

        private bool _isGameOver;

        [SerializeField] private float colorChangeDuration = 1f;

        [Tooltip("Minimum time (seconds) between damage instances - prevents a single overlap (e.g. two colliders/triggers firing the same frame, or staying inside a trigger across frames) from registering as multiple hits.")]
        [SerializeField] private float damageCooldown = 1f;
        private float _lastDamageTime = -Mathf.Infinity;

        [SerializeField] private UnityEvent onDamageEvent;
        [SerializeField] private UnityEvent onDeathEvent;

        private void Awake()
        {
            _currentLives = startLives;
            _player = GetComponent<PlayerCharacter>();

            if (bodyRenderer != null)
            {
                _originalBodyColor = bodyRenderer.material.color;
            }
            else
            {
                Debug.LogError("Body Renderer is not assigned!");
            }

            if (shoulderRenderer != null)
            {
                _originalShoulderColor = shoulderRenderer.material.color;
            }
            else
            {
                Debug.LogError("Shoulder Renderer is not assigned!");
            }

            if (companionRenderer != null)
            {
                _originalCompanionColor = companionRenderer.material.color;
            }
            else
            {
                Debug.LogError("Companion Renderer is not assigned!");
            }
        }

        public void TakeDamage(int damageAmount)
        {
            if (_isGameOver)
            {
                return;
            }

            // Ignore repeated damage calls that land within damageCooldown of the last hit -
            // e.g. player+companion colliders both overlapping the same thorn in one frame,
            // or the trigger firing again before the object has moved out of it.
            if (Time.time - _lastDamageTime < damageCooldown)
            {
                return;
            }
            _lastDamageTime = Time.time;

            _currentLives -= damageAmount;

            OnHealthChanged?.Invoke(startLives, _currentLives);

            if (_currentLives >= 1)
            {
                onDamageEvent?.Invoke();
                StartCoroutine(ChangeColorTemporarily());
            }
            else
            {
                // Player is dead
                _isGameOver = true;

                if (_player != null)
                {
                    _player.enabled = false;
                }

                onDeathEvent?.Invoke();
                SetColorsToRed();

                StartCoroutine(ReloadScene());
            }
        }

        private IEnumerator ReloadScene()
        {
            yield return new WaitForSeconds(2f);

            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        private IEnumerator ChangeColorTemporarily()
        {
            if (_isGameOver)
            {
                yield break;
            }

            SetBodyColor(damageColor);

            yield return new WaitForSeconds(colorChangeDuration);

            if (!_isGameOver)
            {
                if (bodyRenderer != null)
                {
                    bodyRenderer.material.color = _originalBodyColor;
                }

                if (shoulderRenderer != null)
                {
                    shoulderRenderer.material.color = _originalShoulderColor;
                }

                if (companionRenderer != null)
                {
                    companionRenderer.material.color = _originalCompanionColor;
                }
            }
        }

        private void SetColorsToRed()
        {
            SetBodyColor(damageColor);
        }

        private void SetBodyColor(Color color)
        {
            if (bodyRenderer != null)
            {
                bodyRenderer.material.color = color;
            }

            if (shoulderRenderer != null)
            {
                shoulderRenderer.material.color = color;
            }

            if (companionRenderer != null)
            {
                companionRenderer.material.color = color;
            }
        }
    }
}