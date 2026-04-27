using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SpawnerFoulePassante : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public GameObject prefabFoulePassante;
    public GameObject foule;
    public List<SplineContainer> cheminsFoulePassante;

    float cooldownEntreFoulePassante = 5;

    private void Awake()
    {
        foreach (GameObject container in GameObject.FindGameObjectsWithTag("CheminFoulePassante"))
        {
            cheminsFoulePassante.Add(container.GetComponent<SplineContainer>());
        }

        for (int i = 0; i < cheminsFoulePassante.Count; i++)
        {
            GameObject instanceFoulePassante = Instantiate(prefabFoulePassante);
            instanceFoulePassante.transform.SetParent(foule.transform.Find("FoulePassante"));
            instanceFoulePassante.GetComponent<SplineAnimate>().Container = cheminsFoulePassante[i];
            instanceFoulePassante.GetComponent<SplineAnimate>().Play();
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
                cooldownEntreFoulePassante = Random.Range(5f, 10f);
                CreerFoulePassanteSurCheminRandom();
            }
        }
    }



    void CreerFoulePassanteSurCheminRandom()
    {
        int indexCheminRandom = Random.Range(0, cheminsFoulePassante.Count);

        GameObject instanceFoulePassante = Instantiate(prefabFoulePassante);
        instanceFoulePassante.transform.SetParent(foule.transform.Find("FoulePassante"));
        instanceFoulePassante.GetComponent<SplineAnimate>().Container = cheminsFoulePassante[indexCheminRandom];
        instanceFoulePassante.GetComponent<SplineAnimate>().Play();
    }
}
