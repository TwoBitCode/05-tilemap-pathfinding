using UnityEngine;

public class DijkstraMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject startGamePanel; // Reference to the start game panel
    public static bool gameStarted = false; // Tracks if the game has started

    // Constants for Time.timeScale values
    private const float GAME_PAUSED = 0f;
    private const float GAME_RUNNING = 1f;

    private void Start()
    {
        // Show the start panel at launch and prevent gameplay
        if (startGamePanel != null)
        {
            startGamePanel.SetActive(true);
        }
        gameStarted = false; // Ensure gameplay interactions are initially disabled
        Time.timeScale = GAME_PAUSED; // Pause the game
    }

    public void StartGame()
    {
        if (startGamePanel == null)
        {
            Debug.LogWarning("Start game panel is not assigned!");
            return;
        }

        // Hide the start panel and enable gameplay
        startGamePanel.SetActive(false);
        gameStarted = true; // Mark the game as started
        Time.timeScale = GAME_RUNNING; // Resume the game
    }
}
