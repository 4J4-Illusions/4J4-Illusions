using UnityEngine;
using Globals;

public class GameManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public GameObject[] listeLampadaires = new GameObject[5];
    public GameObject overlayCalibration, player;

    [Header("Accès pour autres scripts"), Space]
    public StageJeu stageJeu = 0;
    public bool InCalibInterac = false;
    public int indexLampCour = 0;

    void Awake()
    {
        // setup du singleton
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
    }

    private void Start()
    {
        if (stageJeu == StageJeu.Foret)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = .3f;
        }
    }
}
