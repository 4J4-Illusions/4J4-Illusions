using UnityEngine;
using TMPro;

public class ChangementOptionsAvecBoutonsLangue : MonoBehaviour
{
    public TMP_Text texteOption;

    [Header("Clés de traduction")]
    public string[] optionKeys;

    private int index = 0;

    void Start()
    {
        UpdateText();
    }

    public void Suivant()
    {
        index++;

        if (index >= optionKeys.Length)
            index = 0;

        UpdateText();
    }

    public void Precedent()
    {
        index--;

        if (index < 0)
            index = optionKeys.Length - 1;

        UpdateText();
    }

    private void UpdateText()
    {
        texteOption.text = LanguageManager.Instance.Get(optionKeys[index]);
    }

    // 🔥 IMPORTANT : quand la langue change
    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged += UpdateText;
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged -= UpdateText;
    }
}