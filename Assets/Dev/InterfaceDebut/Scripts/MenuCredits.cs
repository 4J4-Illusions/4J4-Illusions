using UnityEngine;
using System.Collections;

public class MenuCredits : MonoBehaviour
{
    [Header("Objets à cacher")]
    public GameObject[] objectsToHide;

    [Header("Objets à afficher immédiatement")]
    public GameObject[] objectsToShow;

    [Header("Texte à afficher après délai")]
    public GameObject delayedText;

    public float delay = 4f;

    public void SwitchUI()
    {
        // 1. Cacher les éléments
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // 2. Afficher les éléments immédiats
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // 3. S'assurer que le texte est caché au départ
        if (delayedText != null)
            delayedText.SetActive(false);

        // 4. Lancer le délai
        StartCoroutine(ShowTextAfterDelay());
    }

    IEnumerator ShowTextAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        
        if (delayedText != null)
            delayedText.SetActive(true);
    }
}