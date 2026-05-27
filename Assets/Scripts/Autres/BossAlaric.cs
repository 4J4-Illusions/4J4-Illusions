using UnityEngine;
using UnityEngine.AI;

public class BossAlaric : MonoBehaviour
{
    [Header("Accès pour autres scripts"), Space(30)]
    public NavMeshAgent agent;

    AudioSource audsrc;
    float distance, ratioDistanceFinal;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audsrc = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        SuivreJoueur();
        distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        ratioDistanceFinal = 1 - ((Mathf.Clamp(distance, 10, 110) - 10) / 100f);
        GestionBarreAnxiete.Instance.ModifierIndicateursStress(ratioDistanceFinal);
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
        agent.destination = GameManager.Instance.player.transform.position;
    }
}
