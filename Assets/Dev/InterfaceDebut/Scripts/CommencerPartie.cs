using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour changer de scène

public class CommencerPartie : MonoBehaviour
{
    // Appelle cette fonction depuis un bouton UI
    public void BeginGame(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}