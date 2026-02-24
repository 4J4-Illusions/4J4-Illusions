using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;

public class VolumeVignettePulse : MonoBehaviour
{
    public Volume volume;
    VolumeProfile profile;
    Vignette vignette;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume = GetComponent<Volume>();
        profile = volume.profile;

        if(!profile.TryGet<Vignette>(out vignette))
        {
            vignette = profile.Add<Vignette>();
        }
        else
        {
            vignette = (Vignette) profile.components[0];
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
