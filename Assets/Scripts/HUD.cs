using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UIElements.Button;


public class HUD : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private ProgressBar _healthbar = null;
    private Label _gameover = null;
    private Label _winnerchickendinner = null;
    private Button _replay = null;

    private Health playerHealth = null;  // Reference to the player's Health component
    

    void Awake()
    {
        // UI setup
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument)
        {
            _root = _attachedDocument.rootVisualElement;
        }
        if (_root != null)
        {
            _healthbar = _root.Q<ProgressBar>();
            _gameover = _root.Q<Label>("GameOver");
           _winnerchickendinner = _root.Q<Label>("Winner");
           _replay = _root.Q<Button>();
            _replay.clickable.clicked += Restart;


          _winnerchickendinner.visible=false;
            _gameover.visible = false;
            _replay.visible = false;
            PlayerCharacter[] player = FindObjectsOfType<PlayerCharacter>();
            if (player != null)
            {
                for (var i = 0; i < player.Length; i++)
                {
                    if (playerHealth = player[i].GetComponent<Health>())
                    {
                        //Debug.Log(playerHealth.name);
                        // Initialize health UI
                        UpdateHealth(playerHealth.StartHealth, playerHealth.CurrentHealth);
                        // Hook to monitor health changes
                        playerHealth.OnHealthChanged += UpdateHealth;

                        return;
                    }

                }

            }
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        // Check if the game is over in the Health class using the public getter
        if (playerHealth != null && playerHealth.IsGameOver)
        {
            ShowGameOver(); 
        }

            ShowWinner();
        
    }

    public void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthbar == null) return;
        _healthbar.value = currentHealth / startHealth *100;
        _healthbar.title = string.Format("{0}/{1}", currentHealth, startHealth);
        //Debug.Log("Current health: " + currentHealth + "Start health: " + startHealth);
    }

    private void ShowGameOver()
    {
        if (_gameover != null)
        {

            _gameover.visible = true;
            _gameover.style.display = DisplayStyle.Flex; 
        }
    }

    private void ShowWinner()
    {
        if (_winnerchickendinner != null && SceneManager.GetActiveScene().buildIndex == 2)
        {
            _healthbar.visible = false;
            _winnerchickendinner.visible = true;
            _winnerchickendinner.style.display = DisplayStyle.Flex;
            _replay.visible = true;
        }
    }


}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class HUD : MonoBehaviour
//{
//    private UIDocument _attachedDocument = null;
//    private VisualElement _root = null;
//    private ProgressBar _healthbar = null;
//    private Label _gameover = null;

//    void Start()
//    {
//        //UI
//        _attachedDocument = GetComponent<UIDocument>();
//        if (_attachedDocument)
//        {
//            _root = _attachedDocument.rootVisualElement;
//        }
//        if (_root != null)
//        {
//            _healthbar = _root.Q<ProgressBar>();
//            _gameover = _root.Q<Label>();
//            PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
//            if (player != null)
//            {
//                Health playerHealth = player.GetComponent<Health>();
//                if (playerHealth)
//                {
//                    // initialize
//                    UpdateHealth(playerHealth.StartHealth, playerHealth.CurrentHealth);
//                    // hook to monitor changes
//                    playerHealth.OnHealthChanged += UpdateHealth;
//                }
//            }



//        }
//    }
//    public void UpdateHealth(float startHealth, float currentHealth)
//    {
//        if (_healthbar == null) return;
//        _healthbar.value = currentHealth / startHealth;
//        _healthbar.title = string.Format("{0}/{1}", currentHealth, startHealth);
//    }


//    public void ShowGameOver()
//    {
//        if (_gameover != null)
//        {
//            _gameover.text = "Game Over!"; // Set the text of the label
//            _gameover.style.display = DisplayStyle.Flex; // Show the label
//        }
//    }
//}