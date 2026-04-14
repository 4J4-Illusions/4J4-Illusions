using UnityEngine;

public class SonAmbiance : MonoBehaviour
{
    public AudioClip clip;

    private AudioSource source;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.volume = 0.4f;
        source.spatialBlend = 0f;
    }

    void Start()
    {
        Debug.Log("Ambiance start");

        source.Play();
    }
}