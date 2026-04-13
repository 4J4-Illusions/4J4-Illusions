using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LienSliderEtNombre : MonoBehaviour
{
    public Slider slider;

    public TMP_Text valeurSlider;     // texte principal
    public TMP_Text background;       // texte arrière (ombre)

    void Start()
    {
        UpdateValue(slider.value);
        slider.onValueChanged.AddListener(UpdateValue);
    }

    void UpdateValue(float value)
    {
        string texte = Mathf.RoundToInt(value * 100) + "%";

        valeurSlider.text = texte;
        background.text = texte;
    }
}