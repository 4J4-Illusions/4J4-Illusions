using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : MonoBehaviour
{
    [Header("Images par langue")]
    public Sprite frenchSprite;
    public Sprite englishSprite;
    public Sprite spanishSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged += UpdateImage;
            UpdateImage();
        }
    }

    private void OnDestroy()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged -= UpdateImage;
    }

    public void UpdateImage()
    {
        switch (LanguageManager.Instance.currentLanguage)
        {
            case LanguageManager.Language.French:
                image.sprite = frenchSprite;
                break;

            case LanguageManager.Language.English:
                image.sprite = englishSprite;
                break;

            case LanguageManager.Language.Spanish:
                image.sprite = spanishSprite;
                break;
        }
    }
}