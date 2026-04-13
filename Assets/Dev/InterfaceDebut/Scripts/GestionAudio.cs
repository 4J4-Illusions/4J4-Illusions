using UnityEngine;

public class GestionAudio : MonoBehaviour
{
    public AudioSource sourceAudio;
    public AudioClip sonClic;

    public void JouerSonClic()
    {
        sourceAudio.PlayOneShot(sonClic);
    }
}