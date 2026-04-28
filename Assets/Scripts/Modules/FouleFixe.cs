using UnityEngine;

public class FouleFixe : MonoBehaviour
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
            EnleverTexte(transform.Find("TexteFoule").gameObject);
        }
    }



    public void EnleverTexte(GameObject texte)
    {
        texte.SetActive(false);
    }
}