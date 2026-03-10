using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeVignettePulse : MonoBehaviour
{
    Volume volume;
    VolumeProfile profile;
    Vignette vignette;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        profile = volume.profile;

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
        if(volume != null)
        {
            volume.weight = Mathf.Sin(Time.realtimeSinceStartup);
        }
    }
}
