using Globals;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Parametres : MonoBehaviour
{
    // référence statique pour accéder aux propriététs du singleton
    public static Parametres Instance { get; private set; }

    [Header("Accès pour autres scripts"), Space(30)]
    public Dictionary<string, object> dictParametres = new();

    public enum Langue
    {
        Francais,
        Anglais,
        Espagnol
    }
    public enum Graphisme
    {
        UltraLow,
        Low,
        Medium,
        High,
        UltraHigh
    }
    public enum Ombres
    {
        None,
        Hard,
        Soft
    }

    // évènements
    public static Action<string, object> OnSettingsChange;

    string fileName = "settings.json", filePath;
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

        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }
    private void Start()
    {
        // ajout valeurs par défaut
        UpdateParametres("Audio_General", 100);
        UpdateParametres("Audio_Jeu", 100);
        UpdateParametres("Audio_Musique", 100);
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
        OnSettingsChange?.Invoke(key, value);
    }
    void SauvegarderParametres()
    {
        // IO pour sauvegarde
        FichierIO.Create(filePath, JsonUtility.ToJson(parametresDonnees));

        if (File.Exists(filePath))
        {
            //Debug.Log($"fichier \'settings.json\' éxiste:\n{filePath}");
            //var fileData = FichierIO.Read<SettingsData>(pathSave);
            //Debug.Log(fileData);
            //foreach (var item in fileData.GetType().GetFields())
            //{
            //    Debug.Log($"Nom champ: {item.Name}    Valeur: {item.GetValue(fileData)}");
            //}
        }
    }
}

/// <summary>
/// Encapsule les données des paramètres du jeu, avec des champs correspondant à chaque paramètre. Permet de sérialiser et désérialiser facilement les données de paramètres en JSON.
/// </summary>
[Serializable]
public class SettingsData
{
    public int Audio_General;
    public int Audio_Jeu;
    public int Audio_Musique;

    public Parametres.Langue Langue_Langue;

    public Parametres.Graphisme Graphisme_General;
    public Parametres.Ombres Graphisme_Ombres;
    public string Graphisme_Resolution;
    public int Graphisme_FpsCap;

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
            if (field != null)
            {
                field.SetValue(this, Convert.ChangeType(value, field.FieldType));
            }
            else
            {
                throw new ArgumentException("Field is undefined", nameof(key));
            }
        }
    }
}
/// <summary>
/// Représente les différentes résolutions d'affichage disponibles pour le jeu, avec des chaînes de caractères formatées en "largeurxhauteur".
/// </summary>
public static class DisplayResolutions
{
    public static string QuarterHighDefinition = "960x540";
    public static string HighDefinition = "1280x720";
    public static string HighDefinitionPlus = "1600x900";
    public static string FullHighDefinition = "1920x1080";
    public static string UltraWideFullHighDefinition = "2560x1080";
    public static string QuadHighDefinition = "2560x1440";
    public static string FourKUltraHighDefinition = "3840x2160";
    public static string EightKUltraHighDefinition = "7680x4320";
}