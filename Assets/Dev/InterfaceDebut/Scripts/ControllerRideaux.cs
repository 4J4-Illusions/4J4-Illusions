using UnityEngine;
using System.Collections;

public class ControllerRideaux : MonoBehaviour
{
    public Animator RideauDroite;
    public Animator RideauGauche;

    [Header("Ouvrir au demarrage")]
    public bool openOnStart = true;

    void Start()
    {
        if (openOnStart)
        {
            StartCoroutine(OuvrirAuStart());
        }
    }

    IEnumerator OuvrirAuStart()
    {
        yield return null;
        OuvrirRideaux();
    }

    public void OuvrirRideaux()
    {
        Debug.Log("OUVERTURE RIDEAUX");

        if (RideauDroite == null || RideauGauche == null)
        {
            Debug.LogError("Rideaux non assignes");
            return;
        }

        RideauDroite.ResetTrigger("Fermer");
        RideauGauche.ResetTrigger("Fermer");

        RideauDroite.ResetTrigger("Ouvrir");
        RideauGauche.ResetTrigger("Ouvrir");

        RideauDroite.SetTrigger("Ouvrir");
        RideauGauche.SetTrigger("Ouvrir");
    }

    public void FermerRideaux()
    {
        Debug.Log("FERMETURE RIDEAUX");

        if (RideauDroite == null || RideauGauche == null)
        {
            Debug.LogError("Rideaux non assignes");
            return;
        }

        RideauDroite.ResetTrigger("Ouvrir");
        RideauGauche.ResetTrigger("Ouvrir");

        RideauDroite.ResetTrigger("Fermer");
        RideauGauche.ResetTrigger("Fermer");

        RideauDroite.SetTrigger("Fermer");
        RideauGauche.SetTrigger("Fermer");
    }
}