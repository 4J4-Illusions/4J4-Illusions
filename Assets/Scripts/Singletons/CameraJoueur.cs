using Globals;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraJoueur : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public int[] limitesFOV = new int[2] { 60, 90 };
    public float[] limitesBobbing = new float[2] { .5f, .75f };
    public float vitesseChangementFOV = 1;
    public float vitesseBobbing = .1f;
    [Header("Accès publique pour autres scripts"), Space]
    public Vector3 rotationFinale = new(0, 0, 0);

    int targetFOV;
    float targetBobbing;
    ControlesPersonnage player;

    private void Start()
    {
        player = transform.parent.GetComponent<ControlesPersonnage>();

        if(GameManager.Instance.stageJeu == StageJeu.Foret)
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
        if (ControlesPersonnage.isMoving) transform.localPosition = new(0, Mathf.MoveTowards(transform.localPosition.y, targetBobbing, vitesseBobbing));
        else transform.localPosition = player.ajustementPosCam;

        if (transform.localPosition.y == limitesBobbing[0]) targetBobbing = limitesBobbing[1];
        else if (transform.localPosition.y == limitesBobbing[1]) targetBobbing = limitesBobbing[0];
    }
}
