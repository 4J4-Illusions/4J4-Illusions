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

    [Header("Accès pour autres scripts"), Space]
    public StageJeu stageJeu = 0;
    public bool inCalibInterac, gameOver, allowGameLoop = true;
    public int indexLampCour = 0;
    public GameObject player;
    public ControlesPersonnage playerScript;

    // évènements
    public static Action OnGameOver;

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

        listeLampadaires = GameObject.FindGameObjectsWithTag("Lampadaire");
    }
    private void Start()
    {
        stageJeu = (StageJeu) SceneManager.GetActiveScene().buildIndex;

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<ControlesPersonnage>();

        // donne un ordre de complétion aléatoire aux lampadaires
        Array.Sort(listeLampadaires, (a, b) => Random.Range(-1, 1));
    }
    private void OnEnable()
    {
        ScriptMenuPauseDepuisInterface.OnMenuPause += GestionPause;
    }
    private void OnDisable()
    {
        ScriptMenuPauseDepuisInterface.OnMenuPause -= GestionPause;
    }



    /// <summary>
    /// Lance l'évènement de fin de partie, qui peut être écouté par d'autres scripts pour déclencher des actions spécifiques à la fin du jeu (ex: afficher un écran de fin, arrêter les mouvements du joueur, etc.).
    /// </summary>
    public void FinDePartie()
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
}
