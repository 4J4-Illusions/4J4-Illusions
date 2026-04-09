using Globals;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

public class ControlesPersonnage : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject cameraJoueur;
    public GameObject texteInteraction, ondeSonore;
    [Header("Gameobjects utilisés pour le déboggage")]
    public LineRenderer indicateurPorteeInterac;

    [Header("Ajustement inspecteur"), Space]
    [Range(0f, 10f)] public float vitesseMouvement = 5f;
    [Range(0f, 3f)] public float vitesseRotation = .1f;
    [Range(1f, 5f)] public float porteeInteraction = 2f;
    public float[] multiplicateurMouvement = new float[2] {1f, 1.5f};
    public Vector3 ajustementPosCam = new(0, .5f, 0);
    [Header("Paramétrage des options de debug")]
    public bool debugMode = false;
    public bool debugPortee, debugStage;
    public StageJeu debugStageJeu = 0;
    // gestions, trackage et acces pour autres scripts
    public static bool isRunning, isMoving, canMove = true;
    // evenements
    public static Action<Vector3> OnPlayerOnde;

    Rigidbody rigidBody;
    InputAction mouvementAction, rotationAction, courseAction, interactionAction;
    Vector3 mouvementFinal, rotationFinal;
    int indexModifCourse = 0;
    TypeInteraction DefaultInterac = 0;
    RaycastHit hit;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        mouvementAction = InputSystem.actions.FindAction("Player/Move");
        rotationAction = InputSystem.actions.FindAction("Player/Look");
        courseAction = InputSystem.actions.FindAction("Player/Sprint");
        interactionAction = InputSystem.actions.FindAction("Player/Interact");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ondeSonore = ondeSonore.transform.GetChild(0).gameObject;
        cameraJoueur.transform.position = transform.position + ajustementPosCam;

        // active les elements de debogage
        if (debugMode)
        {
            if (debugPortee)
            {
                indicateurPorteeInterac.gameObject.SetActive(true);
                indicateurPorteeInterac.material = new Material(Shader.Find("Sprites/Default"));
                indicateurPorteeInterac.startColor = Color.red;
                indicateurPorteeInterac.endColor = Color.rebeccaPurple;
            }
            if (debugStage)
            {
                GameManager.Instance.stageJeu = debugStageJeu;
            }
        }

        if(GameManager.Instance.stageJeu == StageJeu.Foret)
        {
            DefaultInterac = TypeInteraction.Onde;
        }
    }

    void Update()
    {
        // calcul mouvement et rotation selon le input du clavier et de la souris
        mouvementFinal = multiplicateurMouvement[indexModifCourse] * vitesseMouvement * 
            (transform.forward * mouvementAction.ReadValue<Vector2>().y + transform.right * mouvementAction.ReadValue<Vector2>().x);
        rotationFinal = new Vector3(-rotationAction.ReadValue<Vector2>().y, rotationAction.ReadValue<Vector2>().x, 0) * vitesseRotation;
        if (!canMove) mouvementFinal *= 0;
        // applique rotation a camera et joueur
        cameraJoueur.GetComponent<GestionCamera>().rotationFinale = rotationFinal;
        transform.Rotate(0, rotationFinal.y, 0);

        // pour le debogage
        if (debugMode)
        {
            if (debugPortee)
            {
                indicateurPorteeInterac.SetPosition(0, transform.position);
                indicateurPorteeInterac.SetPosition(1, cameraJoueur.transform.position + cameraJoueur.transform.forward * porteeInteraction);
            }
        }

        // obtention des etats
        isMoving = mouvementFinal != Vector3.zero;
        isRunning = courseAction.IsPressed();

        // appliquer ou non le modificateur de vitesse
        indexModifCourse = isRunning ? 1 : 0;

        // decide comment se fera l'appel de la methode qui gere les interactions
        HandleInteractionInput();
    }

    void FixedUpdate()
    {
        // applique mouvement au joueur
        rigidBody.linearVelocity = mouvementFinal;
    }

    private void OnEnable()
    {
        // abonement evenement
        Gameplay.OnInteraction += (TypeInteraction interaction) => { if (interaction == TypeInteraction.Onde) OnPlayerOnde.Invoke(ondeSonore.transform.position); };
    }
    private void OnDisable()
    {
        // desabonnement evenement
        Gameplay.OnInteraction -= (TypeInteraction interaction) => { if (interaction == TypeInteraction.Onde) OnPlayerOnde.Invoke(ondeSonore.transform.position); };
    }



    void HandleInteractionInput()
    {
        // utilisation raycast pour detecter objet interactif dans la portee du joueur
        if (Physics.Raycast(transform.position, cameraJoueur.transform.forward, out hit, porteeInteraction) && !GameManager.Instance.InCalibInterac)
        {
            //Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.TryGetComponent<ObjetInteractif>(out ObjetInteractif objInter))
            {
                texteInteraction.SetActive(true);
                /*if (GameManager.Instance.InCalibInterac)
                {
                    Gameplay.Interaction(TypeInteraction.CalibrationStop);
                }
                else */
                if (interactionAction.WasPressedThisFrame())
                {
                    objInter.Interaction();
                    return;
                }
            }
        }
        else
        {
            texteInteraction.SetActive(false);
        }

        // interactions standard (sans necessiter un ObjectInteractif)
        if (interactionAction.WasPressedThisFrame() && (hit.collider == null || GameManager.Instance.InCalibInterac))
        {
            //Debug.Log("Interaction hors Objet Interactif");
            if (GameManager.Instance.stageJeu == StageJeu.Foret)
            {
                Gameplay.Interaction(DefaultInterac, ondeSonore);
            }
            else if (GameManager.Instance.InCalibInterac)
            {
                Gameplay.Interaction(TypeInteraction.CalibrationStop);
            }
            else
            {
                Gameplay.Interaction(DefaultInterac);
            }
            return;
        }
    }
}
