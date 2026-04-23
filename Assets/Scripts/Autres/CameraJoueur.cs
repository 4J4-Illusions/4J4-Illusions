using Globals;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class CameraJoueur : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public int[] limitesFOV = new int[2] { 60, 90 };
    public float 
        vitesseChangementFOV = 1,
        vitesseBobbing = .1f,
        variationBobbing = .1f;
    public bool allowBobbing;
    [Header("Accès publique pour autres scripts"), Space]
    public Vector3 rotationFinale = new(0, 0, 0);
    public Volume volume;

    int targetFOV;
    float targetBobbing;
    ControlesPersonnage player;
    // constantes
    float POSITION_Y_INITIALE;

    private void Awake()
    {
        GetComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = true;
        POSITION_Y_INITIALE = transform.localPosition.y;
        //Debug.Log($"Limities du bobbing: [base, variation] = [{POSITION_Y_INITIALE}, {POSITION_Y_INITIALE - variationBobbing}]");
        volume = GetComponent<Volume>();
    }
    private void Start()
    {
        player = transform.parent.GetComponent<ControlesPersonnage>();

        // active le mode de rendu de la profondeur pour le niveau de la foret (pour l'effet d'onde sonore)
        if (GameManager.Instance.stageJeu == StageJeu.Foret)
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // applique rotation a camera
        if (!ControlesPersonnage.canMove) rotationFinale *= 0;
        transform.Rotate(rotationFinale.x, 0, 0);

        // limite les mouvements de la camera
        transform.localRotation =
            new(Mathf.Clamp(transform.localRotation.x, Quaternion.Euler(-90, 0, 0).x, Quaternion.Euler(90, 0, 0).x),
            transform.localRotation.y,
            transform.localRotation.z,
            transform.localRotation.w);

        // changement champ de vision si le joueur cours ou pas
        targetFOV = (ControlesPersonnage.isRunning) ? limitesFOV[1] : limitesFOV[0];
        GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(GetComponent<Camera>().fieldOfView, targetFOV, vitesseChangementFOV);

        // effet de bobbing (mvt bond naturel de la tete quand on marche)
        if (allowBobbing)
        {
            if (ControlesPersonnage.isMoving) transform.localPosition = new(0, Mathf.MoveTowards(transform.localPosition.y, targetBobbing, vitesseBobbing), .2f);
            else transform.localPosition = player.ajustementPosCam;

            if (transform.localPosition.y >= POSITION_Y_INITIALE) targetBobbing = POSITION_Y_INITIALE - variationBobbing;
            else if (transform.localPosition.y <= POSITION_Y_INITIALE - variationBobbing) targetBobbing = POSITION_Y_INITIALE;
        }
    }
}
