using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace Utils
{
    public class HUD : MonoBehaviour
    {
        private UIDocument _attachedDocument;
        private VisualElement _root;
        private ProgressBar _healthBar;
        private Label _gameOver;
        private Label _winnerChickenDinner;
        private Button _replay;

        private Health _playerHealth;

        private void Awake()
        {
            // Get the UI Document
            _attachedDocument = GetComponent<UIDocument>();

            if (_attachedDocument == null)
            {
                Debug.LogError("HUD: No UIDocument found on this GameObject.");
                return;
            }

            _root = _attachedDocument.rootVisualElement;

            if (_root == null)
            {
                Debug.LogError("HUD: Root VisualElement is null.");
                return;
            }

            // Find UI elements
            _healthBar = _root.Q<ProgressBar>();
            _gameOver = _root.Q<Label>("GameOver");
            _winnerChickenDinner = _root.Q<Label>("Winner");
            _replay = _root.Q<Button>();

            if (_healthBar == null)
            {
                Debug.LogError("HUD: Health ProgressBar was not found.");
            }

            if (_gameOver == null)
            {
                Debug.LogError("HUD: Label 'GameOver' was not found.");
            }

            if (_winnerChickenDinner == null)
            {
                Debug.LogError("HUD: Label 'Winner' was not found.");
            }

            if (_replay == null)
            {
                Debug.LogError("HUD: Replay Button was not found.");
            }
            else
            {
                _replay.clickable.clicked += Restart;
            }

            // Set initial UI state
            if (_winnerChickenDinner != null)
            {
                _winnerChickenDinner.visible = false;
            }

            if (_gameOver != null)
            {
                _gameOver.visible = false;
            }

            if (_replay != null)
            {
                _replay.visible = false;
            }

            // Find the player
            PlayerCharacter[] players =
                FindObjectsByType<PlayerCharacter>(FindObjectsSortMode.None);

            foreach (PlayerCharacter player in players)
            {
                _playerHealth = player.GetComponent<Health>();

                if (_playerHealth != null)
                {
                    UpdateHealth(
                        _playerHealth.StartHealth,
                        _playerHealth.CurrentHealth
                    );

                    _playerHealth.OnHealthChanged += UpdateHealth;

                    break;
                }
            }

            if (_playerHealth == null)
            {
                Debug.LogError("HUD: Could not find a Health component on a PlayerCharacter.");
            }
        }

        private void Restart()
        {
            SceneManager.LoadScene(1);
        }

        private void Update()
        {
            if (_playerHealth != null && _playerHealth.IsGameOver)
            {
                ShowGameOver();
            }

            ShowWinner();
        }

        private void UpdateHealth(float startHealth, float currentHealth)
        {
            if (_healthBar == null)
            {
                return;
            }

            _healthBar.value = currentHealth / startHealth * 100f;
            _healthBar.title = $"{currentHealth}/{startHealth}";
        }

        private void ShowGameOver()
        {
            if (_gameOver == null)
            {
                return;
            }

            _gameOver.visible = true;
            _gameOver.style.display = DisplayStyle.Flex;
        }

        private void ShowWinner()
        {
            if (_winnerChickenDinner == null)
            {
                return;
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (_healthBar != null)
                {
                    _healthBar.visible = false;
                }

                _winnerChickenDinner.visible = true;
                _winnerChickenDinner.style.display = DisplayStyle.Flex;

                if (_replay != null)
                {
                    _replay.visible = true;
                }
            }
        }

        private void OnDestroy()
        {
            if (_replay != null)
            {
                _replay.clickable.clicked -= Restart;
            }

            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChanged -= UpdateHealth;
            }
        }
    }
}