using UnityEngine;
using UnityEngine.SceneManagement;

public class RejouerNiveau : MonoBehaviour
{
    public void RejouerNiveauScene(string nomScene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomScene);
    }
}