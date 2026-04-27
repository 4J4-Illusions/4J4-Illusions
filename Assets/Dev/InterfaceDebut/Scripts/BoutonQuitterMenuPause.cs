using UnityEngine;

public class BoutonQuitterMenuPause : MonoBehaviour
{
    public void Quitter()
    {
        Debug.Log("Fermeture du jeu...");

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
