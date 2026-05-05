using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    public TextMeshProUGUI text;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LanguageManager.Instance.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        text.text = LanguageManager.Instance.Get(key);
    }
}