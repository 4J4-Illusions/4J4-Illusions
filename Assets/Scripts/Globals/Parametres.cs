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
    public Dictionary<string, object> dictParametres = new();

    public enum Langue
    {
        Francais,
        Anglais,
        Espagnol
    }

    // évènements
    public static Action<string, object> OnSettingsChange;

    string pathSave;
    SettingsData parametresDonnees = new();

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

        pathSave = Path.Combine(Application.persistentDataPath, "settings.json");
    }
    private void Start()
    {
        // ajout valeurs par défaut
        UpdateParametres("Audio_General", "100");
        UpdateParametres("Audio_Jeu", "100");
        UpdateParametres("Audio_Musique", "100");
    }



    /// <summary>
    /// Met à jour la valeur d'un paramètre dans le dictionnaire et déclenche l'évènement de changement de paramètre.
    /// </summary>
    /// <param name="kvp">La paire de clé-valeur du paramètre</param>
    public void UpdateParametres(string key, object value)
    {
        dictParametres[key] = value;
        parametresDonnees[key] = value;
        //Debug.Log(dictParametres.Count);
        //Debug.Log(dictParametres[key]);

        SauvegarderParametres();
        OnSettingsChange.Invoke(key, value);
    }
    void SauvegarderParametres()
    {
        // IO pour sauvegarde
        File.WriteAllText(pathSave, JsonUtility.ToJson(parametresDonnees));

        if (File.Exists(pathSave))
        {
            Debug.Log($"fichier \'settings.json\' éxiste:\n{pathSave}");
            //var fileData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(pathSave));
            //var fileData = JsonUtility.FromJson<Dictionary<string, object>>(File.ReadAllText(pathSave));
            //var fileData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(pathSave));
            //Debug.Log(fileData);
            //Debug.Log($"{fileData.Audio_General}, {fileData.Audio_Jeu}, {fileData.Audio_Musique}, ");
        }
    }
}

[Serializable]
public class SettingsData
{
    public int Audio_General;
    public int Audio_Jeu;
    public int Audio_Musique;
    public int Langue_Langue;
    public int Graphisme_General;
    public int Graphisme_Ombres;
    public string Graphisme_Resolution;

    // Source - https://stackoverflow.com/a/55495158
    // Posted by Christian Gollhardt, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-05-11, License - CC BY-SA 4.0
    public object this[string key]
    {
        get
        {
            var field = GetType().GetField(key);
            return field.GetValue(this);
        }
        set
        {
            var field = GetType().GetField(key);
            //field.SetValue(this, value);
            field.SetValue(this, Convert.ChangeType(value, field.FieldType));

            // adaptation selon stackoverflow
            // Source - https://stackoverflow.com/a/1089130
            // Posted by LBushkin, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-05-11, License - CC BY-SA 3.0
            //field.SetValue(this, Convert.ChangeType(value, field.FieldType), null);
        }
    }
}