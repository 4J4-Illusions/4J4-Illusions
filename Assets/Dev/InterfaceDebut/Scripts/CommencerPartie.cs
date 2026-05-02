using UnityEngine;
using UnityEngine.SceneManagement; // Nécessaire pour changer de scène

public class CommencerPartie : MonoBehaviour
{
    [Header("Nom de la scène à charger")]
    public string sceneToLoad = "Game"; // Mets ici le nom exact de ta scène

    // Appelle cette fonction depuis un bouton UI
    public void BeginGame(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}