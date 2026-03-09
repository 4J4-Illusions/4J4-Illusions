using UnityEngine;
using Globals;
using Utils;

public class ObjetInteractif : MonoBehaviour
{
    // valeur servant a identifier le type d'interaction
    public TypeInteraction typeInteraction = TypeInteraction.None;

    /// <summary>
    /// Appele la méthode générale pour gérer les interactions avec les objets interactifs.
    /// </summary>
    /// <param name="obj">Le GameObject en question</param>
    public void Interaction()
    {
        Debug.Log("Interaction avec " + transform.name);
        Gameplay.Interaction(typeInteraction, gameObject);
    }
}
