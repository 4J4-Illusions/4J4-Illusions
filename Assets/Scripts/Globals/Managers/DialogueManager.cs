using Globals;
using UnityEngine.UI;
using Newtonsoft.Json;
using QuickType;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// renommage de la classe de manière à rester constant à chaque nouvelle création de la classe représantant la structure json des dialogues
public class DialogueItem : Niveau2 { }

public class DialogueManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static DialogueManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject overlayDialogue;
    public TextMeshProUGUI zoneTexteDialogue, zoneTitreDialogue, zoneTexteRappel;
    public GameObject[] choixDialogues;
    [Header("Projet")]
    public AudioClip sonUI;
    public Image zoneImagePersonnage;
    public Sprite imageVyktor;
    public Sprite imageAlaric;
    public Sprite imageAutres;

    [Header("Accès pour autres scripts"), Space(30)]
    public string fullPath = "";
    public DialogueItem[] dialogueItems;
    public Dialogues tousLesDialogues;

    public static bool inDialogue = false;
    public static Action<bool> OnDialogueInteraction;

    int indexDialogue = 0;
    bool doPreemptiveQuit = false, doitFaireChoix = false;

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
    public void ToggleOverlayDialogue(bool etatActif, int setIndex = -1)
    {
        //Debug.Log("etatActif: " + etatActif);
        overlayDialogue.SetActive(etatActif);
        inDialogue = etatActif;
        OnDialogueInteraction?.Invoke(etatActif);

        if (etatActif) ProgresserDialogue((setIndex == -1) ? indexDialogue : setIndex);
    }

    // Coroutine pour faire apparaître le texte lettre par lettre (typewriter)
    // IEnumerator TypeWriter(string texte)
    IEnumerator TypeWriter(string texte)
    {
        // Réinitialisation du texte
        zoneTexteDialogue.text = "";

        // Pour chaque caractère dans la ligne de dialogue actuelle
        foreach (char c in texte)
        {
            // On ajoute les caractères un par un avec un délai
            zoneTexteDialogue.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    /// <summary>
    /// Affiche le texte du dialogue actuel dans les zones de texte correspondantes.
    /// </summary>
    /// <param name="dialogue">Le dialogue à afficher</param>
    void AfficherTexteDialogue(DialogueItem dialogue)
    {
        zoneTitreDialogue.text = (dialogue.Personnage != Personnage.Empty) ? dialogue.Personnage.ToString() : "?????";

        if (GameManager.Instance.langue == LanguageManager.Language.English)
        { zoneTexteRappel.text = "Press \"E\" to continue"; }
        else zoneTexteRappel.text = "Appuyez sur \"E\" pour continuer";

        // Arrêt de toutes les coroutines
        StopAllCoroutines();
        // Lancement de la coroutine pour faire apparaître le texte lettre par lettre
        StartCoroutine(TypeWriter((GameManager.Instance.langue == LanguageManager.Language.English) ? dialogue.En : dialogue.Fr));

        // Affichage de l'image du personnage en fonction du nom du personnage dans le dialogue
        switch (dialogue.Personnage)
        {
            case Personnage.Vyktor:
                zoneImagePersonnage.sprite = imageVyktor;
                break;

            case Personnage.Alaric:
                zoneImagePersonnage.sprite = imageAlaric;
                break;

            case Personnage.Foule:
                zoneImagePersonnage.sprite = imageAutres;
                break;

            case Personnage.Empty:
                zoneImagePersonnage.sprite = imageAutres;
                break;
        }
    }
    /// <summary>
    /// Conclu l'affichage du dialogue en réinitialisant l'index du dialogue et en fermant l'overlay de dialogue.
    /// </summary>
    void FinDialogue()
    {
        indexDialogue = 0;
        doPreemptiveQuit = false;
        ToggleOverlayDialogue(false);
    }
    /// <summary>
    /// Fait progresser le dialogue en passant au texte suivant. Si le dialogue atteint la fin, il se réinitialise et ferme l'overlay.
    /// </summary>
    /// <param name="targetDialogue">Option optionnelle pour afficher un dialogue spécifique</param>
    public void ProgresserDialogue(int targetDialogue = -1)
    {
        if (doPreemptiveQuit)
        {
            FinDialogue();
            return;
        }

        if (!doitFaireChoix)
        {
            //Debug.Log("targetDialogue: " + targetDialogue);
            //Debug.Log("indexDialogue avant: " + indexDialogue);
            indexDialogue = (targetDialogue == -1) ? indexDialogue + 1 : targetDialogue;
            //Debug.Log("indexDialogue apres: " + indexDialogue);
            //Debug.Log("dialogueItems.Length: " + dialogueItems.Length);

            AudioManager.Instance.JouerSon(CategorieSon.SFX, sonUI);

            if (indexDialogue >= dialogueItems.Length)
            {
                FinDialogue();
                return;
            }
            //else AfficherTexteDialogue(dialogueItems[indexDialogue]);
            AfficherTexteDialogue(dialogueItems[indexDialogue]);

            AudioManager.Instance.JouerSonDialogue(dialogueItems[indexDialogue].Id);

            // gestion comportement selon options extra
            //Debug.Log("dialogue id: " + dialogueItems[indexDialogue].Id);
            GestionOptionsExtra(dialogueItems[indexDialogue]);
        }
    }
    /// <summary>
    /// Gère le dialogue introfuctif de chaque scène en fonction de l'étape du jeu.
    /// </summary>
    /// <param name="stageJeu">L'étape actuelle du jeu</param>
    public void SceneLoadedInit(StageJeu stageJeu)
    {
        //StageJeu stageJeu = (StageJeu)scene.buildIndex;
        //Debug.Log("stageJeu: " + stageJeu);

        if (new StageJeu[] { StageJeu.PreludeSuite, StageJeu.Desert, StageJeu.Foret, StageJeu.Theatre }.Contains(stageJeu))
        {
            // setup de l'overlay de dialogue
            overlayDialogue = GameObject.FindWithTag("DialogueOverlay");
            ToggleOverlayDialogue(false);

            zoneTexteDialogue = overlayDialogue.transform.Find("TexteDialogue").GetComponent<TextMeshProUGUI>();
            zoneTitreDialogue = overlayDialogue.transform.Find("Nom").GetComponent<TextMeshProUGUI>();
            zoneTexteRappel = overlayDialogue.transform.Find("Rappel").GetComponent<TextMeshProUGUI>();
            zoneImagePersonnage = overlayDialogue.transform.Find("ImagePerso").GetComponent<Image>();

            choixDialogues = new GameObject[2] {
                overlayDialogue.transform.GetChild(overlayDialogue.transform.childCount - 2).gameObject,
                overlayDialogue.transform.GetChild(overlayDialogue.transform.childCount - 1).gameObject
            };
            Array.Sort(choixDialogues, (a, b) => a.name.CompareTo(b.name));
            foreach (var item in choixDialogues)
            {
                item.SetActive(false);
            }

            // récupère les dialogues correspondants à l'étape du jeu actuelle et les convertit en DialogueItem[]
            object[] dialogues = null;
            switch (stageJeu)
            {
                case StageJeu.PreludeSuite:
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
    /// <param name="dialogue">Le dialogue dont les options seront extraites</param>
    void GestionOptionsExtra(DialogueItem dialogue)
    {
        Debug.Log(dialogue.Id);
        ExtraOptions options = dialogue.ExtraOptions;
        if (options == null) return;

        foreach (var item in options.GetType().GetProperties().Select(prop => prop.Name))
        {
            //Debug.Log("nom propriete: " + item);
            object propValue = options.GetType().GetProperty(item).GetValue(options);
            //Debug.Log("valeur propriete:" + propValue);
            //if (propValue == null) return;

            switch (item)
            {
                case "ProchaineCible":
                    if (propValue == null) return;
                    else if ((string)propValue == "") { doPreemptiveQuit = true; }
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
                case "ContientChoix":
                    if (propValue == null) return;

                    //Debug.Log("dialogue.Id: " + dialogue.Id);
                    doitFaireChoix = true;

                    for (int i = 0; i < choixDialogues.Length; i++)
                    {
                        // aficher les boutons de choix
                        choixDialogues[i].SetActive(true);

                        // récupérer le texte du choix dans le json et l'afficher sur les boutons
                        Dictionary<string, Choix> dictChoix = tousLesDialogues.Prelude.Choix;

                        choixDialogues[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                            (GameManager.Instance.langue == LanguageManager.Language.English) ?
                            dictChoix[dialogue.Id + "_00" + i].En :
                            dictChoix[dialogue.Id + "_00" + i].Fr;

                        // gérer le comportement du choix en fonction de son type (dialogue ou event)
                        if (dictChoix[dialogue.Id + "_00" + i].Action == "dialogue")
                        {
                            choixDialogues[i].GetComponent<Button>().onClick.RemoveAllListeners();
                            choixDialogues[i].GetComponent<Button>().onClick.AddListener(() =>
                            {
                                doitFaireChoix = false;
                                ProgresserDialogue(int.Parse(dictChoix[dialogue.Id + "00" + i].Cible[^3..]) - 1);
                            });
                        }
                    }
                    break;
            }
        }

        return;
    }
}
