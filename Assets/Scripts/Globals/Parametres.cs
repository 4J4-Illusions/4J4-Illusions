using System;
using System.Collections.Generic;
using UnityEngine;

public class Parametres : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static Parametres Instance { get; private set; }

    [Header("Accès pour autres scripts"), Space]
    public Dictionary<string, string> dictParametres = new();

    // évènements
    public static Action<KeyValuePair<string, string>> OnSettingsChange;

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

        dictParametres.Add("Audio-General", "100");
        dictParametres.Add("Audio-Jeu", "100");
        dictParametres.Add("Audio-Musique", "100");
    }



    void UpdateParametres(KeyValuePair<string, string> kvp)
    {
        dictParametres[kvp.Key] = kvp.Value;
        OnSettingsChange(kvp);
    }
}
