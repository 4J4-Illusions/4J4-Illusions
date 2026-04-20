using UnityEngine;
using LightType = UnityEngine.LightType;

public class RecompenseNiveau : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public float valeurRotation = 3f;
    public GameObject prefabParticuleRecompense;

    Vector3 rotation;

    private void Awake()
    {
        rotation = new Vector3(
            Random.Range(-valeurRotation, valeurRotation), 
            Random.Range(-valeurRotation, valeurRotation), 
            Random.Range(-valeurRotation, valeurRotation));

        GameObject particule = Instantiate(prefabParticuleRecompense, transform.position, Quaternion.identity);
        particule.transform.SetParent(transform);
        GameObject lumiere = new("PointLightRecompense");
        lumiere.transform.position = transform.position;
        lumiere.AddComponent<Light>().type = LightType.Point;
        lumiere.transform.SetParent(transform);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation);
    }
}
