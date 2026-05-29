using UnityEngine;
using UnityEngine.UI;

public class IndicateurLampadaireSurEcran : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject canvas;
    public GameObject indicateurUI;
    public Camera cameraJoueur;
    [Header("Ajustement inspecteur")]
    public Vector3 ajustPosIndic;
    public float fixDecalageY = 0;

    Vector3 posConvertie2d, canvasDimensions;
    float indicUIDimensions; // float au lieu de vector2 parce que l'indicateur est carré
    bool estBonCoteCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }
    private void OnEnable()
    {
        Invoke(nameof(DesactiveIndicateur), 3f);
    }
    private void OnDisable()
    {
        indicateurUI.SetActive(false);
    }



    /// <summary>
    /// Méthode qui s'assure que l'indicateur ne dépasse pas les extrmités du canvas en limitant la position aux dimensions du canvas
    /// </summary>
    /// <param name="posNonFormattee">Position non conforme de l'indicateur avant de l'adapter pour un bon affichage sur canvas</param>
    /// <returns></returns>
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
    /// <summary>
    /// Désactive le script et, par conséquent, l'indicateur. Cela permet de limiter la durée d'affichage de l'indicateur à l'écran après son activation.
    /// </summary>
    void DesactiveIndicateur()
    {
        enabled = false;
    }
}
