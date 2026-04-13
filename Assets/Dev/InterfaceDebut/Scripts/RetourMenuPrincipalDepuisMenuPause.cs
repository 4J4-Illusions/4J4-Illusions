using UnityEngine;
using UnityEngine.InputSystem;

public class RetourMenuPrincipalDepuisMenuPause : MonoBehaviour
{
    [Header("Canvas menu principal")]
    public GameObject canvasPrincipal;

    [Header("Canvas parametres (ou autre menu)")]
    public GameObject canvasSecondaire;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (canvasSecondaire != null && canvasSecondaire.activeInHierarchy)
            {
                RetourMenuPrincipal();
            }
        }
    }

    void RetourMenuPrincipal()
    {
        Debug.Log("ESC -> RETOUR MENU PRINCIPAL");

        if (canvasSecondaire != null)
            canvasSecondaire.SetActive(false);

        if (canvasPrincipal != null)
            canvasPrincipal.SetActive(true);
    }
}