using UnityEngine;
using Utils;

public class MonstreForet : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public float vitesseMonstre = .1f;
    public float incrementDureeDeplacement = 5;

    Vector3 dernierePosOnde;
    float dureeDeplacement = 0;

    // Update is called once per frame
    void Update()
    {
        if (dureeDeplacement > 0 && GameManager.Instance.allowGameLoop)
        {
            //Debug.Log("can move");
            AllerVersJoueur(dernierePosOnde);
            dureeDeplacement -= Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, dernierePosOnde, vitesseMonstre);
        }

        //if (GetComponent<StressPoint>().inRange)
        //{
        //    //Debug.Log("in range");
        //    Gameplay.Jumpscare();
        //    gameObject.SetActive(false);
        //    //enabled = false;
        //}
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
        dernierePosOnde = targetPos;
        //Debug.Log(dernierePosOnde);
        dureeDeplacement += incrementDureeDeplacement;
    }
    /// <summary>
    /// Méthode qui arrête le déplacement du monstre.
    /// </summary>
    void ArreterBouger()
    {
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
