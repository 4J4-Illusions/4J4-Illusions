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
    public List<AudioClip> ambience0;
    /// <summary>
    /// les numéros dans les noms de variables correspondent à l'étape du jeu à laquelle les clips sont associés selon l'enum <see cref="StageJeu"/>
    /// </summary>
    public List<AudioClip>
        ambience1, ambience2, ambience3, ambience4;

    // évènements
    public static Action OnAudioSettingsChange;

    float volumeGeneral = 1; // volume général
    float volumeJeu = 1; // volume sfx
    float volumeMusique = 1; // volume ambience
    AudioClip[] clipsAmbience;
    readonly List<AudioSource> listeAudsrcs = new();
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
        clipsAmbience = new[] { ambience0, ambience1, ambience2, ambience3, ambience4 }.SelectMany(clip => clip).ToArray();
        foreach (AudioClip clip in clipsAmbience)
        {
            JouerSon(CategorieSon.Ambience, clip);
        }
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
    public float SetAudioVolume(CategorieSon categSon)
    {
        float volumeCategorie = (categSon == CategorieSon.Ambience) ? volumeMusique : volumeJeu;
        return volumeCategorie * volumeGeneral;
    }
    /// <summary>
    /// Donne une valeur de volume pour un clip audio en fonction de sa catégorie (déterminée à partir du clip lui-même) et des paramètres de volume généraux et spécifiques à la catégorie.
    /// </summary>
    /// <param name="clip">Le clip dont la catégorie sera determinée</param>
    /// <returns>La valeur de volume</returns>
    public float SetAudioVolume(AudioClip clip)
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

        // met à jour dynamiquement les sons d'ambience
        foreach (AudioSource aud in listeAudsrcs)
        {
            aud.volume = volumeGeneral * volumeMusique;
        }
    }
    /// <summary>
    /// Joue un clip en fonction de sa catégorie. Si le clip est un son d'ambience, l'associe à un AudioSource dédié (en créant un nouveau si nécessaire) et le joue.
    /// </summary>
    /// <param name="categ">La catégorie du clip</param>
    /// <param name="clip">Le clip</param>
    /// <param name="refAudsrc">Une référence à un <see cref="AudioSource"/> déjà éxistant pour copier ses valeurs</param>
    /// <returns>Si le clip est un son d'ambience, retourne l'AudioSource associé, sinon retourne null</returns>
    public AudioSource JouerSon(CategorieSon categ, AudioClip clip, AudioSource refAudsrc = null)
    {
        //Debug.Log("Jouer son");
        //Debug.Log(clip.name);
        if (categ == CategorieSon.SFX)
        {
            if (refAudsrc != null) audsrc.pitch = refAudsrc.pitch;
            audsrc.volume = SetAudioVolume(CategorieSon.Ambience);
            audsrc.PlayOneShot(clip);
        }
        else
        {
            //Debug.Log("Son d'ambience");

            // check si un AudioSource existe déjà pour ce clip d'ambience
            //listeAudsrcs = GetComponents<AudioSource>().ToList();
            listeClips = listeAudsrcs.Select(aud => (aud != null) ? aud.clip : null).ToList();
            foreach (AudioSource source in listeAudsrcs)
            {
                if (source != null && source.clip == clip)
                {
                    source.volume = (refAudsrc != null) ? refAudsrc.volume : SetAudioVolume(CategorieSon.Ambience);
                    source.Play();
                    return source;
                }
            }

            // si le clip n'a pas d'AudioSource associé, en créer un nouveau
            AudioSource nouvAudsrc = gameObject.AddComponent<AudioSource>();
            listeAudsrcs.Add(nouvAudsrc);
            nouvAudsrc.loop = true;
            nouvAudsrc.clip = clip;
            nouvAudsrc.volume = (refAudsrc != null) ? refAudsrc.volume : SetAudioVolume(CategorieSon.Ambience);
            nouvAudsrc.Play();
            return nouvAudsrc;
        }
        return null;
    }
}
