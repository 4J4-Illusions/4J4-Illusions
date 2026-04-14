using Globals;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static AudioManager Instance { get; private set; }

    [Header("Affectation inspecteur"), Space]
    public AudioClip[] clipsAmbience;
    public AudioClip[] clipsSFX;

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



    public CategorieSon GetClipCategory(AudioClip clip)
    {
        if (clipsAmbience.Contains(clip)) return CategorieSon.Ambience;
        return CategorieSon.SFX;
    }
    public float SetClipVolume(CategorieSon categSon)
    {
        float volumeCategorie = float.Parse(Parametres.Instance.dictParametres[(categSon == CategorieSon.Ambience) ? "Audio-Musique" : "Audio-Jeu"]) / 100;
        float volumeGeneral = float.Parse(Parametres.Instance.dictParametres["Audio-General"]) / 100;
        return volumeCategorie * volumeGeneral;
    }
    public float SetClipVolume(AudioClip clip)
    {
        CategorieSon categSon = GetClipCategory(clip);

        float volumeCategorie = float.Parse(Parametres.Instance.dictParametres[(categSon == CategorieSon.Ambience) ? "Audio-Musique" : "Audio-Jeu"]) / 100;
        float volumeGeneral = float.Parse(Parametres.Instance.dictParametres["Audio-General"]) / 100;
        return volumeCategorie * volumeGeneral;
    }
}
