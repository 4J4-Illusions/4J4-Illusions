using UnityEngine;
using Globals;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public GameObject[] listeLampadaires = new GameObject[5];
    public GameObject overlayCalibration;
    public GameObject modelePapier;
    public GameObject[] listeBoutsPapier = new GameObject[5];
    public Material matPapier;

    [Header("Accès pour autres scripts"), Space]
    public StageJeu stageJeu = 0;
    public bool
        inCalibInterac, modeObtentionItem, /*modes*/
        gameOver, allowGameLoop = true, /*états*/
        objectifComplete, niveauComplete /*progression de niveau*/;
    public int indexLampCour = 0;
    public GameObject player, recompense, cameraJoueur;
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
        Cursor.lockState = CursorLockMode.Locked;

        stageJeu = (StageJeu)SceneManager.GetActiveScene().buildIndex;

        player = GameObject.FindWithTag("Player");
        cameraJoueur = player.transform.Find("CameraJoueur").gameObject;
        playerScript = player.GetComponent<ControlesPersonnage>();

        listeLampadaires = GameObject.FindGameObjectsWithTag("Lampadaire");
        // ordre aléatoire aux lampadaires
        Array.Sort(listeLampadaires, (a, b) => Random.Range(-1, 1));

        recompense = GameObject.FindWithTag("Recompense");
        recompense.SetActive(false);

        objectifComplete = niveauComplete = false;

        for (int i = 0; i < modelePapier.transform.childCount; i++)
        {
            //Debug.Log(prefabModelePapier.transform.GetChild(i).name);
            listeBoutsPapier[i] = modelePapier.transform.GetChild(i).gameObject;
        }
    }
    private void OnEnable()
    {
        // abonnement évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause += GestionPause;
        OnObjectiveComplete += ObjectifComplete;
        OnLevelComplete += TransitionNiveau;
    }
    private void OnDisable()
    {
        // désabonnements évènements
        ScriptMenuPauseDepuisInterface.OnMenuPause -= GestionPause;
        OnObjectiveComplete -= ObjectifComplete;
        OnLevelComplete -= TransitionNiveau;
    }



    /// <summary>
    /// Lance l'évènement de fin de partie, qui peut être écouté par d'autres scripts pour déclencher des actions spécifiques à la fin du jeu (ex: afficher un écran de fin, arrêter les mouvements du joueur, etc.).
    /// </summary>
    void FinDePartie()
    {
        OnGameOver.Invoke();
        GestionPause(true);
        Invoke(nameof(RetourMenu), 5f);
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
    void RetourMenu()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Lance la séquence de jumpscare, qui correspond à la fin du jeu.
    /// </summary>
    public void Jumpscare()
    {
        FinDePartie();
    }
    public void ProgressionObjectifNiveau(StageJeu stage)
    {
        switch (stage)
        {
            case StageJeu.Desert:
                break;
            case StageJeu.Foret:
                indexLampCour++;

                // si tous les lampadaires sont allumés, l'objectif est complété
                if (indexLampCour == 5) { objectifComplete = true; OnObjectiveComplete.Invoke(stage); }
                break;
            case StageJeu.Theatre:
                break;
        }

        //OnLevelProgress.Invoke();
    }
    void ObjectifComplete(StageJeu stage)
    {
        recompense.SetActive(true);

        switch (stage)
        {
            case StageJeu.Desert:
                break;
            case StageJeu.Foret:
                foreach(GameObject monstre in GameObject.FindGameObjectsWithTag("Monstre"))
                {
                    Destroy(monstre);
                }
                break;
            case StageJeu.Theatre:
                break;
        }
    }

    public void NiveauTermine()
    {
        niveauComplete = true;
        OnLevelComplete.Invoke();
    }
    void TransitionNiveau()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
