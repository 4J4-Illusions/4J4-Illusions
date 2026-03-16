using UnityEngine;

public class CalibRoulette : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject background;
    public GameObject pointeur;

    float rotatOffset = 200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        background.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
}
