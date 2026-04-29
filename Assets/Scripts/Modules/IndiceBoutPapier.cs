using UnityEngine;

public class IndiceBoutPapier : MonoBehaviour
{
    [Header("Accès pour autres scripts"), Space(30)]
    public float cooldownEnleverTexte = 0;

    // Update is called once per frame
    void Update()
    {
        if(cooldownEnleverTexte > 0)
        {
            cooldownEnleverTexte -= Time.deltaTime;
            cooldownEnleverTexte = Mathf.Max(cooldownEnleverTexte, 0);
        }
        else
        {
            //EnleverTexte(transform.Find("TexteFoule").gameObject);
            EnleverTexte(transform.parent.Find("TexteFoule").gameObject);
        }
    }



    /// <summary>
    /// Enlève le texte servant d'indice pour trouve run bout de papier après un certain temps
    /// </summary>
    /// <param name="texte">Le GameObject contenant le texte</param>
    public void EnleverTexte(GameObject texte)
    {
        texte.SetActive(false);
    }
}