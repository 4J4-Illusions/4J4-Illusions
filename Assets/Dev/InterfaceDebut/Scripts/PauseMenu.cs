using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Pause UI")]
    public GameObject pauseUI; // Drag ton panel pause ici

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                // Pause le jeu et affiche le menu
                PauseGame();
            }
            else
            {
                // Dépause le jeu et cache le menu
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        pauseUI.SetActive(true);       // Affiche le menu pause
        Time.timeScale = 0f;           // Stoppe le jeu
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseUI.SetActive(false);      // Cache le menu pause
        Time.timeScale = 1f;           // Reprend le jeu
        isPaused = false;
    }
}