using Globals;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ControlesPersonnage : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject cameraJoueur;
    public GameObject texteInteraction, ondeSonore;
    public ScriptMenuPauseDepuisInterface controllerMenu;
    [Header("Ajustement inspecteur")]
    public float vitesseMouvement = 5f,
        vitesseRotation = .1f,
        porteeInteraction = 2f;
    public float[] multiplicateurMouvement = new float[2] { 1f, 1.5f };
    public Vector3 ajustementPosCam = new(0, .6f, .2f);

    public static bool isRunning, isMoving, canMove = true;
    public static Action OnPlayerOnde;

    Rigidbody rigidBody;
    InputAction mouvementAction, rotationAction, courseAction, interactionAction;
    Vector3 mouvementFinal, rotationFinale;
    int indexModifCourse = 0;
    TypeInteraction DefaultInterac = 0;
    RaycastHit hit;
    AudioSource audsrc;
    Animator animPerso;
    NavMeshAgent agent;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        //audsrc = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        mouvementAction = InputSystem.actions.FindAction("Player/Move");
        rotationAction = InputSystem.actions.FindAction("Player/Look");
        courseAction = InputSystem.actions.FindAction("Player/Sprint");
        interactionAction = InputSystem.actions.FindAction("Player/Interact");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraJoueur.transform.localPosition = ajustementPosCam;
        animPerso = transform.Find("Model").GetComponent<Animator>();
        //Debug.Log(animPerso.transform.name);
        audsrc = GetComponent<AudioManagerConnect>().audsrc;
        //Debug.Log(audsrc);

        if (GameManager.Instance.stageJeu == StageJeu.Foret)
        {
            DefaultInterac = TypeInteraction.Onde;
        }
    }
    void Update()
    {
        // calcul mouvement et rotation selon le input du clavier et de la souris
        mouvementFinal = multiplicateurMouvement[indexModifCourse] * vitesseMouvement *
            (transform.forward * mouvementAction.ReadValue<Vector2>().y + transform.right * mouvementAction.ReadValue<Vector2>().x);
        //Debug.Log(mouvementFinal);
        rotationFinale = new Vector3(-rotationAction.ReadValue<Vector2>().y, rotationAction.ReadValue<Vector2>().x, 0) * vitesseRotation;
        if (!canMove) mouvementFinal = rotationFinale *= 0;
        // applique rotation a camera et joueur
        cameraJoueur.GetComponent<CameraJoueur>().rotationFinale = rotationFinale;
        transform.Rotate(0, rotationFinale.y, 0);

        // obtention des etats
        isMoving = mouvementFinal != Vector3.zero;
        isRunning = courseAction.IsPressed();

        // appliquer ou non le modificateur de vitesse
        indexModifCourse = isRunning ? 1 : 0;
        // s'assure que audio source est pas null
        if (audsrc == null)
        {
            audsrc = GetComponent<AudioManagerConnect>().audsrc;
        }
        audsrc.pitch = multiplicateurMouvement[indexModifCourse];

        // decide comment se fera l'appel de la methode qui gere les interactions
        HandleInteractionInput();

        // controlle du son de marche selon son mouvement
        if (isMoving && !audsrc.isPlaying)
        {
            //audsrc.volume = AudioManager.Instance.SetClipVolume(AudioManager.Instance.GetClipCategory(audsrc.clip));
            //audsrc.Play();
            audsrc = AudioManager.Instance.JouerSon(CategorieSon.Ambience, audsrc.clip);
        }
        else if (!isMoving && audsrc.isPlaying)
        {
            audsrc.Stop();
        }

        // gestion des animations
        GererAnimations();
    }
    void FixedUpdate()
    {
        // applique mouvement au joueur
        if (agent != null)
        {
            agent.Move(new Vector3(mouvementFinal.x, 0, mouvementFinal.z) * Time.deltaTime);
        }
        else rigidBody.linearVelocity = new Vector3(mouvementFinal.x, rigidBody.linearVelocity.y, mouvementFinal.z);
    }
    private void OnEnable()
    {
        // abonement évènement
        Gameplay.OnInteraction += (TypeInteraction interaction) => { if (interaction == TypeInteraction.Onde) OnPlayerOnde.Invoke(); };
    }
    private void OnDisable()
    {
        // désabonnement évènement
        Gameplay.OnInteraction -= (TypeInteraction interaction) => { if (interaction == TypeInteraction.Onde) OnPlayerOnde.Invoke(); };
    }



    /// <summary>
    /// Méthode qui gère les interactions du joueur.
    /// </summary>
    void HandleInteractionInput()
    {
        // ouverture du menu de pause
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (controllerMenu.settingsUI.activeSelf) controllerMenu.CloseSettings();
            else controllerMenu.OpenSettingsWithDelay();
            return;
        }

        // utilisation raycast pour detecter objet interactif dans la portee du joueur
        //Debug.DrawRay(cameraJoueur.transform.position, cameraJoueur.transform.forward * porteeInteraction, Color.red);
        if (Physics.Raycast(
            origin: cameraJoueur.transform.position,
            direction: cameraJoueur.transform.forward,
            hitInfo: out hit,
            maxDistance: porteeInteraction) && !CalibrationManager.inCalibrationInteraction)
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.TryGetComponent<ObjetInteractif>(out ObjetInteractif objInter))
            {
                texteInteraction.SetActive(true);
                if (interactionAction.WasPressedThisFrame())
                {
                    objInter.Interaction();

                    // joue l'animation de briquet si l'interaction concerne un lampadaire
                    if (objInter.typeInterac == TypeInteraction.Lampadaire)
                    {
                        //Debug.Log("Animer briquet");
                        animPerso.SetTrigger("triggerBriquet");
                    }

                    return;
                }
            }
        }
        else
        {
            texteInteraction.SetActive(false);
        }

        // interactions standard (sans passer directemenr par un objet interactif)
        if (interactionAction.WasPressedThisFrame() && (hit.collider == null || CalibrationManager.inCalibrationInteraction))
        {
            //Debug.Log("Interaction hors objet interactif");
            if (CalibrationManager.inCalibrationInteraction)
            {
                Gameplay.Interaction(TypeInteraction.CalibrationStop);
            }
            else if (DialogueManager.Instance.inDialogue)
            {
                Gameplay.Interaction(TypeInteraction.Dialogue);
            }
            else if (GameManager.Instance.stageJeu == StageJeu.Foret)
            {
                Gameplay.Interaction(DefaultInterac, ondeSonore);
            }
            else
            {
                Gameplay.Interaction(DefaultInterac);
            }
        }
    }
    /// <summary>
    /// Gère les animations du personnage en fonction de son état de mouvement et de course.
    /// </summary>
    void GererAnimations()
    {
        animPerso.SetBool("isMoving", isMoving);
        animPerso.SetBool("isRunning", isRunning);
    }
}
