using UnityEngine;
using Globals;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public GameObject[] _listeLampadaires = new GameObject[5];
    public GameObject _overlayCalibration, player;

    // gestions, trackage et acces pour autres scripts
    public StageJeu stageJeu = 0;
    public bool InCalibInterac = false;
    public int indexLampCour = 0;
    public GameObject[] listeLampadaires { get { return _listeLampadaires; } }
    public GameObject overlayCalibration { get { return _overlayCalibration; } }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Debug.Log(Screen.currentResolution);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
