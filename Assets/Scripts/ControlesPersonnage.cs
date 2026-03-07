using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlesPersonnage : MonoBehaviour
{
    [Header("Affectation inspecteur")]
    public GameObject cameraJoueur;

    [Header("Ajustement inspecteur")]
    [Range(0f, 10f)] public float vitesseMouvement = 5f;
    public float[] multiplicateurMouvement = new float[2] {1f, 1.5f};
    [Range(0f, 3f)] public float vitesseRotation = .1f;
    public Vector3 ajustementPosCam = new(0, .5f, 0);

    public static event Action OnPlayerInteract;

    Rigidbody rigidBody;
    InputAction mouvementAction, rotationAction, courseAction, interactionAction;
    Vector3 mvtFinal, rotFinal;
    int valeurModifCourse = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mouvementAction = InputSystem.actions.FindAction("Player/Move");
        rotationAction = InputSystem.actions.FindAction("Player/Look");
        courseAction = InputSystem.actions.FindAction("Player/Sprint");
        interactionAction = InputSystem.actions.FindAction("Player/Interact");

        Cursor.lockState = CursorLockMode.Locked;

        cameraJoueur.transform.position = transform.position + ajustementPosCam;
    }

    private void Update()
    {
        // applique rotation a camera et joueur
        rotFinal = new Vector3(-rotationAction.ReadValue<Vector2>().y, rotationAction.ReadValue<Vector2>().x, 0) * vitesseRotation;
        cameraJoueur.transform.Rotate(rotFinal.x, 0, 0);
        transform.Rotate(0, rotFinal.y, 0);

        if (courseAction.IsPressed())
        {
            valeurModifCourse = 1;
        }
        else
        {
            valeurModifCourse = 0;
        }

        if (interactionAction.WasPressedThisFrame())
        {
            OnPlayerInteract.Invoke();
        }
    }

    private void FixedUpdate()
    {
        // applique mouvement ua joueur
        mvtFinal = multiplicateurMouvement[valeurModifCourse] * vitesseMouvement * (transform.forward * mouvementAction.ReadValue<Vector2>().y + transform.right * mouvementAction.ReadValue<Vector2>().x);
        rigidBody.linearVelocity = mvtFinal;
    }

}
