using UnityEngine;
using UnityEngine.AI;

public class MonstreForet : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public float vitesseMonstreParSeconde = 1f;
    public float incrementDureeDeplacement = 5;

    float dureeDeplacement = 0, distance;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (dureeDeplacement > 0 && GameManager.Instance.allowGameLoop)
        {
            //Debug.Log("can move");
            AllerVersJoueur();
            dureeDeplacement -= Time.deltaTime;

            distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
            if (distance <= 2)
            {
                GameManager.Instance.Jumpscare();
            }
        }
        else
        {
            agent.speed = 0;
        }

        //if (GetComponent<StressPoint>().inRange)
        //{
        //    //Debug.Log("in range");
        //    Gameplay.Jumpscare();
        //    gameObject.SetActive(false);
        //    //enabled = false;
        //}
    }
    private void OnEnable()
    {
        ControlesPersonnage.OnPlayerOnde += PermettreBouger;
    }
    private void OnDisable()
    {
        ControlesPersonnage.OnPlayerOnde -= PermettreBouger;
    }



    /// <summary>
    /// Permet au monstre de se déplacer vers la position de l'onde sonore pendant une durée déterminée avant de s'arrêter
    /// </summary>
    /// <param name="targetPos"></param>
    void PermettreBouger()
    {
        dureeDeplacement += incrementDureeDeplacement;

        agent.speed = vitesseMonstreParSeconde;
    }
    /// <summary>
    /// Met à jour la position du monstre en direction de la position cible.
    /// </summary>
    /// <param name="targetPos"></param>
    void AllerVersJoueur()
    {
        agent.SetDestination(GameManager.Instance.player.transform.position);
    }
}
