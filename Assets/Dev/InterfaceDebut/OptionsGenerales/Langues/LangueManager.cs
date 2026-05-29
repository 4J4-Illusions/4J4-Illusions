using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        French,
        Spanish,
        English
    }

    public Language currentLanguage = Language.French;

    public event Action OnLanguageChanged;

    private Dictionary<string, string> translations = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔥 IMPORTANT : on charge APRES que les UI existent
    private void Start()
    {
        LoadLanguage(currentLanguage);
    }

    public void LoadLanguage(Language lang)
    {
        currentLanguage = lang;

        string fileName = lang switch
        {
            Language.French => "fr",
            Language.Spanish => "es",
            Language.English => "en",
            _ => "fr"
        };

        TextAsset file = Resources.Load<TextAsset>("Languages/" + fileName);

        if (file == null)
        {
            Debug.LogError("Langue introuvable: " + fileName);
            return;
        }

        LocalizationData data = JsonUtility.FromJson<LocalizationData>(file.text);

        translations.Clear();

        foreach (var item in data.items)
        {
            translations[item.key] = item.value;
        }

        // 🔥 SAFE EVENT
        OnLanguageChanged?.Invoke();
    }

    public void SetLanguage(Language lang)
    {
        LoadLanguage(lang);
    }

    public string Get(string key)
    {
        if (translations.TryGetValue(key, out string value))
            return value;

        return $"[{key}]";
    }
}