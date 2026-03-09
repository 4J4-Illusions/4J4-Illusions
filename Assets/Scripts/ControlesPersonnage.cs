using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlesPersonnage : MonoBehaviour
{
    [Header("Affectation inspecteur")]
    public GameObject cameraJoueur;
    public GameObject texteInteraction;
    public LineRenderer debugRangeInteract;

    [Header("Ajustement inspecteur")]
    [Range(0f, 10f)] public float vitesseMouvement = 5f;
    [Range(0f, 3f)] public float vitesseRotation = .1f;
    [Range(1f, 5f)] public float porteeInteraction = 2f;
    public float[] multiplicateurMouvement = new float[2] {1f, 1.5f};
    public Vector3 ajustementPosCam = new(0, .5f, 0);
    public bool debugMode = false;

    Rigidbody rigidBody;
    InputAction mouvementAction, rotationAction, courseAction, interactionAction;
    Vector3 mvtFinal, rotFinal;
    int indexModifCourse = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // affectations
        rigidBody = GetComponent<Rigidbody>();
        mouvementAction = InputSystem.actions.FindAction("Player/Move");
        rotationAction = InputSystem.actions.FindAction("Player/Look");
        courseAction = InputSystem.actions.FindAction("Player/Sprint");
        interactionAction = InputSystem.actions.FindAction("Player/Interact");

        Cursor.lockState = CursorLockMode.Locked;

        cameraJoueur.transform.position = transform.position + ajustementPosCam;

        // active les elements de debogage
        if (debugMode)
        {
            debugRangeInteract.gameObject.SetActive(true);
            debugRangeInteract.material = new Material(Shader.Find("Sprites/Default"));
            debugRangeInteract.startColor = Color.red;
            debugRangeInteract.endColor = Color.rebeccaPurple;
        }
    }

    private void Update()
    {
        // calcul mouvement et rotation selon input
        mvtFinal = multiplicateurMouvement[indexModifCourse] * vitesseMouvement * (transform.forward * mouvementAction.ReadValue<Vector2>().y + transform.right * mouvementAction.ReadValue<Vector2>().x);
        rotFinal = new Vector3(-rotationAction.ReadValue<Vector2>().y, rotationAction.ReadValue<Vector2>().x, 0) * vitesseRotation;

        // pour le debogage
        if (debugMode)
        {
            debugRangeInteract.SetPosition(0, transform.position);
            debugRangeInteract.SetPosition(1, cameraJoueur.transform.position + cameraJoueur.transform.forward * porteeInteraction);
        }

        // applique rotation a camera et joueur
        cameraJoueur.transform.Rotate(rotFinal.x, 0, 0);
        transform.Rotate(0, rotFinal.y, 0);

        // actions selon d'autres touches
        if (courseAction.IsPressed())
        {
            indexModifCourse = 1;
        }
        else
        {
            indexModifCourse = 0;
        }


        // utilisation raycast pour detecter objet interactif dans la portee du joueur
        if(Physics.Raycast(transform.position, cameraJoueur.transform.forward, out RaycastHit hit, porteeInteraction))
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

    }

    private void FixedUpdate()
    {
        // applique mouvement au joueur
        rigidBody.linearVelocity = mvtFinal;
    }

}
