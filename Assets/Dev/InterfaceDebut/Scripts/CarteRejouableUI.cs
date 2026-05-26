using TMPro;
using UnityEngine;

public class CarteRejouableUI : MonoBehaviour
{
    public TMP_Text titre;
    public TMP_Text description;

    private string sceneName;

    public void Initialiser(string scene)
    {
        sceneName = scene;

        titre.text = scene;

        description.text =
            "Cliquez pour rejouer ce niveau";
    }

    public void CliquerCarte()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}