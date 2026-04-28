using UnityEngine;

public class FouleFixe : MonoBehaviour
{
    GameObject tete, rootTete;
    float dureeTempsProche, distance;

    private void Awake()
    {
        tete = transform.Find("Foule_Female/Foule_Female/Body/Head").gameObject;
        //Debug.Log(tete, tete);
        rootTete = tete.GetComponent<SkinnedMeshRenderer>().rootBone.gameObject;
        //Debug.Log(rootTete, rootTete);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        Debug.Log("distance: " + distance, this);

        if(distance <= 25)
        {
            dureeTempsProche += Time.deltaTime;
            Debug.Log("durée temps proche: " + dureeTempsProche, this);
        }
        else
        {
            dureeTempsProche = Mathf.Max(dureeTempsProche - Time.deltaTime, 0);
        }

        if(dureeTempsProche >= 5)
        {
            FixerJoueur();
        }
    }



    void FixerJoueur()
    {
        Debug.Log("regarde joueur");
        Vector3 lookAtPos = GameManager.Instance.player.transform.position;
        lookAtPos.y = 0;
        //rootTete.transform.LookAt(lookAtPos);
        //transform.LookAt(lookAtPos);
        Quaternion lookAtRot = Quaternion.LookRotation(transform.position - GameManager.Instance.player.transform.position);
        lookAtRot.x = lookAtRot.z = 0;
        transform.rotation = lookAtRot;
    }
}
