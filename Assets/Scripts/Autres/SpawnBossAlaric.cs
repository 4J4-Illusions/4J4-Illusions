using UnityEngine;

public class SpawnBossAlaric : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject boss;

    AudioSource audsrc;

    private void Awake()
    {
        audsrc = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            AudioManager.Instance.JouerSon(Globals.CategorieSon.Ambience, audsrc.clip, audsrc);
            Destroy(gameObject);
        }
    }
}
