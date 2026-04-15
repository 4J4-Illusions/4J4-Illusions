using Globals;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class GestionBarreAnxiete : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static GestionBarreAnxiete Instance;

    [Header("Affectation inspecteur"), Space]
    public GameObject conteneurBarre;
    public Animator animCoeur;
    public GameObject texteGameOver;

    [Header("Ajustement inspecteur"), Space]
    public bool modeProgBarre = false;
    [Range(0, 1)] public float progressionBarre = .001f;
    // gestions, trackage et acces pour autres scripts
    public static Dictionary<int, StressPointEntry> collectionStressPoints = new();
    public static float stressTotal;

    Image imgBarre;
    float vitesseAnimCoeur = 1, finalProgBarre;
    readonly Dictionary<int, StressPointEntry> instantEntriesToUpdate = new();
    bool pauseProgBarre = false;
    VolumeVignette vfxVignette;
    AudioSource audioSource;

    void Awake()
    {
        /*
         * setup du singleton
         * trouvé sur ce lien:
         * https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        */
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

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
            if (stressTotal < 1)
            {
                //Debug.Log("Increasing...");
                stressTotal += progressionBarre;
            }
        }
        else
        {
            float sommeStress = 0;
            pauseProgBarre = false;
            foreach (KeyValuePair<int, StressPointEntry> entry in collectionStressPoints)
            {
                sommeStress += entry.Value.valeurStress;
                if (entry.Value.pauseProgBarre) pauseProgBarre = true;
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
            stressTotal += (sommeStress > 0) ? sommeStress : finalProgBarre;
        }
        audioSource.volume = vfxVignette.intensite = imgBarre.fillAmount = stressTotal;
        vitesseAnimCoeur = 1 + stressTotal * 4;
        animCoeur.SetFloat("speedMultiplier", vitesseAnimCoeur);

        if (stressTotal >= 1)
        {
            Gameplay.Jumpscare();
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += CriseDePanique;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= CriseDePanique;
    }



    /// <summary>
    /// Éxecute la crise de panique, qui correspond à la fin du jeu, en remplissant complètement la barre d'anxiété et en désactivant le script pour arrêter toute mise à jour de la barre d'anxiété.
    /// </summary>
    void CriseDePanique()
    {
        imgBarre.fillAmount = 1;
        texteGameOver.SetActive(true);

        enabled = false;
    }
}
