using UnityEngine;
using TMPro;

public class ChangementOptionsAvecBoutonsLangue : MonoBehaviour
{
    public TMP_Text texteOption;

    [Header("Clés de traduction (UI texte)")]
    public string[] optionKeys;

    [Header("Langues associées (IMPORTANT)")]
    public LanguageManager.Language[] languages;

    private int index = 0;

    private void Start()
    {
        if (texteOption == null)
            texteOption = GetComponent<TMP_Text>();

        UpdateText();
    }

    public void Suivant()
    {
        if (optionKeys == null || optionKeys.Length == 0)
            return;

        index++;

        if (index >= optionKeys.Length)
            index = 0;

        ApplyChange();
    }

    public void Precedent()
    {
        if (optionKeys == null || optionKeys.Length == 0)
            return;

        index--;

        if (index < 0)
            index = optionKeys.Length - 1;

        ApplyChange();
    }

    private void ApplyChange()
    {
        UpdateText();

        // 🔥 CHANGE LA LANGUE RÉELLE ICI
        if (languages != null && languages.Length > index)
        {
            Debug.Log($"[LANG BUTTON] Changement langue → {languages[index]}");

            LanguageManager.Instance.LoadLanguage(languages[index]);
        }
        else
        {
            Debug.LogWarning("[LANG BUTTON] Tableau languages non configuré correctement");
        }
    }

    private void UpdateText()
    {
        if (LanguageManager.Instance == null)
        {
            Debug.LogWarning("LanguageManager NULL");
            return;
        }

        if (optionKeys == null || optionKeys.Length == 0)
        {
            Debug.LogWarning("optionKeys vide");
            return;
        }

        Debug.Log($"[UI LANG] Key = {optionKeys[index]}");

        texteOption.text = LanguageManager.Instance.Get(optionKeys[index]);
    }
}