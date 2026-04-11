using Globals;
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
    public bool modeProgBarre = false;
    [Range(0, 1)] public float progressionBarre = .001f;
    // gestions, trackage et acces pour autres scripts
    public static Dictionary<int, StressPointEntry> collectionStressPoints = new();

    Image imgBarre;
    float vitesseAnimCoeur = 1, finalProgBarre;
    Dictionary<int, StressPointEntry> instantEntriesToUpdate = new();
    bool pauseProgBarre = false;
    VolumeVignette vfxVignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgBarre = conteneurBarre.transform.GetChild(0).GetComponent<Image>();
        vfxVignette = GameManager.Instance.player.GetComponentInChildren<VolumeVignette>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeProgBarre)
        {
            if (imgBarre.fillAmount < 1)
            {
                //Debug.Log("Increasing...");
                imgBarre.fillAmount += progressionBarre;
            }
        }
        else
        {
            float totalStress = 0;
            pauseProgBarre = false;
            foreach (KeyValuePair<int, StressPointEntry> entry in collectionStressPoints)
            {
                totalStress += entry.Value.valeurStress;
                if(entry.Value.pauseProgBarre) pauseProgBarre = true;
                if (entry.Value.type == TypeStress.Instant && entry.Value.valeurStress > 0)
                {
                    StressPointEntry updatedValue = entry.Value;
                    updatedValue.valeurStress = 0;
                    instantEntriesToUpdate.Add(entry.Key, updatedValue);
                }
            }
            foreach (KeyValuePair<int, StressPointEntry> instantEntry in instantEntriesToUpdate)
            {
                collectionStressPoints[instantEntry.Key] = instantEntry.Value;
            }
            instantEntriesToUpdate.Clear();

            finalProgBarre = (!pauseProgBarre) ? (-progressionBarre / 10) : 0;
            //if(finalProgBarre >= 0) Debug.Log(finalProgBarre);
            imgBarre.fillAmount += (totalStress > 0) ? totalStress : finalProgBarre;
            vfxVignette.intensite = imgBarre.fillAmount;
        }
        vitesseAnimCoeur = 1 + imgBarre.fillAmount * 4;
        animCoeur.SetFloat("speedMultiplier", vitesseAnimCoeur);
    }
}
