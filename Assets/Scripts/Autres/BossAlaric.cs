using UnityEngine;
using UnityEngine.AI;

public class BossAlaric : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    AudioSource audsrc;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audsrc = GetComponent<AudioSource>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        SuivreJoueur();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Jumpscare(audsrc);
        }
    }



    void SuivreJoueur()
    {
        agent.destination = player.transform.position;
    }
}
