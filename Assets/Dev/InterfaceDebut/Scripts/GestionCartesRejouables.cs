using UnityEngine;

public class GestionCartesRejouables : MonoBehaviour
{
    public Transform content;
    public GameObject prefabCarte;

    public void CreerCarte(string nomScene)
    {
        GameObject carte =
            Instantiate(prefabCarte, content);
            carte.SetActive(true);

        CarteRejouableUI ui =
            carte.GetComponent<CarteRejouableUI>();

        ui.Initialiser(nomScene);
    }
}