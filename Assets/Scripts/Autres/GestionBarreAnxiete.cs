using Globals;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
    public float reductionStressSoulagement = .25f;
    [Range(0, 1)] public float progressionBarre = .001f;
    // gestions, trackage et acces pour autres scripts
    public static Dictionary<int, StressPointEntry> collectionStressPoints = new();
    public static float stressTotal;

    Image imgBarre;
    float vitesseAnimCoeur = 1, finalProgBarre;
    readonly Dictionary<int, StressPointEntry> instantEntriesToUpdate = new();
    bool pauseProgBarre = false;
    Volume volume;
    AudioSource audsrc;

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

        stressTotal = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imgBarre = conteneurBarre.transform.GetChild(0).GetComponent<Image>();
        volume = GameManager.Instance.cameraJoueur.GetComponent<Volume>();
        audsrc = GetComponent<AudioManagerConnect>().audsrc;
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
            //Debug.Log("somme stres: " + sommeStress);
            foreach (KeyValuePair<int, StressPointEntry> instantEntry in instantEntriesToUpdate)
            {
                collectionStressPoints[instantEntry.Key] = instantEntry.Value;
            }
            instantEntriesToUpdate.Clear();

            finalProgBarre = (!pauseProgBarre) ? (-progressionBarre / 10) : 0;
            //Debug.Log("final prog barre: " + finalProgBarre);
            if(GameManager.Instance.allowGameLoop) stressTotal += (sommeStress > 0) ? sommeStress : finalProgBarre;
            else stressTotal += 0;
            stressTotal = MathF.Min(MathF.Max(stressTotal, 0), 1);
            //Debug.Log("stress total: " + stressTotal);
        }
        volume.weight = imgBarre.fillAmount = stressTotal;
        audsrc.volume = stressTotal * AudioManager.Instance.SetAudioVolume(CategorieSon.Ambience);
        vitesseAnimCoeur = 1 + stressTotal * 4;
        animCoeur.SetFloat("speedMultiplier", vitesseAnimCoeur);

        if (stressTotal >= 1)
        {
            GameManager.Instance.Jumpscare();
        }
    }
    private void OnEnable()
    {
        GameManager.OnGameOver += CriseDePanique;
        GameManager.OnLevelProgress += Soulagement;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= CriseDePanique;
        GameManager.OnLevelProgress -= Soulagement;
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
    void Soulagement()
    {
        stressTotal -= reductionStressSoulagement;
    }
}
