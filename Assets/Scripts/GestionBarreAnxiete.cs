using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionBarreAnxiete : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public GameObject conteneurBarre;
    public Animator animCoeur;

    [Header("Ajustement inspecteur"), Space]
    public bool modeBarreProgression = false;
    [Range(0, 1)] public float progressionBarre = .001f;
    // gestions, trackage et acces pour autres scripts
    public static Dictionary<int, float> collectionStressPoints = new();

    Image imgBarre;
    float vitesseAnimCoeur = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgBarre = conteneurBarre.transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(modeBarreProgression)
        {
            if (imgBarre.fillAmount < 1)
            {
                //Debug.Log("Increasing...");
                imgBarre.fillAmount += progressionBarre;
            }
        }
        else
        {
            if(collectionStressPoints.Count > 0)
            {
                foreach (float stressValue in collectionStressPoints.Values)
                {
                    imgBarre.fillAmount += stressValue;
                }
            }
            else
            {
                imgBarre.fillAmount -= progressionBarre / 10;
            }
        }
        vitesseAnimCoeur = 1 + imgBarre.fillAmount * 4;
        animCoeur.SetFloat("speedMultiplier", vitesseAnimCoeur);
    }
}
