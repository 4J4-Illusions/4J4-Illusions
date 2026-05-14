using Globals;
using UnityEngine;

public class AudioManagerConnect : MonoBehaviour
{
    [Header("Accès pour autres scripts"), Space(30)]
    public AudioSource audsrc;

    private void Awake()
    {
        audsrc = GetComponent<AudioSource>();
        //audsrc = AudioManager.Instance.JouerSon(CategorieSon.Ambience, audsrc.clip, audsrc);
        //Destroy(GetComponent<AudioSource>());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audsrc = AudioManager.Instance.JouerSon(CategorieSon.Ambience, audsrc.clip, audsrc);
        Destroy(GetComponent<AudioSource>());
    }
}
