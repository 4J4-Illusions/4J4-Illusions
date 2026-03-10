using UnityEngine;

public class ControllerRideaux : MonoBehaviour
{
    public Animator RideauDroite;
    public Animator RideauGauche;

    void Start()
    {
        OuvrirRideaux();
    }

    void OuvrirRideaux()
    {
        RideauDroite.SetTrigger("Ouvrir");
        RideauGauche.SetTrigger("Ouvrir");
    }

    public void FermerRideaux()
    {
        Debug.Log("Fermeture rideaux");
        RideauDroite.SetTrigger("Fermer");
        RideauGauche.SetTrigger("Fermer");
    }
}