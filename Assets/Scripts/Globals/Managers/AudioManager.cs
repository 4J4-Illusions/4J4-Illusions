using Globals;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static AudioManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space(30)]
    public List<AudioClip> ambienceMenu;
    /// <summary>
    /// les numéros dans les noms de variables correspondent à l'étape du jeu à laquelle les clips sont associés selon l'enum <see cref="StageJeu"/>
    /// </summary>
    public List<AudioClip> ambienceIntro, ambienceDesert, ambienceForet, ambienceTheatre;

    public static Action OnAudioSettingsChange;

    float volumeGeneral = 1; // volume général
    float volumeJeu = 1; // volume sfx
    float volumeMusique = 1; // volume ambience (musique en arrière-plan)
    AudioClip[] clipsAmbience;
    readonly List<AudioSource> listeAudsrcs = new();
    AudioSource audsrc;
    Dictionary<AudioSource, float> sonsAmbienceAvecVolumeBase = new();
    List<float> sonsBase;

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
        clipsAmbience = new[] { ambienceMenu, ambienceIntro, ambienceDesert, ambienceForet, ambienceTheatre }.SelectMany(clip => clip).ToArray();
        sonsBase = Enumerable.Repeat(1f, clipsAmbience.Length).ToList();
    }
    private void Start()
    {
        foreach (AudioClip clip in clipsAmbience)
        {
            //JouerSon(CategorieSon.Ambience, clip);
            switch (GameManager.Instance.stageJeu)
            {
                case StageJeu.Menu:
                    if (ambienceMenu.Contains(clip)) JouerSon(CategorieSon.Ambience, clip);
                    break;
                case StageJeu.Intro:
                    if (ambienceIntro.Contains(clip)) JouerSon(CategorieSon.Ambience, clip);
                    break;
                case StageJeu.Desert:
                    if (ambienceDesert.Contains(clip)) JouerSon(CategorieSon.Ambience, clip);
                    break;
                case StageJeu.Foret:
                    if (ambienceForet.Contains(clip)) JouerSon(CategorieSon.Ambience, clip);
                    break;
                case StageJeu.Theatre:
                    if (ambienceTheatre.Contains(clip)) JouerSon(CategorieSon.Ambience, clip);
                    break;
            }
        }
    }
    private void OnEnable()
    {
        Parametres.OnSettingsChange += (string key, object value) => { if (key.StartsWith("Audio_")) CalculVolumeFinal(key[6..], value); };
    }
    private void OnDisable()
    {
        Parametres.OnSettingsChange -= (string key, object value) => { if (key.StartsWith("Audio_")) CalculVolumeFinal(key[6..], value); };
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
        return SetAudioVolume(categSon);
    }
    /// <summary>
    /// Donne une valeur de volume pour un clip audio d'ambience en fonction des paramètres de volume généraux et spécifiques à la catégorie, ainsi que d'une valeur de volume de base propre au clip (permettant de faire des réglages individuels pour chaque clip d'ambience).
    /// </summary>
    /// <param name="volumeBase">Volume de base</param>
    /// <returns>La valeur de volume</returns>
    public float SetAudioVolume(float volumeBase)
    {
        return volumeBase * SetAudioVolume(CategorieSon.Ambience);
    }
    /// <summary>
    /// Calcule le volume en fonction de la catégorie de volume (général, jeu ou musique), puis met à jour la variable de volume correspondante avec la valeur donnée.
    /// </summary>
    /// <param name="categVolume">La catégorie de volume à mettre à jour</param>
    /// <param name="valeur">La nouvelle valeur de volume</param>
    void CalculVolumeFinal(string categVolume, object valeur)
    {
        //Debug.Log(categVolume);
        //Debug.Log(valeur);
        float valeurConvertie = (int)valeur / 100f;

        if (categVolume == "General") volumeGeneral = valeurConvertie;
        else if (categVolume == "Jeu") volumeJeu = valeurConvertie;
        else volumeMusique = valeurConvertie;
        //Debug.Log($"Valeurs volumes:    General-{volumeGeneral}    Jeu-{volumeJeu}    Musique-{volumeMusique}");

        // met à jour dynamiquement les sons d'ambience
        //Debug.Log(listeAudsrcs.Count);
        foreach (AudioSource aud in listeAudsrcs)
        {
            //Debug.Log(aud.clip.name);
            aud.volume = volumeGeneral * volumeMusique * sonsBase[listeAudsrcs.IndexOf(aud)];
            aud.Play();
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
            if (refAudsrc != null)
            {
                audsrc.pitch = refAudsrc.pitch;
                audsrc.volume = refAudsrc.volume;
            }
            else
            {
                audsrc.volume = SetAudioVolume(CategorieSon.SFX);
            }
            audsrc.PlayOneShot(clip);
        }
        else
        {
            //Debug.Log("Son d'ambience");

            // check si un AudioSource existe déjà pour ce clip d'ambience
            //listeAudsrcs = GetComponents<AudioSource>().ToList();
            foreach (AudioSource source in listeAudsrcs)
            {
                if (source != null && source.clip == clip)
                {
                    if (refAudsrc != null)
                    {
                        sonsBase[listeAudsrcs.IndexOf(source)] = source.volume = refAudsrc.volume;
                    }
                    else
                    {
                        source.volume = SetAudioVolume(CategorieSon.Ambience);
                    }
                    source.Play();
                    return source;
                }
            }

            // si le clip n'a pas d'AudioSource associé, en créer un nouveau
            AudioSource nouvAudsrc = gameObject.AddComponent<AudioSource>();
            listeAudsrcs.Add(nouvAudsrc);
            sonsBase.Add(1);
            nouvAudsrc.loop = true;
            nouvAudsrc.clip = clip;
            if (refAudsrc != null)
            {
                sonsBase[^1] = nouvAudsrc.volume = refAudsrc.volume;
            }
            else
            {
                nouvAudsrc.volume = SetAudioVolume(CategorieSon.Ambience);
            }
            //nouvAudsrc.volume = (refAudsrc != null) ? refAudsrc.volume : SetAudioVolume(CategorieSon.Ambience);
            nouvAudsrc.Play();
            return nouvAudsrc;
        }
        return null;
    }
    public void Reinitialisation(StageJeu stageJeu)
    {

    }
}
