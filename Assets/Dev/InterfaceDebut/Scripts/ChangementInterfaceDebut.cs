using UnityEngine;
using System.Collections;

public class MenuCredits : MonoBehaviour
{
    [Header("UI")]
    public GameObject[] objectsToHide;
    public GameObject[] objectsToShow;
    public GameObject delayedText;

    [Header("Rideaux")]
    public ControllerRideaux rideaux;

    [Header("Temps ouverture")]
    public float animationDelay = 2f;

    public void SwitchUI()
    {
        StartCoroutine(DelayedSwitch());
    }

    IEnumerator DelayedSwitch()
    {
        // ⏳ attente avant de lancer la transition
        yield return new WaitForSeconds(1f);

        // lancement de la transition normale
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        Debug.Log("DEBUT TRANSITION");

        // 1. petite pause pour laisser le temps aux animations en cours
        yield return new WaitForSeconds(2f);

        // 2. changer UI pendant que les rideaux sont fermes
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        if (delayedText != null)
            delayedText.SetActive(false);

        // 3. ouvrir les rideaux
        if (rideaux == null)
        {
            Debug.LogError("RIDEAUX PAS ASSIGNE DANS L'INSPECTOR");
        }
        else
        {
            Debug.Log("RIDEAUX OK -> OUVERTURE");
            rideaux.OuvrirRideaux();
        }

        // 4. attendre animation ouverture
        yield return new WaitForSeconds(animationDelay);

        // 5. afficher texte
        if (delayedText != null)
            delayedText.SetActive(true);
    }
}