using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeVignette : MonoBehaviour
{
    [Header("Accès publique pour autres scripts"), Space]
    public float intensite = .5f;

    VolumeProfile profile;
    Vignette vignette;

    private void Awake()
    {
        profile = GetComponent<Volume>().profile;

        if (!profile.TryGet<Vignette>(out vignette))
        {
            vignette = profile.Add<Vignette>();
        }
        else
        {
            vignette = (Vignette)profile.components[0];
        }

        vignette.color = new ColorParameter(Color.red, true);
    }
    // Update is called once per frame
    void Update()
    {
        if(vignette != null)
        {
            vignette.intensity = new ClampedFloatParameter(intensite, 0, 1, true);
        }
    }
}
