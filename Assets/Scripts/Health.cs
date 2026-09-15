using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _startLives = 3;
    private int _currentLives;

    public float StartHealth { get { return _startLives; } }
    public float CurrentHealth { get { return _startLives; } }

    public delegate void HealthChange(float startHealth, float currentHealth);
    public event HealthChange OnHealthChanged;

    private PlayerCharacter player;

    [SerializeField] private Renderer bodyRenderer; 
    [SerializeField] private Renderer shoulderRenderer; 
    [SerializeField] private Renderer companionRenderer; 
    [SerializeField] private Color damageColor = Color.red; 
    private Color originalBodyColor;
    private Color originalShoulderColor; 
    private Color originalCompanionColor;

  
    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    public float colorChangeDuration = 1f;


    [SerializeField]
    private UnityEvent _onDamageEvent;
    [SerializeField]
    private UnityEvent _onDeathEvent;


    private void Awake()
    {
        _currentLives = _startLives;
        player = GetComponent<PlayerCharacter>();

        // Ensure that the renderers have color properties
        if (bodyRenderer != null && bodyRenderer.material.HasProperty("_Color"))
        {
            originalBodyColor = bodyRenderer.material.color;
        }
        else
        {
            Debug.LogError("Body Renderer or Material does not have a '_Color' property!");
        }

        if (shoulderRenderer != null && shoulderRenderer.material.HasProperty("_Color"))
        {
            originalShoulderColor = shoulderRenderer.material.color;
        }
        else
        {
            Debug.LogError("Shoulder Renderer or Material does not have a '_Color' property!");
        }

        if (companionRenderer != null && companionRenderer.material.HasProperty("_Color"))
        {
            originalCompanionColor = companionRenderer.material.color;
        }
        else
        {
            Debug.LogError("Body Renderer or Material does not have a '_Color' property!");
        }
    }

    public int CurrentLives => _currentLives;

    public void TakeDamage(int damageAmount)
    {
        if (isGameOver) return; 

        _currentLives -= damageAmount;

        OnHealthChanged?.Invoke(_startLives, _currentLives); ;

        if (_currentLives >= 1)
        {
            _onDamageEvent?.Invoke();
            StartCoroutine(ChangeColorTemporarily());
        }
        else
        {
           // When the player is dead
            isGameOver = true;
            player.enabled = false;
            _onDeathEvent?.Invoke();
            SetColorsToRed();
            StartCoroutine(ReloadScene());
          
        }
    }
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f);
        var currentScene= SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    private IEnumerator ChangeColorTemporarily()
    {
        if (!isGameOver) // Only change color if the game isn't over
        {
            if (bodyRenderer != null)
            {
                bodyRenderer.material.color = damageColor; 
            }

            if (shoulderRenderer != null)
            {
                shoulderRenderer.material.color = damageColor; 
            }

            if (companionRenderer != null)
            {
                companionRenderer.material.color = damageColor; 
            }

            // Wait some time
            yield return new WaitForSeconds(colorChangeDuration);

            // Revert the colors if the game isn't over
            if (bodyRenderer != null && !isGameOver)
            {
                bodyRenderer.material.color = originalBodyColor; 
            }

            if (shoulderRenderer != null && !isGameOver)
            {
                shoulderRenderer.material.color = originalShoulderColor; 
            }

            if (companionRenderer != null && !isGameOver)
            {
                companionRenderer.material.color = originalCompanionColor;
            }
        }
    }

    // Set color permanently
    private void SetColorsToRed()
    {
        if (bodyRenderer != null)
        {
            bodyRenderer.material.color = damageColor; 
        }

        if (shoulderRenderer != null)
        {
            shoulderRenderer.material.color = damageColor; 

        }

        if (companionRenderer != null)
        {
            companionRenderer.material.color = damageColor; 
        }
    }
}
