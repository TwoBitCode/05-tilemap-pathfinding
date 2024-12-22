using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject startGamePanel; // Start Game Panel
    [SerializeField] private GameObject gameOverPanel;  // Game Over Panel
    [SerializeField] private string nextSceneName; // Name of the next scene
    [SerializeField] private TMP_Text gameOverText; // TMP_Text component for Game Over message
    [SerializeField] private float delayBeforeNextScene = 3f; // Delay before switching to the next scene

    private void Start()
    {
        if (startGamePanel != null)
        {
            startGamePanel.SetActive(true);
            Time.timeScale = 0; // Pause the game initially
        }
    }

    public void StartGame()
    {
        if (startGamePanel != null)
        {
            startGamePanel.SetActive(false);
        }

        Time.timeScale = 1; // Resume the game
        Debug.Log("Start button clicked. Game is now running.");
    }

    public void ShowGameOver(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverText != null)
            {
                gameOverText.text = message; // Display the specific Game Over message
            }
        }

        Time.timeScale = 0; // Pause the game
        StartCoroutine(WaitAndLoadNextScene()); // Start delay coroutine
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSecondsRealtime(delayBeforeNextScene); // Wait for the specified delay

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // Load the next scene
        }
        else
        {
            Debug.LogError("Next scene name is not specified!");
        }
    }
}
