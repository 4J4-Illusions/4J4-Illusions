using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SpawnerFoulePassante : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public GameObject[] prefabsFoulePassante = new GameObject[2];
    public GameObject foule;
    public List<SplineContainer> cheminsFoulePassante;

    float cooldownEntreFoulePassante = 5;
    SplineAnimate splanim;

    private void Awake()
    {
        foreach (GameObject container in GameObject.FindGameObjectsWithTag("CheminFoulePassante"))
        {
            cheminsFoulePassante.Add(container.GetComponent<SplineContainer>());
        }

        for (int i = 0; i < cheminsFoulePassante.Count; i++)
        {
            GameObject instanceFoulePassante = Instantiate(SelectionSexeMembreFoule());
            instanceFoulePassante.transform.SetParent(foule.transform.Find("FoulePassante"));
            splanim = instanceFoulePassante.GetComponent<SplineAnimate>();
            splanim.Container = cheminsFoulePassante[i];
            splanim.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (cheminsFoulePassante.Count > 0)
        {
            if (cooldownEntreFoulePassante > 0) cooldownEntreFoulePassante -= Time.deltaTime;
            else
            {
                cooldownEntreFoulePassante = Random.Range(3f, 7f);
                CreerFoulePassanteSurCheminRandom();
            }
        }
    }



    /// <summary>
    /// Crée une nouvelle instance de foule passante sur un chemin aléatoire
    /// </summary>
    void CreerFoulePassanteSurCheminRandom()
    {
        int indexCheminRandom = Random.Range(0, cheminsFoulePassante.Count);

        GameObject instanceFoulePassante = Instantiate(SelectionSexeMembreFoule());
        instanceFoulePassante.transform.SetParent(foule.transform.Find("FoulePassante"));
        splanim = instanceFoulePassante.GetComponent<SplineAnimate>();
        //splanim.Container = cheminsFoulePassante[i];
        splanim.Container = cheminsFoulePassante[indexCheminRandom];
        splanim.Duration = Random.Range(20f, 50f);
        splanim.Play();
    }
    /// <summary>
    /// Choix aléatoire du sexe du membre de la foule passante à créer
    /// </summary>
    /// <returns>Le prefab avec le sexe choisi aléatoirement</returns>
    GameObject SelectionSexeMembreFoule()
    {

        return prefabsFoulePassante[(int)Mathf.Round(Random.Range(0f, 1f))];
    }
}
