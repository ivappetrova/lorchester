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

        [SerializeField] private UnityEvent onDamageEvent;
        [SerializeField] private UnityEvent onDeathEvent;

        // Shader color property ID
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _currentLives = startLives;
            _player = GetComponent<PlayerCharacter>();

            // Store the original body color
            if (bodyRenderer != null && bodyRenderer.material.HasProperty(ColorProperty))
            {
                _originalBodyColor = bodyRenderer.material.color;
            }
            else
            {
                Debug.LogError("Body Renderer or Material does not have a '_Color' property!");
            }

            // Store the original shoulder color
            if (shoulderRenderer != null && shoulderRenderer.material.HasProperty(ColorProperty))
            {
                _originalShoulderColor = shoulderRenderer.material.color;
            }
            else
            {
                Debug.LogError("Shoulder Renderer or Material does not have a '_Color' property!");
            }

            // Store the original companion color
            if (companionRenderer != null && companionRenderer.material.HasProperty(ColorProperty))
            {
                _originalCompanionColor = companionRenderer.material.color;
            }
            else
            {
                Debug.LogError("Companion Renderer or Material does not have a '_Color' property!");
            }
        }

        public void TakeDamage(int damageAmount)
        {
            if (_isGameOver)
            {
                return;
            }

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

            // Change all renderers to damage color
            if (bodyRenderer != null)
            {
                bodyRenderer.material.SetColor(ColorProperty, damageColor);
            }

            if (shoulderRenderer != null)
            {
                shoulderRenderer.material.SetColor(ColorProperty, damageColor);
            }

            if (companionRenderer != null)
            {
                companionRenderer.material.SetColor(ColorProperty, damageColor);
            }

            yield return new WaitForSeconds(colorChangeDuration);

            // Restore original colors
            if (!_isGameOver)
            {
                if (bodyRenderer != null)
                {
                    bodyRenderer.material.SetColor(ColorProperty, _originalBodyColor);
                }

                if (shoulderRenderer != null)
                {
                    shoulderRenderer.material.SetColor(ColorProperty, _originalShoulderColor);
                }

                if (companionRenderer != null)
                {
                    companionRenderer.material.SetColor(ColorProperty, _originalCompanionColor);
                }
            }
        }

        // Set color permanently when the player dies
        private void SetColorsToRed()
        {
            if (bodyRenderer != null)
            {
                bodyRenderer.material.SetColor(ColorProperty, damageColor);
            }

            if (shoulderRenderer != null)
            {
                shoulderRenderer.material.SetColor(ColorProperty, damageColor);
            }

            if (companionRenderer != null)
            {
                companionRenderer.material.SetColor(ColorProperty, damageColor);
            }
        }
    }
}