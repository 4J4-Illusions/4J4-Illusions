using UnityEngine;

public class FouleFixe : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public float vitesseRotation;

    float dureeTempsProche, distance;
    StressPoint strpt;
    Quaternion rotationParDefaut, lookAtRot;

    private void Awake()
    {
        rotationParDefaut = transform.rotation;
        rotationParDefaut.x = rotationParDefaut.z = 0;

        strpt = GetComponent<StressPoint>();
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log("distance: " + distance, this);

        if (distance <= 25)
        {
            dureeTempsProche += Time.deltaTime;
            //Debug.Log("durée temps proche: " + dureeTempsProche, this);
        }
        else
        {
            dureeTempsProche -= Time.deltaTime;
        }
        dureeTempsProche = Mathf.Clamp(dureeTempsProche, 0, 7.5f);

        if(dureeTempsProche >= 5)
        {
            FixerJoueur();
            strpt.intervalleValeursStressPourcent[1] = .02f;
        }
        else
        {
            RetourRotNomale();
            strpt.intervalleValeursStressPourcent[1] = .01f;
        }
    }



    /// <summary>
    /// CHange la rotation de l'objet pour qu'il regarde le joueur, en ne tournant que sur l'axe y et en utilisant une interpolation pour que la rotation soit fluide.
    /// </summary>
    void FixerJoueur()
    {
        //Debug.Log("regarde joueur");
        lookAtRot = Quaternion.LookRotation(GameManager.Instance.player.transform.position - transform.position);
        lookAtRot.x = lookAtRot.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRot, vitesseRotation * Time.deltaTime);
    }
    /// <summary>
    /// Remet la rotation de l'objet à sa rotation de départ, en utilisant une interpolation pour que la rotation soit fluide.
    /// </summary>
    void RetourRotNomale()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationParDefaut, vitesseRotation * Time.deltaTime);
    }
}
