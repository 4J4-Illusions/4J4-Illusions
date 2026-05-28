using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuRetourEscape : MonoBehaviour
{
    [Header("Menu principal")]
    public GameObject[] objetsBase;

    [Header("Menus secondaires (Credits, Settings, Quit etc.)")]
    public GameObject[] objetsSecondaires;
    public ScriptMenuPauseDepuisInterface menuController;

    [Header("Rideaux")]
    public ControllerRideaux rideaux;

    [Header("Duree animation")]
    public float animationDelay = 2f;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(RetourMenuPropre());
        }
    }

    IEnumerator RetourMenuPropre()
    {
        Debug.Log("ESC -> RESET UI COMPLET");

        // 0. fermer menu paramètres
        menuController.CloseSettings();

        // 1. fermer rideaux
        if (rideaux != null)
            rideaux.FermerRideaux();

        yield return new WaitForSeconds(animationDelay);

        // 2. RESET GLOBAL UI (IMPORTANT FIX)
        ResetUI();

        // 3. activer menu principal
        foreach (GameObject obj in objetsBase)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // 4. rouvrir rideaux
        if (rideaux != null)
            rideaux.OuvrirRideaux();
    }

    void ResetUI()
    {
        Debug.Log("RESET UI GLOBAL");

        // désactive TOUT ce qui peut bloquer Quitter / Credits / Settings
        foreach (GameObject obj in objetsSecondaires)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}