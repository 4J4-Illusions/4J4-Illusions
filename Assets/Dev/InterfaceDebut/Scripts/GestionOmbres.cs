using UnityEngine;

public class GestionOmbres : MonoBehaviour
{
    public ChangementOptionsAvecBoutons optionOmbres;

    public void AppliquerOmbres()
    {
        switch (optionOmbres.Index)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable;
                break;

            case 1:
                QualitySettings.shadows = ShadowQuality.HardOnly;
                break;

            case 2:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution =
                    ShadowResolution.Medium;
                break;

            case 3:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowResolution =
                    ShadowResolution.VeryHigh;
                break;
        }
    }
}