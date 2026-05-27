using UnityEngine;
using UnityEngine.AI;

public class BossAlaric : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Ajustement inspecteur")]
    public float vitesse = 3.5f;

    [Header("Accès pour autres scripts"), Space(30)]
    public NavMeshAgent agent;

    AudioSource audsrc;
    float distance, ratioDistanceFinal;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = vitesse;
        audsrc = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.allowGameLoop)
        {
            agent.speed = vitesse;
        }
        else
        {
            agent.speed = 0;
        }
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



    /// <summary>
    /// Met à jour la destination de l'agent de navigation pour qu'il suive la position du joueur.
    /// </summary>
    void SuivreJoueur()
    {
        agent.destination = GameManager.Instance.player.transform.position;
    }
}
