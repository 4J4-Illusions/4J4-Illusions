using UnityEngine;
using Globals;
using System;

public class GameManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GameManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public GameObject[] listeLampadaires = new GameObject[5];
    public GameObject overlayCalibration, player;

    [Header("Accès pour autres scripts"), Space]
    public StageJeu stageJeu = 0;
    public bool InCalibInterac, gameOver;
    public int indexLampCour = 0;

    // evenements
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
    }

    private void Start()
    {
        // setup du brouillard pour la forêt
        if (stageJeu == StageJeu.Foret)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = .3f;
        }
    }



    /// <summary>
    /// Lance l'évènement de fin de partie, qui peut être écouté par d'autres scripts pour déclencher des actions spécifiques à la fin du jeu (ex: afficher un écran de fin, arrêter les mouvements du joueur, etc.).
    /// </summary>
    public void FinDePartie()
    {
        OnGameOver.Invoke();
    }
}
