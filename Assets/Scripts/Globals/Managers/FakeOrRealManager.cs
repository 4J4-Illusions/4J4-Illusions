using UnityEngine;

public class FakeOrRealManager : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Hiérarchie")]
    public GameObject boss;

    float cooldown = 10;
    int rngFakeOrReal;
    AudioSource audsrc;

    private void Awake()
    {
        audsrc = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            rngFakeOrReal = Random.Range(1, 10);
            //rngFakeOrReal = Random.Range(7, 20);
            if (rngFakeOrReal > 7)
            {
                TeleporterAlaric();
            }
            else
            {
                JouerFauxSon();
            }
            cooldown = Random.Range(10, 15);
        }
        //cooldown = Mathf.Max(cooldown, 0);
    }



    /// <summary>
    /// Téléporte le boss Alaric à l'une des positions prédéfinies, en choisissant celui qui est le plus proche du joueur.
    /// Est aussi accompagné d'un son qui désoriente le joueur.
    /// </summary>
    void TeleporterAlaric()
    {
        JouerFauxSon();

        int indexClosestBossTp = -1;
        float dist = float.PositiveInfinity;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<FakeOrReal>().distance < dist) indexClosestBossTp = i;
        }

        boss.GetComponent<BossAlaric>().agent.Warp(transform.GetChild(indexClosestBossTp).position);
    }
    /// <summary>
    /// Joue un son de désorientation en changeant aléatoirement la vitesse de lecture du son.
    /// Peut rarement être joué tout en téléportant le boss, pour ajouter un élément de surprise et de confusion pour le joueur.
    /// </summary>
    void JouerFauxSon()
    {
        audsrc.pitch = Random.Range(1f, 2f);
        AudioManager.Instance.JouerSon(Globals.CategorieSon.SFX, audsrc.clip, audsrc);
    }
}
