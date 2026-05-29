using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LienSliderEtNombre : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public Slider slider;
    public TMP_Text valeurSlider;     // texte principal
    public TMP_Text background;       // texte arrière (ombre)

    void Start()
    {
        UpdateValue(slider.value);
        slider.onValueChanged.AddListener(UpdateValue);
    }



    /// <summary>
    /// Met à jour le texte du slider et sauvegarde la valeur dans le dictionnaire de paramètres
    /// </summary>
    /// <param name="value">Valeur du slider</param>
    void UpdateValue(float value)
    {
        // convertit la valeur du slider en pourcentage entier
        int valeurInt = Mathf.RoundToInt(value * 100);
        //Debug.Log(valeurInt);
        string valeurTexte = valeurInt + "%";
        //Debug.Log(valeurTexte);

        // met à jour les textes du slider
        valeurSlider.text = valeurTexte;
        background.text = valeurTexte;

        // sauvegarde dans le dictionnaire de paramètres
        string paramKeyAudio = "Audio_" + slider.name[12..]; // extrait la partie du nom du slider après "SliderAudio"
        //Debug.Log(paramKeyAudio);
        Parametres.Instance.UpdateParametres(paramKeyAudio, valeurInt);
        //Debug.Log($"Paramètre {paramKeyAudio} mis à jour: {valeurInt}");
        //Debug.Log($"Valeur dans le dictionnaire: {Parametres.Instance.dictParametres[paramKeyAudio]}");
    }
}