using UnityEngine;
using Globals;

public class ObjetInteractif : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    // valeur servant a identifier le type d'interaction
    public TypeInteraction typeInterac = 0;

    /// <summary>
    /// Appele la méthode générale pour gérer les interactions avec les objets interactifs.
    /// </summary>
    public void Interaction()
    {
        //Debug.Log("Interaction avec " + transform.name);
        Gameplay.Interaction(typeInterac, gameObject);
    }
}
