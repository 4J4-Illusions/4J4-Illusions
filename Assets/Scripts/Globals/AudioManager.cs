using Globals;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static AudioManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public AudioClip[] clipsAmbience;
    public AudioClip[] clipsSFX;
    [Header("Accès pour autres scripts"), Space]
    [Range(0, 1)] public float volumeGeneral;
    [Range(0, 1)] public float volumeJeu, volumeMusique;

    // évènements
    public static Action OnAudioSettingsChange;

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
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Parametres.OnSettingsChange += (KeyValuePair<string, string> kvp) => { if (kvp.Key.StartsWith("Audio-")) Debug.Log("update audio"); };
    }



    /// <summary>
    /// Récupère la catégorie d'un clip audio (Ambience ou SFX) en fonction de sa présence dans les listes de clips.
    /// </summary>
    /// <param name="clip">Le clip à vérifier</param>
    /// <returns>La catégorie du clip</returns>
    public CategorieSon GetClipCategory(AudioClip clip)
    {
        if (clipsAmbience.Contains(clip)) return CategorieSon.Ambience;
        return CategorieSon.SFX;
    }
    /// <summary>
    /// Donne une valeur de volume pour un clip audio en fonction de sa catégorie et des paramètres de volume généraux et spécifiques à la catégorie.
    /// </summary>
    /// <param name="categSon">La catégorie du son</param>
    /// <returns>La valeur de volume</returns>
    public float SetClipVolume(CategorieSon categSon)
    {
        float volumeCategorie = float.Parse(Parametres.Instance.dictParametres[(categSon == CategorieSon.Ambience) ? "Audio-Musique" : "Audio-Jeu"]) / 100;
        float volumeGeneral = float.Parse(Parametres.Instance.dictParametres["Audio-General"]) / 100;
        return volumeCategorie * volumeGeneral;
    }
    /// <summary>
    /// Donne une valeur de volume pour un clip audio en fonction de sa catégorie (déterminée à partir du clip lui-même) et des paramètres de volume généraux et spécifiques à la catégorie.
    /// </summary>
    /// <param name="clip">Le clip dont la catégorie sera determinée</param>
    /// <returns>La valeur de volume</returns>
    public float SetClipVolume(AudioClip clip)
    {
        CategorieSon categSon = GetClipCategory(clip);

        float volumeCategorie = float.Parse(Parametres.Instance.dictParametres[(categSon == CategorieSon.Ambience) ? "Audio-Musique" : "Audio-Jeu"]) / 100;
        float volumeGeneral = float.Parse(Parametres.Instance.dictParametres["Audio-General"]) / 100;
        return volumeCategorie * volumeGeneral;
    }
}
