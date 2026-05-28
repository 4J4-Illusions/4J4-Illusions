using UnityEngine;

public class GestionQualite : MonoBehaviour
{
    public ChangementOptionsAvecBoutons optionQualite;

    public void AppliquerQualite()
    {
        QualitySettings.SetQualityLevel(optionQualite.Index);
    }
}