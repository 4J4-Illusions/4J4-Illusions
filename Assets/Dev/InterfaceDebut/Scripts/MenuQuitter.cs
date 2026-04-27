using UnityEngine;
using System.Collections;

public class MenuQuitter : MonoBehaviour
{
    [Header("UI MENU BASE")]
    public GameObject[] objetsBase;

    [Header("UI POPUP QUITTER")]
    public GameObject[] popupQuitter;

    [Header("RIDEAUX")]
    public ControllerRideaux rideaux;

    [Header("DUREE ANIMATION (FERMETURE / OUVERTURE)")]
    public float animationDelay = 2f;

    [Header("DUREES SEPAREES (FIX PRECIS)")]
    public float fermetureDelay = 2f;
    public float ouvertureDelay = 2f;

    [Header("DELAI AVANT ACTION")]
    public float delayAvantAction = 1f;

    // =========================
    // OUVRIR MENU QUITTER
    // =========================
    public void OuvrirMenuQuitter()
    {
        StartCoroutine(DelayedOuvrirMenuQuitter());
    }

    IEnumerator DelayedOuvrirMenuQuitter()
    {
        Debug.Log("ATTENTE AVANT OUVERTURE MENU QUITTER");
        yield return new WaitForSeconds(delayAvantAction);

        Debug.Log("FERMETURE RIDEAUX");

        if (rideaux != null)
            rideaux.FermerRideaux();

        yield return new WaitForSeconds(fermetureDelay);

        foreach (GameObject obj in objetsBase)
            if (obj != null)
                obj.SetActive(false);

        foreach (GameObject obj in popupQuitter)
            if (obj != null)
                obj.SetActive(true);

        Debug.Log("OUVERTURE RIDEAUX");

        if (rideaux != null)
            rideaux.OuvrirRideaux();

        yield return new WaitForSeconds(ouvertureDelay);
    }

    // =========================
    // BOUTON : NON
    // =========================
    public void NonQuitter()
    {
        StartCoroutine(SequenceNon());
    }

    IEnumerator SequenceNon()
    {
        Debug.Log("NON -> FERMETURE RIDEAUX");

        // 1. fermer rideaux
        if (rideaux != null)
            rideaux.FermerRideaux();

        // 2. attendre fin fermeture (IMPORTANT FIX)
        yield return new WaitForSeconds(fermetureDelay);

        Debug.Log("RETOUR MENU BASE");

        // 3. swap UI
        foreach (GameObject obj in popupQuitter)
            if (obj != null)
                obj.SetActive(false);

        foreach (GameObject obj in objetsBase)
            if (obj != null)
                obj.SetActive(true);

        // 4. ouvrir rideaux
        if (rideaux != null)
            rideaux.OuvrirRideaux();

        // 5. attendre fin ouverture (corrige ton problème)
        yield return new WaitForSeconds(ouvertureDelay);
    }

    // =========================
    // BOUTON : OUI
    // =========================
    public void OuiQuitter()
    {
        Debug.Log("QUIT GAME");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}