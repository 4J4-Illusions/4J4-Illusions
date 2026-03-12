using UnityEngine;

public class InidcateurLampadaireSurEcran : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject indicateurUI;
    public Camera cameraJoueur;

    [Header("Ajustement inspecteur"), Space]
    public Vector3 ajustPosIndic = new(0, 0, 0);
    [Header("Paramétrage des options de debug")]
    public bool debugMode = false;
    public bool debugRay, debugIndicPerm;

    Vector2 posConverti2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (debugMode)
        {
            if (debugIndicPerm) indicateurUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        posConverti2d = Camera.main.WorldToScreenPoint(transform.position + ajustPosIndic);
        Debug.Log("Position 2D : " + posConverti2d);

        if (debugMode)
        {
            if (debugRay)
            {
                Ray camRay = Camera.main.ScreenPointToRay(posConverti2d);
                Debug.DrawRay(camRay.origin, camRay.direction * 100, Color.red);
            }
        }

        indicateurUI.GetComponent<RectTransform>().anchoredPosition = posConverti2d;
    }
}
