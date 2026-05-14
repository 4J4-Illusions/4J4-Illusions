using System;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static DialogueManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    [TextArea] public string[] textesDialogue;
    public string titreDialogue = "Jean-Michel";

    [Header("Hiérarchie")]
    public GameObject overlayDialogue;
    public TextMeshProUGUI zoneTexteDialogue;

    public static bool inDialogue = false;
    public static Action<bool> OnDialogueInteraction;

    int indexTexteDialogue = 0;

    void Awake()
    {        /*
         * setup du singleton
         * trouvé sur ce lien:
         * https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        */
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ToggleOverlayDialogue(false);
        zoneTexteDialogue = overlayDialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        overlayDialogue.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = titreDialogue;
    }



    /// <summary>
    /// Bascule l'état de l'overlay de dialogue et met à jour la variable inDialogue en conséquence.
    /// </summary>
    /// <param name="etatActif">L'état de bascule de l'overlay</param>
    public void ToggleOverlayDialogue(bool etatActif)
    {
        overlayDialogue.SetActive(etatActif);
        inDialogue = etatActif;
        OnDialogueInteraction?.Invoke(etatActif);

        if(etatActif) zoneTexteDialogue.text = textesDialogue[indexTexteDialogue];
    }
    /// <summary>
    /// Fait progresser le dialogue en passant au texte suivant. Si le dialogue atteint la fin, il se réinitialise et ferme l'overlay.
    /// </summary>
    public void ProgresserDialogue()
    {
        indexTexteDialogue++;
        if (indexTexteDialogue >= textesDialogue.Length)
        {
            indexTexteDialogue = 0;
            ToggleOverlayDialogue(false);
        }

        zoneTexteDialogue.text = textesDialogue[indexTexteDialogue];
    }
}
