using Globals;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static AudioManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public List<AudioClip> clipsAmbience;
    public List<AudioClip> clipsSFX;

    // évènements
    public static Action OnAudioSettingsChange;

    float volumeGeneral = 1; // volume général
    float volumeJeu = 1; // volume sfx
    float volumeMusique = 1; // volume ambience
    List<AudioSource> listeAudsrcs;
    List<AudioClip> listeClips;
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
        DontDestroyOnLoad(gameObject);

        audsrc = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Parametres.OnSettingsChange += (KeyValuePair<string, string> kvp) => { if (kvp.Key.StartsWith("Audio-")) CalculVolumeFinal(kvp.Key[6..], kvp.Value); };
    }
    private void OnDisable()
    {
        Parametres.OnSettingsChange -= (KeyValuePair<string, string> kvp) => { if (kvp.Key.StartsWith("Audio-")) CalculVolumeFinal(kvp.Key[6..], kvp.Value); };
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
        float volumeCategorie = (categSon == CategorieSon.Ambience) ? volumeMusique : volumeJeu;
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

        float volumeCategorie = (categSon == CategorieSon.Ambience) ? volumeMusique : volumeJeu;
        return volumeCategorie * volumeGeneral;
    }
    /// <summary>
    /// Calcule le volume en fonction de la catégorie de volume (général, jeu ou musique), puis met à jour la variable de volume correspondante avec la valeur donnée.
    /// </summary>
    /// <param name="categVolume">La catégorie de volume à mettre à jour</param>
    /// <param name="valeur">La nouvelle valeur de volume</param>
    void CalculVolumeFinal(string categVolume, string valeur)
    {
        //Debug.Log(categVolume);
        //Debug.Log(valeur);
        float valeurConvertie = float.Parse(valeur) / 100;

        if (categVolume == "General") volumeGeneral = valeurConvertie;
        else if (categVolume == "Jeu") volumeJeu = valeurConvertie;
        else volumeMusique = valeurConvertie;
        //Debug.Log($"Valeurs volumes:    General-{volumeGeneral}    Jeu-{volumeJeu}    Musique-{volumeMusique}");
    }
    /// <summary>
    /// Joue un clip en fonction de sa catégorie. Si le clip est un son d'ambience, l'associe à un AudioSource dédié (en créant un nouveau si nécessaire) et le joue.
    /// </summary>
    /// <param name="categ">La catégorie du clip</param>
    /// <param name="clip">Le clip</param>
    /// <returns>Si le clip est un son d'ambience, retourne l'AudioSource associé, sinon retourne null</returns>
    public AudioSource JouerSon(CategorieSon categ, AudioClip clip)
    {
        Debug.Log("Jouer son");
        if (categ == CategorieSon.SFX) audsrc.PlayOneShot(clip);
        else
        {
            Debug.Log("Son d'ambience");

            // check si un AudioSource existe déjà pour ce clip d'ambience
            listeAudsrcs = GetComponents<AudioSource>().ToList();
            listeClips = listeAudsrcs.Select(aud => aud.clip).ToList();
            foreach (AudioSource source in listeAudsrcs)
            {
                if (source.clip == clip)
                {
                    source.Play();
                    return source;
                }
            }

            // si le clip n'a pas d'AudioSource associé, en créer un nouveau
            AudioSource nouvAudsrc = gameObject.AddComponent<AudioSource>();
            listeAudsrcs.Add(nouvAudsrc);
            nouvAudsrc.playOnAwake = false;
            nouvAudsrc.loop = true;
            nouvAudsrc.clip = clip;
            nouvAudsrc.volume = SetClipVolume(CategorieSon.Ambience);
            nouvAudsrc.Play();
            return nouvAudsrc;
        }
        return null;
    }
}
