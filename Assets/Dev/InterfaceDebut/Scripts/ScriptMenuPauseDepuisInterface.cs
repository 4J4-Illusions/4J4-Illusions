using UnityEngine;

public class ScriptMenuPauseDepuisInterface : MonoBehaviour
{
    [Header("Menu Paramètres UI")]
    public GameObject settingsUI; // Drag ton panel Paramètres ici

    void Start()
    {
        // Assure-toi que le menu est caché au départ
        if (settingsUI != null)
            settingsUI.SetActive(false);
    }

    // Appelle cette fonction depuis ton bouton UI
    public void OpenSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(true);
    }

    // Fonction pour fermer le menu (tu peux l'appeler depuis un bouton "Retour")
    public void CloseSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(false);
    }
}