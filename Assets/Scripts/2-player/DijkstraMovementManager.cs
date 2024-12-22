using UnityEngine;

public class DijkstraMovementManager : MonoBehaviour
{
    [SerializeField] private GameObject startGamePanel; // Start Game Panel
    public static bool gameStarted = false; // Flag to track if the game has started

    private void Start()
    {
        // Show the Start Panel and pause the game
        if (startGamePanel != null)
        {
            startGamePanel.SetActive(true);
        }
        gameStarted = false; // Ensure the game is not started at launch
        Time.timeScale = 0; // Pause the game
    }

    public void StartGame()
    {
        // Hide the Start Panel and resume the game
        if (startGamePanel != null)
        {
            startGamePanel.SetActive(false);
        }
        gameStarted = true; // Allow gameplay interactions
        Time.timeScale = 1; // Resume the game
    }
}
