using UnityEngine;
using LightType = UnityEngine.LightType;

public class RecompenseNiveau : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public float valeurRotation = 3f;
    public GameObject prefabParticuleRecompense;

    Vector3 rotation, taille;
    float tailleUnAxe;

    private void Awake()
    {
        rotation = new Vector3(
            Random.Range(-valeurRotation, valeurRotation),
            Random.Range(-valeurRotation, valeurRotation),
            Random.Range(-valeurRotation, valeurRotation));

        tailleUnAxe = Mathf.Max(.2f * transform.localScale.x, 1);
        taille = Vector3.one * tailleUnAxe;

        GameObject particule = Instantiate(prefabParticuleRecompense, transform.position, Quaternion.identity);
        particule.transform.SetParent(transform);
        GameObject lumiere = new("PointLightRecompense");
        lumiere.transform.position = transform.position;
        Light lightLum = lumiere.AddComponent<Light>();
        lightLum.type = LightType.Point;
        lightLum.range = transform.localScale.x;
        lightLum.intensity = transform.localScale.x / 10;
        lumiere.transform.SetParent(transform);

        particule.transform.localScale = lightLum.transform.localScale = taille;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation);
    }
}
