using System.Collections;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        while (LanguageManager.Instance == null)
            yield return null;

        LanguageManager.Instance.OnLanguageChanged += UpdateText;

        UpdateText(); // sync immédiate
    }

    private void OnDestroy()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (LanguageManager.Instance == null || text == null)
            return;

        text.text = LanguageManager.Instance.Get(key);
    }
}