using UnityEngine;
using Utils;

public class MonstreForet : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public float vitesseMonstre = .1f;
    public float dureeDeplacement = 5;

    bool canMove = false;
    Vector3 dernierePosOnde;

    private void Start()
    {
        Debug.Log(GetComponent<StressPoint>().inRange);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Debug.Log("can move");
            AllerVersJoueur(dernierePosOnde);
            //transform.position = Vector3.MoveTowards(transform.position, dernierePosOnde, vitesseMonstre);
        }

        if (GetComponent<StressPoint>().inRange)
        {
            Debug.Log("in range");
            Gameplay.Jumpscare();
            gameObject.SetActive(false);
            //enabled = false;
        }
    }

    private void OnEnable()
    {
        ControlesPersonnage.OnPlayerOnde += PermettreBouger;
    }
    private void OnDisable()
    {
        ControlesPersonnage.OnPlayerOnde -= PermettreBouger;
    }



    /// <summary>
    /// Permet au monstre de se déplacer vers la position de l'onde sonore pendant une durée déterminée avant de s'arrêter
    /// </summary>
    /// <param name="targetPos"></param>
    void PermettreBouger(Vector3 targetPos)
    {
        canMove = true;
        dernierePosOnde = targetPos;
        Debug.Log(dernierePosOnde);
        Debug.Log(transform.position);
        Invoke(nameof(ArreterBouger), dureeDeplacement);
    }
    /// <summary>
    /// Méthode qui arrête le déplacement du monstre.
    /// </summary>
    void ArreterBouger()
    {
        canMove = false;
    }
    /// <summary>
    /// Met à jour la position du monstre en direction de la position cible.
    /// </summary>
    /// <param name="targetPos"></param>
    void AllerVersJoueur(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, vitesseMonstre);
    }
}
