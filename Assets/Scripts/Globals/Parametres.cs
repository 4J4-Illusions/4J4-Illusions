using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Parametres : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static Parametres Instance { get; private set; }

    [Header("Accès pour autres scripts"), Space]
    public Dictionary<string, string> dictParametres = new();

    // évènements
    public static Action<string, string> OnSettingsChange;

    string pathSave;

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
    private void Start()
    {
        // ajout valeurs par défaut
        UpdateParametres("Audio-General", "100");
        UpdateParametres("Audio-Jeu", "100");
        UpdateParametres("Audio-Musique", "100");
    }
    private void OnEnable()
    {
        //OnSettingsChange += SauvegarderParametres;
    }
    private void OnDisable()
    {
        //OnSettingsChange -= SauvegarderParametres;
    }



    /// <summary>
    /// Met à jour la valeur d'un paramètre dans le dictionnaire et déclenche l'évènement de changement de paramètre.
    /// </summary>
    /// <param name="kvp">La paire de clé-valeur du paramètre</param>
    public void UpdateParametres(string key, string value)
    {
        dictParametres[key] = value;
        //Debug.Log(dictParametres.Count);
        //Debug.Log(dictParametres[key]);
        OnSettingsChange.Invoke(key, value);
    }
    void SauvegarderParametres(string key, string value)
    {
        // IO pour sauvegarde
        pathSave = Path.Combine(Application.persistentDataPath, "settings.json");

        KvpWrapper wrapper = new KvpWrapper { key = key, value = value };
        Debug.Log(JsonUtility.ToJson(wrapper));

        File.WriteAllText(pathSave, JsonUtility.ToJson(wrapper));
        if (File.Exists(pathSave))
        {
            //Debug.Log($"fichier \'settings.json\' éxiste:\n{pathSave}");
            var fileData = JsonUtility.FromJson<KvpWrapper>(File.ReadAllText(pathSave));
            Debug.Log($"{fileData.key}, {fileData.value}");
        }
    }
}

[Serializable]
public class KvpWrapper
{
    public string key;
    public string value;
}