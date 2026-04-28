using UnityEngine;
using Globals;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject overlayCalibration;
    public GameObject modelePapier;
    public GameObject jumpscareImage;
    [Header("Projet")]
    public Material matPapier;
    public AudioClip sonJumpscare;

    [Header("Accès pour autres scripts"), Space(30)]
    public StageJeu stageJeu = 0;
    [Header("Modes")]
    public bool inCalibInterac;
    public bool modeObtentionItem;
    [Header("États")]
    public bool gameOver;
    public bool allowGameLoop = true;
    [Header("Progression des niveaux")]
    public bool objectifComplete;
    public bool niveauComplete;
    public int indexLampCour;
    public int progressionBoutsPapier;
    public GameObject[] listeLampadaires = new GameObject[5];
    public GameObject[] listeBoutsPapier = new GameObject[5];
    [Header("Autres")]
    public GameObject player;
    public GameObject recompense;
    public GameObject cameraJoueur;
    public ControlesPersonnage playerScript;

    // évènements
    public static Action OnGameOver, OnLevelProgress, OnLevelComplete;
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

        //Initialisation(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    private void OnEnable()
    {
        // abonnement évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause += GestionPause;
        OnObjectiveComplete += CompleterObjectif;
        OnLevelComplete += TransitionNiveau;
        SceneManager.sceneLoaded += Initialisation;
    }
    private void OnDisable()
    {
        // désabonnements évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause -= GestionPause;
        OnObjectiveComplete -= CompleterObjectif;
        OnLevelComplete -= TransitionNiveau;
        SceneManager.sceneLoaded -= Initialisation;
    }



    /// <summary>
    /// Lance l'évènement de fin de partie, qui peut être écouté par d'autres scripts pour déclencher des actions spécifiques à la fin du jeu (ex: afficher un écran de fin, arrêter les mouvements du joueur, etc.).
    /// </summary>
    void FinDePartie()
    {
        OnGameOver.Invoke();
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
        ControlesPersonnage.canMove = allowGameLoop = !enPause;

        Cursor.lockState = (enPause) ? CursorLockMode.None : CursorLockMode.Locked;
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
    public void Jumpscare()
    {
        jumpscareImage.SetActive(true);
        AudioManager.Instance.JouerSon(CategorieSon.SFX, sonJumpscare);

        FinDePartie();
    }
    public void AvancerObjectifNiveau(StageJeu stage)
    {
        switch (stage)
        {
            case StageJeu.Desert:
                progressionBoutsPapier++;

                // si tous les bouts de papier sont récoltés, l'objectif est complété
                if (progressionBoutsPapier == 5) { objectifComplete = true; OnObjectiveComplete.Invoke(stage); }
                break;
            case StageJeu.Foret:
                indexLampCour++;

                // si tous les lampadaires sont allumés, l'objectif est complété
                if (indexLampCour == 5) { objectifComplete = true; OnObjectiveComplete.Invoke(stage); }
                break;
            case StageJeu.Theatre:
                break;
        }

        QueteCompteur.Ajouter(1);
        OnLevelProgress.Invoke();
    }
    void CompleterObjectif(StageJeu stage)
    {
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
    public void TerminerNiveau()
    {
        niveauComplete = true;
        OnLevelComplete.Invoke();
    }
    void TransitionNiveau()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Initialisation(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Chargement de la scène: " + scene.name);

        // affectations valeurs générales importantes
        Cursor.lockState = CursorLockMode.Locked;
        stageJeu = (StageJeu)scene.buildIndex;

        // affectations éléments de la hiérarchie
        player = GameObject.FindWithTag("Player");
        cameraJoueur = player.transform.Find("CameraJoueur").gameObject;
        playerScript = player.GetComponent<ControlesPersonnage>();
        recompense = GameObject.FindWithTag("Recompense");
        //Debug.Log(GameObject.FindWithTag("Recompense"));
        //Debug.Log(Time.realtimeSinceStartup);
        //Debug.Log(recompense.name);
        if (recompense != null) recompense.SetActive(false);

        // reset les compteurs et états
        indexLampCour = progressionBoutsPapier = 0;
        objectifComplete = niveauComplete = gameOver = false;
        allowGameLoop = true;

        // autres initialisation selon le stage de jeu
        switch (stageJeu)
        {
            case StageJeu.Desert:
                modelePapier = cameraJoueur.transform.Find("PapiersACompleter").gameObject;
                for (int i = 0; i < modelePapier.transform.childCount; i++)
                {
                    //Debug.Log(prefabModelePapier.transform.GetChild(i).name);
                    listeBoutsPapier[i] = modelePapier.transform.GetChild(i).gameObject;
                }
                break;
            case StageJeu.Foret:
                jumpscareImage = GameObject.FindWithTag("Jumpscare");
                jumpscareImage.SetActive(false);
                listeLampadaires = GameObject.FindGameObjectsWithTag("Lampadaire");
                // ordre aléatoire aux lampadaires
                Array.Sort(listeLampadaires, (a, b) => Random.Range(-1, 1));
                break;
            case StageJeu.Theatre:
                break;
        }
    }
}
