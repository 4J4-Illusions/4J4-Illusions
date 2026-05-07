using System;
using System.Collections.Generic;
using UnityEngine;

public class Parametres : MonoBehaviour
{
    // r�f�rence statique pour acc�der aux propri�t�ts du singleton
    public static Parametres Instance { get; private set; }

    [Header("Acc�s pour autres scripts"), Space]
    public Dictionary<string, string> dictParametres = new();

    // �v�nements
    public static Action<KeyValuePair<string, string>> OnSettingsChange;

    void Awake()
    {
        /*
         * setup du singleton
         * trouv� sur ce lien:
         * https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        */
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateParametres(new("Audio-General", "100"));
        UpdateParametres(new("Audio-Jeu", "100"));
        UpdateParametres(new("Audio-Musique", "100"));
    }



    /// <summary>
    /// Met � jour la valeur d'un param�tre dans le dictionnaire et d�clenche l'�v�nement de changement de param�tre.
    /// </summary>
    /// <param name="kvp">La paire de cl�-valeur du param�tre</param>
    public void UpdateParametres(KeyValuePair<string, string> kvp)
    {
        dictParametres[kvp.Key] = kvp.Value;
        //OnSettingsChange.Invoke(kvp);
    }
}
