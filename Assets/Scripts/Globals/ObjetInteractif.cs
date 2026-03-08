using UnityEngine;
using Globals;

public class ObjetInteractif : MonoBehaviour
{
    public TypesInteraction typeInteraction = TypesInteraction.None;

    public void Interaction()
    {
        Debug.Log("Interaction avec " + transform.name);
        if (typeInteraction == TypesInteraction.None)
        {
            Destroy(gameObject);
        }
    }
}
