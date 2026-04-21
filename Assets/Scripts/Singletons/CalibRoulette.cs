using UnityEngine;

public class CalibRoulette : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject background;
    public GameObject pointeur;
    [Header("Ajustement inspecteur"), Space]
    public float[] rangeVitRot = new float[2] { -10f, -5f };

    float vitRotation;
    RectTransform rectPointeur;
    float[] sectionRouletteAToucher = new float[2];
    const float ROTAT_OFFSET = 200;
    // constantes
    const int RANGE_ZONE_ROULETTE = 40, RANGE_ECART_ROULETTE = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectPointeur = pointeur.GetComponent<RectTransform>();
        rectPointeur.localEulerAngles = new Vector3(0, 0, ROTAT_OFFSET);
    }
    // Update is called once per frame
    void Update()
    {
        rectPointeur.Rotate(0, 0, vitRotation);
    }
    void OnEnable()
    {
        vitRotation = Random.Range(rangeVitRot[0], rangeVitRot[1]);
        background.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        sectionRouletteAToucher[0] = FixRotationMismatch(background.GetComponent<RectTransform>().localEulerAngles.z - RANGE_ZONE_ROULETTE/2);
        sectionRouletteAToucher[1] = FixRotationMismatch(sectionRouletteAToucher[0] + RANGE_ZONE_ROULETTE);
        Debug.Log($"[{sectionRouletteAToucher[0]}, {sectionRouletteAToucher[1]}]");
    }



    /// <summary>
    /// Arrête la roulette et vérifie si le pointeur est dans la zone à toucher pour réussir la calibration
    /// </summary>
    public void StopRoulette()
    {
        vitRotation = 0;
        Debug.Log(rectPointeur.localEulerAngles.z);
        float rotAvecOffset = FixRotationMismatch(rectPointeur.localEulerAngles.z - ROTAT_OFFSET);
        Debug.Log(rotAvecOffset);
        if (
            rotAvecOffset >= sectionRouletteAToucher[0] &&
            rotAvecOffset <= sectionRouletteAToucher[1])
        {
            Debug.Log("Calibration reussie!");
        }
        else
        {
            Debug.Log("Calibration ratee...");
        }

        Invoke(nameof(FinInteracCalib), 2);
    }
    /// <summary>
    /// Termine l'interaction de calibration
    /// </summary>
    void FinInteracCalib()
    {
        GameManager.Instance.inCalibInterac = false;
        ControlesPersonnage.canMove = true;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Règle les problèmes de mismatch de rotation entre le background et le pointeur pour que les calculs de réussite de calibration soient cohérents
    /// </summary>
    /// <param name="rotation">La rotation à régler</param>
    /// <returns>La nouvelle rotation, adaptée pour le calcul de réussite de l'interaction de calibration</returns>
    float FixRotationMismatch(float rotation)
    {
        if (rotation < 0) return 360 + rotation;
        else if (rotation > 360) return rotation - 360;
        else return rotation;
    }
}
