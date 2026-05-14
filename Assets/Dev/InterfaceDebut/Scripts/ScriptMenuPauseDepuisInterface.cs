using UnityEngine;
using System.Collections;
using System;

public class ScriptMenuPauseDepuisInterface : MonoBehaviour
{
    [Header("Menu Paramètres UI")]
    public GameObject settingsUI;       // Glisse ton panel Paramètres ici
    public float delayBeforeOpen = 2f;  // délai après le clic

    public static bool inMenu = false;
    // evenements
    public static Action<bool> OnMenuPause;

    void Start()
    {
        // Assure-toi que le menu est caché au départ
        if (settingsUI != null)
            settingsUI.SetActive(false);
    }

    // Cette fonction est appelée par ton bouton Paramètres
    public void OpenSettingsWithDelay()
    {
        // Lance la coroutine
        StartCoroutine(OpenMenuAfterDelay());
    }

    IEnumerator OpenMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeOpen); // attend 2 secondes
        OpenSettings();
    }

    // Ouvre le menu et met le jeu en pause
    private void OpenSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(true);

        //Time.timeScale = 0f; // pause le jeu
        OnMenuPause.Invoke(true);
        inMenu = true;
    }

    // Ferme le menu et reprend le jeu
    public void CloseSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(false);

        //Time.timeScale = 1f; // reprend le jeu
        OnMenuPause.Invoke(false);
        inMenu = false;
    }
}