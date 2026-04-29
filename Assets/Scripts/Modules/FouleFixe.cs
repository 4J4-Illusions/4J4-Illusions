using UnityEngine;

public class FouleFixe : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public float vitesseRotation;

    Transform tete, rootTete;
    float dureeTempsProche, distance;
    bool enTrainFixer;
    StressPoint strpt;
    Animator anim;

    private void Awake()
    {
        tete = transform.Find("Foule_Female/Foule_Female/Body/Head");
        //Debug.Log(tete, tete);
        rootTete = tete.GetComponent<SkinnedMeshRenderer>().rootBone;
        //Debug.Log(rootTete, rootTete);
        strpt = GetComponent<StressPoint>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        /*
         * Test rotation vers joueur avec transform
         */
        distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log("distance: " + distance, this);

        if (distance <= 25)
        {
            dureeTempsProche += Time.deltaTime;
            //Debug.Log("durée temps proche: " + dureeTempsProche, this);
        }
        else
        {
            dureeTempsProche -= Time.deltaTime;
        }
        dureeTempsProche = Mathf.Clamp(dureeTempsProche, 0, 5);

        enTrainFixer = (dureeTempsProche >= 5);
        strpt.intervalleValeursStressPourcent[1] = (enTrainFixer) ? .02f : .01f;
    }
    private void LateUpdate()
    {
        if (dureeTempsProche >= 5)
        {
            FixerJoueur();
        }
        else
        {
        }
    }



    void FixerJoueur()
    {
        Debug.Log("regarde joueur");
        Vector3 lookAtPos = GameManager.Instance.player.transform.position;
        lookAtPos.y = 0;
        //rootTete.transform.LookAt(lookAtPos);
        //transform.LookAt(lookAtPos);
        Quaternion lookAtRot = Quaternion.LookRotation(GameManager.Instance.player.transform.position - transform.position);
        lookAtRot.x = lookAtRot.z = 0;

        /*
         * Tourner tout le corps
         */
        //transform.rotation = lookAtRot;

        /*
         * Tourner bone tete
         */
        //rootTete.rotation = lookAtRot;
        //Debug.Log(Quaternion.Slerp(rootTete.rotation, lookAtRot, vitesseRotation * Time.deltaTime).eulerAngles);
        rootTete.rotation = Quaternion.Slerp(rootTete.rotation, lookAtRot, vitesseRotation * Time.deltaTime);
        //rootTete.rotation = Quaternion.RotateTowards(rootTete.rotation, lookAtRot, vitesseRotation * Time.deltaTime);
    }
}
