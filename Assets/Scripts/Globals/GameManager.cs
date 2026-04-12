using UnityEngine;
using Globals;

public class GameManager : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject[] _listeLampadaires = new GameObject[5];
    public GameObject _overlayCalibration;

    // gestions, trackage et acces pour autres scripts
    public static StageJeu stageJeu = 0;
    public static bool InCalibInterac = false;
    public static int indexLampCour = 0;
    public static GameObject[] listeLampadaires;
    public static GameObject overlayCalibration;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        listeLampadaires = _listeLampadaires;
        overlayCalibration = _overlayCalibration;
    }
}
