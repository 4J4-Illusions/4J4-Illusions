using UnityEngine;

public class MonstreForet : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public float vitesseMonstre = .1f;
    public float dureeDeplacement = 5;

    bool canMove = false;
    Vector3 dernierePosOnde;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Debug.Log("can move");
            //AllerVersJoueur(dernierePosOnde);
            transform.position = Vector3.MoveTowards(transform.position, dernierePosOnde, vitesseMonstre);
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



    void PermettreBouger(Vector3 targetPos)
    {
        canMove = true;
        dernierePosOnde = targetPos;
        Debug.Log(dernierePosOnde);
        Debug.Log(transform.position);
        Invoke(nameof(ArreterBouger), dureeDeplacement);
    }
    void ArreterBouger()
    {
        canMove = false;
    }
    //void AllerVersJoueur(Vector3 targetPos)
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, targetPos, vitesseMonstre);
    //}
}
