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

    Rigidbody rigidBody;
    InputAction mouvementAction, rotationAction, courseAction, interactionAction;
    Vector3 mvtFinal, rotFinal;
    int indexModifCourse = 0;
    TypeInteraction DefaultInterac = 0;
    RaycastHit hit;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        mouvementAction = InputSystem.actions.FindAction("Player/Move");
        rotationAction = InputSystem.actions.FindAction("Player/Look");
        courseAction = InputSystem.actions.FindAction("Player/Sprint");
        interactionAction = InputSystem.actions.FindAction("Player/Interact");

        Cursor.lockState = CursorLockMode.Locked;
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
                GameManager.stage = debugStageJeu;
            }
        }

        if(GameManager.stage == StageJeu.Foret)
        {
            DefaultInterac = TypeInteraction.Onde;
        }
    }

    private void Update()
    {
        // calcul mouvement et rotation selon le input de la souris
        mvtFinal = multiplicateurMouvement[indexModifCourse] * vitesseMouvement * (transform.forward * mouvementAction.ReadValue<Vector2>().y + transform.right * mouvementAction.ReadValue<Vector2>().x);
        rotFinal = new Vector3(-rotationAction.ReadValue<Vector2>().y, rotationAction.ReadValue<Vector2>().x, 0) * vitesseRotation;
        // applique rotation a camera et joueur
        cameraJoueur.transform.Rotate(rotFinal.x, 0, 0);
        transform.Rotate(0, rotFinal.y, 0);

        // pour le debogage
        if (debugMode)
        {
            if (debugPortee)
            {
                indicateurPorteeInterac.SetPosition(0, transform.position);
                indicateurPorteeInterac.SetPosition(1, cameraJoueur.transform.position + cameraJoueur.transform.forward * porteeInteraction);
            }
        }

        // utilisation raycast pour detecter objet interactif dans la portee du joueur
        if(Physics.Raycast(transform.position, cameraJoueur.transform.forward, out hit, porteeInteraction))
        {
            //Debug.Log(hit.transform.gameObject.name);
            if(hit.transform.gameObject.TryGetComponent<ObjetInteractif>(out ObjetInteractif objInter))
            {
                texteInteraction.SetActive(true);
                if (interactionAction.WasPressedThisFrame())
                {
                    objInter.Interaction();
                }
            }
        }
        else
        {
            texteInteraction.SetActive(false);
        }

        // actions selon le input du clavier
        if (courseAction.IsPressed())
        {
            indexModifCourse = 1;
        }
        else
        {
            indexModifCourse = 0;
        }
        if (interactionAction.WasPressedThisFrame() && hit.collider == null)
        {
            if(GameManager.stage == StageJeu.Foret)
            {
                Gameplay.Interaction(DefaultInterac, ondeSonore);
            }
            else if (GameManager.InCalibInterac)
            {
                Gameplay.Interaction(TypeInteraction.CalibrationStop);
            }
            else
            {
                Gameplay.Interaction(DefaultInterac);
            }
        }
    }

    private void FixedUpdate()
    {
        // applique mouvement au joueur
        rigidBody.linearVelocity = mvtFinal;
    }

}
