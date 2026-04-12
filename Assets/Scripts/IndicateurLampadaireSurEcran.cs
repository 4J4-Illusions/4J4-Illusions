using UnityEngine;
using UnityEngine.UI;

public class IndicateurLampadaireSurEcran : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject canvas;
    public GameObject prefabIndicateurUI, indicateurUI;
    public Camera cameraJoueur;

    [Header("Ajustement inspecteur"), Space]
    public Vector3 ajustPosIndic;
    public float fixDecalageY = 0;
    [Header("Paramétrage des options de debug")]
    public bool debugMode = false;
    public bool debugRay, debugIndicPerm;

    Vector3 posConvertie2d, canvasDimensions;
    // float au lieu de vector2 parce que l'indicateur est carre
    float indicUIDimensions;
    bool estBonCoteCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (debugMode)
        {
            if (debugIndicPerm) indicateurUI.SetActive(true);
        }

        canvasDimensions = canvas.GetComponent<RectTransform>().rect.size;
        indicUIDimensions = indicateurUI.GetComponent<RectTransform>().rect.width + indicateurUI.GetComponent<Outline>().effectDistance.x;
        //Debug.Log(indicUIDimensions);
    }

    // Update is called once per frame
    void Update()
    {
        posConvertie2d = Camera.main.WorldToScreenPoint(transform.position + ajustPosIndic);
        //Debug.Log("Position 2D convertie: " + posConvertie2d);
        estBonCoteCam = posConvertie2d.z > 0;
        indicateurUI.SetActive(estBonCoteCam);
        indicateurUI.GetComponent<RectTransform>().anchoredPosition = KeepInsideBorders(posConvertie2d);
        //indicateurUI.GetComponent<RectTransform>().anchoredPosition = posConvertie2d;
        //Debug.Log("Position finale indicateur: " + indicateurUI.GetComponent<RectTransform>().anchoredPosition);

        if (debugMode)
        {
            if (debugRay)
            {
                Ray camRay = Camera.main.ScreenPointToRay(posConvertie2d);
                Debug.DrawRay(camRay.origin, camRay.direction * 100, Color.red);
            }
        }
    }

    void OnEnable()
    {
        indicateurUI = Instantiate(prefabIndicateurUI, canvas.transform);
        Invoke(nameof(DesactiveIndicateur), 3f);
    }

    void OnDestroy()
    {
        Destroy(indicateurUI);
    }



    Vector3 KeepInsideBorders(Vector3 posNonFormattee)
    {
        if ((posNonFormattee.x - indicUIDimensions) < 0)
        {
            posNonFormattee.x = indicUIDimensions;
        }
        else if ((posNonFormattee.x + indicUIDimensions) > canvasDimensions.x)
        {
            posNonFormattee.x = canvasDimensions.x - indicUIDimensions;
        }

        if ((posNonFormattee.y - indicUIDimensions) < 0)
        {
            posNonFormattee.y = indicUIDimensions - fixDecalageY;
        }
        else if ((posNonFormattee.y + indicUIDimensions) > canvasDimensions.y)
        {
            posNonFormattee.y = canvasDimensions.y - (indicUIDimensions + fixDecalageY);
        }

        return posNonFormattee;
    }

    void DesactiveIndicateur()
    {
        Destroy(indicateurUI);
        enabled = false;
    }
}
