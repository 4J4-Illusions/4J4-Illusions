using UnityEngine;
using Globals;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject calibOverlay;
    public GameObject modelePapier, jumpscareImage;
    [Header("Projet")]
    public Material matPapier;
    public AudioClip sonJumpscare;

    [Header("Accès pour autres scripts"), Space(30)]
    public StageJeu stageJeu = 0;
    [Header("Modes")]
    public bool modeObtentionItem;
    [Header("États")]
    public bool gameOver;
    public bool allowGameLoop = true;
    [Header("Progression des niveaux")]
    public bool objectifComplete;
    public bool niveauComplete;
    public int indexLampCour, progressionBoutsPapier;
    public GameObject[] listeLampadaires = new GameObject[5],
        listeBoutsPapier = new GameObject[5];
    [Header("Autres")]
    public GameObject player, recompense, cameraJoueur;
    public ControlesPersonnage playerScript;
    public LanguageManager.Language langue = LanguageManager.Language.French;

    public static Action OnGameOver, OnLevelComplete;
    public static Action<float> OnLevelProgress;
    public static Action<StageJeu> OnObjectiveComplete;

    void Awake()
    {
        /*
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

        //Debug.Log(Screen.currentResolution);
        // affections de valeurs de gameplay
        // inclus un de fps et l'encrage de la souris
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //Resolution[] resolutions = Screen.resolutions;
        //foreach (var item in resolutions)
        //{
        //    Debug.Log(item);
        //    Debug.Log($"ratio: {(float)item.width / (float)item.height}");
        //}
    }
    private void OnEnable()
    {
        // abonnement évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause += GestionPause;
        DialogueManager.OnDialogueInteraction += GestionPause;
        OnObjectiveComplete += CompleterObjectif;
        OnLevelComplete += TransitionNiveau;
        SceneManager.sceneLoaded += Initialisation;
    }
    private void OnDisable()
    {
        // désabonnements évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause -= GestionPause;
        DialogueManager.OnDialogueInteraction -= GestionPause;
        OnObjectiveComplete -= CompleterObjectif;
        OnLevelComplete -= TransitionNiveau;
        SceneManager.sceneLoaded -= Initialisation;
    }



    /// <summary>
    /// Lance l'évènement de fin de partie, qui peut être écouté par d'autres scripts pour déclencher des actions spécifiques à la fin du jeu (ex: afficher un écran de fin, arrêter les mouvements du joueur, etc.).
    /// </summary>
    void FinDePartie()
    {
        OnGameOver?.Invoke();
        GestionPause(true);
        Invoke(nameof(Recommencer), 5f);
    }
    /// <summary>
    /// Met le jeu dans un état de pause ou de reprise en fonction de la valeur du paramètre "enPause".
    /// Lorsque le jeu est en pause, les contrôles du personnage sont désactivés et le curseur de la souris est libéré.
    /// Lorsque le jeu reprend, les contrôles du personnage sont réactivés et le curseur est verrouillé à nouveau.
    /// </summary>
    /// <param name="enPause">La valeur de pause</param>
    void GestionPause(bool enPause)
    {
        // recalcul l'état de pause pour s'assurer que toutes les sources de pause sont prises en compte
        enPause = (
            enPause |
            ScriptMenuPauseDepuisInterface.inMenu |
            DialogueManager.inDialogue
            );
        //Debug.Log("enPause: " + enPause);

        ControlesPersonnage.canMove = allowGameLoop = !enPause;
        //Debug.Log("allowGameLoop: " + allowGameLoop);

        if (!new StageJeu[] {StageJeu.Menu, StageJeu.Prelude, StageJeu.PreludeSuite }.Contains(stageJeu)) Cursor.lockState = (enPause) ? CursorLockMode.None : CursorLockMode.Locked;
    }
    /// <summary>
    /// Retourne au menu principal
    /// </summary>
    void Recommencer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Lance la séquence de jumpscare, qui correspond à la fin du jeu.
    /// </summary>
    public void Jumpscare(AudioSource aud = null)
    {
        if (stageJeu == StageJeu.Foret || stageJeu == StageJeu.Theatre)
        {
            GestionPause(true);
            jumpscareImage.SetActive(true);
            AudioManager.Instance.JouerSon(CategorieSon.SFX, sonJumpscare, aud);
        }

        FinDePartie();
    }
    /// <summary>
    /// Éxécute la logique de progression de l'objectif du niveau en fonction du stage de jeu actuel.
    /// </summary>
    /// <param name="stage">Le stage courant</param>
    public void AvancerObjectifNiveau(StageJeu stage)
    {
        Debug.Log("L'objectif progresse");

        switch (stage)
        {
            case StageJeu.Desert:
                progressionBoutsPapier++;

                // si tous les bouts de papier sont récoltés, l'objectif est complété
                if (progressionBoutsPapier == 5) { objectifComplete = true; OnObjectiveComplete?.Invoke(stage); }
                break;
            case StageJeu.Foret:
                indexLampCour++;

                // si tous les lampadaires sont allumés, l'objectif est complété
                if (indexLampCour == 5) { objectifComplete = true; OnObjectiveComplete?.Invoke(stage); }
                break;
            case StageJeu.Theatre:
                break;
        }

        QueteCompteur.Ajouter(1);
        if (stage == StageJeu.Theatre) OnLevelProgress?.Invoke(.05f);
        else OnLevelProgress?.Invoke(.25f);
    }
    /// <summary>
    /// Éxécute la logique de complétion de l'objectif du niveau en fonction du stage de jeu actuel.
    /// </summary>
    /// <param name="stage">Le stage courant</param>
    void CompleterObjectif(StageJeu stage)
    {
        Debug.Log("L'objectif est complété");
        recompense.SetActive(true);

        switch (stage)
        {
            case StageJeu.Desert:
                break;
            case StageJeu.Foret:
                foreach (GameObject monstre in GameObject.FindGameObjectsWithTag("Monstre"))
                {
                    Destroy(monstre);
                }
                break;
            case StageJeu.Theatre:
                break;
        }
    }
    /// <summary>
    /// Complète le niveau an llançant l'évènement de complétion de niveau.
    /// </summary>
    public void TerminerNiveau()
    {
        niveauComplete = true;
        OnLevelComplete?.Invoke();
    }
    /// <summary>
    /// Fait la transition vers le niveau suivant.
    /// </summary>
    void TransitionNiveau()
    {
        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else SceneManager.LoadScene(0);
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
    }
    /// <summary>
    /// Initialisation des propriétés de gameManager à chaque chargement de scène.
    /// </summary>
    /// <param name="scene">La scène chargée</param>
    /// <param name="mode">Le mode de chargement de la scène</param>
    void Initialisation(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Chargement de la scène: " + scene.name);
        Debug.Log("Build index: " + scene.buildIndex);

        // affectations valeurs générales importantes
        stageJeu = (StageJeu)scene.buildIndex;
        Debug.Log("Stage du jeu: " + stageJeu);

        // reset du compteur de quête
        QueteCompteur.ResetCompteur();

        // reset les compteurs et états
        indexLampCour = progressionBoutsPapier = 0;
        objectifComplete = niveauComplete = gameOver = false;
        allowGameLoop = true;


        //// affections quand la scene est pas celle de menu
        //if (scene.buildIndex != 0)
        //{
        //    // etat curseur
        //    Cursor.lockState = CursorLockMode.Locked;

        //    AudioManager.Instance.SceneLoadedInit(stageJeu);
        //    DialogueManager.Instance.SceneLoadedInit(stageJeu);
        //}
        // affections quand la scene est ni celle de menu ni celle de prelude
        if (!new int[] { 0, 4 }.Contains(scene.buildIndex))
        {
            // etat curseur
            Cursor.lockState = CursorLockMode.Locked;

            // initialisation/réinitialisation d'autres managers
            AudioManager.Instance.SceneLoadedInit(stageJeu);
            DialogueManager.Instance.SceneLoadedInit(stageJeu);

            // affectations éléments de la hiérarchie
            player = GameObject.FindWithTag("Player");
            cameraJoueur = player.transform.Find("CameraJoueur").gameObject;
            playerScript = player.GetComponent<ControlesPersonnage>();

            // recompense de niveau
            recompense = GameObject.FindWithTag("Recompense");
            //Debug.Log(recompense.name);
            if (recompense != null) recompense.SetActive(false);
        }
        else
        {
            allowGameLoop = false;
        }


        // autres initialisation selon le stage de jeu
        switch (stageJeu)
        {
            case StageJeu.Desert:
                // bouts de papier
                modelePapier = cameraJoueur.transform.Find("PapiersACompleter").gameObject;
                for (int i = 0; i < modelePapier.transform.childCount; i++)
                {
                    //Debug.Log(prefabModelePapier.transform.GetChild(i).name);
                    listeBoutsPapier[i] = modelePapier.transform.GetChild(i).gameObject;
                }
                break;
            case StageJeu.Foret:
                // image jumpscare
                jumpscareImage = GameObject.FindWithTag("Jumpscare");
                jumpscareImage.SetActive(false);
                jumpscareImage.transform.localScale = Vector3.one;

                // liste de lampadaires
                listeLampadaires = GameObject.FindGameObjectsWithTag("Lampadaire");
                // ordre aléatoire aux lampadaires
                Array.Sort(listeLampadaires, (a, b) => Random.Range(-1, 1));
                break;
            case StageJeu.Theatre:
                // overlay de calibration
                calibOverlay = GameObject.FindWithTag("CalibOverlay");
                calibOverlay.SetActive(false);
                calibOverlay.transform.localScale = Vector3.one;

                // image jumpscare
                jumpscareImage = GameObject.FindWithTag("Jumpscare");
                jumpscareImage.SetActive(false);
                jumpscareImage.transform.localScale = Vector3.one * 2;
                break;
        }
    }
}
