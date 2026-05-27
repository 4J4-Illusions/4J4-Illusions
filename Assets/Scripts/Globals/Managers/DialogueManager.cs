using Globals;
using Newtonsoft.Json;
using QuickType;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

// renommage de la classe de manière à rester constant à chaque nouvelle création de la classe représantant la structure json des dialogues
public class DialogueItem : Niveau2 { }

public class DialogueManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static DialogueManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    [TextArea] public DialogueItem[] dialogueItems;
    [Header("Hiérarchie")]
    public GameObject overlayDialogue;
    public TextMeshProUGUI zoneTexteDialogue, zoneTitreDialogue;
    [Header("Projet")]
    public AudioClip sonUI;

    [Header("Accès pour autres scripts"), Space(30)]
    public string fullPath = "";
    public Dialogues tousLesDialogues;

    public static bool inDialogue = false;
    public static Action<bool> OnDialogueInteraction;

    int indexDialogue = 0;

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

        fullPath = Path.Combine(Application.streamingAssetsPath, "Data", "Dialogues.json");
        tousLesDialogues = Dialogues.FromJson(File.ReadAllText(fullPath));
    }



    /// <summary>
    /// Bascule l'état de l'overlay de dialogue et met à jour la variable inDialogue en conséquence.
    /// </summary>
    /// <param name="etatActif">L'état de bascule de l'overlay</param>
    public void ToggleOverlayDialogue(bool etatActif)
    {
        //Debug.Log("etatActif: " + etatActif);
        overlayDialogue.SetActive(etatActif);
        inDialogue = etatActif;
        OnDialogueInteraction?.Invoke(etatActif);

        if (etatActif) ProgresserDialogue(indexDialogue);
    }
    /// <summary>
    /// Affiche le texte du dialogue actuel dans les zones de texte correspondantes.
    /// </summary>
    /// <param name="dialogue">Le dialogue à afficher</param>
    void AfficherTexteDialogue(DialogueItem dialogue)
    {
        zoneTexteDialogue.text = dialogue.Fr;
        zoneTitreDialogue.text = dialogue.Personnage.ToString();
    }
    /// <summary>
    /// Conclu l'affichage du dialogue en réinitialisant l'index du dialogue et en fermant l'overlay de dialogue.
    /// </summary>
    void FinDialogue()
    {
        indexDialogue = 0;
        ToggleOverlayDialogue(false);
    }
    /// <summary>
    /// Fait progresser le dialogue en passant au texte suivant. Si le dialogue atteint la fin, il se réinitialise et ferme l'overlay.
    /// </summary>
    /// <param name="targetDialogue">Option optionnelle pour afficher un dialogue spécifique</param>
    public void ProgresserDialogue(int targetDialogue = -1)
    {
        //Debug.Log("targetDialogue: " + targetDialogue);
        indexDialogue = (targetDialogue == -1) ? indexDialogue + 1 : targetDialogue;
        //Debug.Log("indexDialogue: " + indexDialogue);
        //Debug.Log("dialogueItems.Length: " + dialogueItems.Length);

        AudioManager.Instance.JouerSon(CategorieSon.SFX, sonUI);
        if (indexDialogue >= dialogueItems.Length)
        {
            FinDialogue();
            return;
        }
        else AfficherTexteDialogue(dialogueItems[indexDialogue]);

        // gestion comportement selon options extra
        //Debug.Log("dialogue id: " + dialogueItems[indexDialogue].Id);
        GestionOptionsExtra(dialogueItems[indexDialogue].ExtraOptions);
    }
    /// <summary>
    /// Gère le dialogue introfuctif de chaque scène en fonction de l'étape du jeu.
    /// </summary>
    /// <param name="stageJeu">L'étape actuelle du jeu</param>
    public void SceneLoadedDialogue(StageJeu stageJeu)
    {
        //StageJeu stageJeu = (StageJeu)scene.buildIndex;
        //Debug.Log("stageJeu: " + stageJeu);

        if (new StageJeu[] { StageJeu.Intro, StageJeu.Desert, StageJeu.Foret, StageJeu.Theatre }.Contains(stageJeu))
        {
            // setup de l'overlay de dialogue
            overlayDialogue = GameObject.FindWithTag("DialogueOverlay");
            ToggleOverlayDialogue(false);
            zoneTexteDialogue = overlayDialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            zoneTitreDialogue = overlayDialogue.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            // récupère les dialogues correspondants à l'étape du jeu actuelle et les convertit en DialogueItem[]
            object[] dialogues = null;
            switch (stageJeu)
            {
                case StageJeu.Intro:
                    //Debug.Log("stageJeu = intro");
                    dialogues = tousLesDialogues.Prelude.Dialogues;
                    break;
                case StageJeu.Desert:
                    //Debug.Log("stageJeu = desert");
                    dialogues = tousLesDialogues.Niveau1.Dialogues;
                    break;
                case StageJeu.Foret:
                    //Debug.Log("stageJeu = foret");
                    dialogues = tousLesDialogues.Niveau2;
                    break;
                case StageJeu.Theatre:
                    //Debug.Log("stageJeu = theatre");
                    dialogues = tousLesDialogues.Niveau3;
                    break;
            }
            dialogueItems = JsonConvert.DeserializeObject<DialogueItem[]>(JsonConvert.SerializeObject(dialogues));

            ToggleOverlayDialogue(true);
        }
    }
    /// <summary>
    /// Cette méthode gère les options supplémentaires associées à un dialogue, permettant d'ajuster le comportement du jeu en fonction des paramètres définis dans les dialogues.
    /// </summary>
    /// <param name="options">Les possibles options additionnelles</param>
    void GestionOptionsExtra(ExtraOptions options = null)
    {
        if (options == null) return;

        foreach (var item in options.GetType().GetProperties().Select(prop => prop.Name))
        {
            //Debug.Log("nom propriete: "+ item);
            object propValue = options.GetType().GetProperty(item).GetValue(options);
            //Debug.Log("valeur propriete:" + propValue);
            switch (item)
            {
                case "ProchaineCible":
                    if (propValue == null) return;
                    else if ((string)propValue == "") FinDialogue();
                    else
                    {
                        //Debug.Log(((string)propValue));
                        //Debug.Log(((string)propValue)[^3..]);

                        //ProgresserDialogue(int.Parse(((string)propValue)[^3..]) - 1);
                        indexDialogue = int.Parse(((string)propValue)[^3..]) - 2;
                    }
                    break;
                case "Event":
                    switch (propValue)
                    {
                        case "OnStartDesertLevel":
                            GameObject.Find("SpawnerFoulePassante").SetActive(true);
                            break;
                    }
                    break;
            }
        }
    }
}
