using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GestionNiveauxRejouables : MonoBehaviour
{
    public static GestionNiveauxRejouables Instance;
    public GestionCartesRejouables gestionCartes;

    public List<string> niveauxRejouables = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AjouterNiveauActuel()
    {
        string scene =
            SceneManager.GetActiveScene().name;

        if (!niveauxRejouables.Contains(scene))
        {
            niveauxRejouables.Add(scene);
            gestionCartes.CreerCarte(scene);
        }
    }
}