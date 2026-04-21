using UnityEngine;

public class GestionCanvas : MonoBehaviour
{
    public GameObject canvas;
    public ControllerRideaux rideaux;

    public void FermerCanvas()
    {
        canvas.SetActive(false);
    }

    public void OuvrirCanvas()
    {
        if(rideaux != null) rideaux.OuvrirRideaux();
    }
}