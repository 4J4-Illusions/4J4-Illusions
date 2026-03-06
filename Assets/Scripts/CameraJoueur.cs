using UnityEngine;

public class CameraJoueur : MonoBehaviour
{
    [Header("Objets Unity")]
    public GameObject joueur;

    [Space(10)]
    public Vector3 ajustementPos;
    public Vector3 ajustementRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = joueur.transform.position + ajustementPos;
        transform.rotation = joueur.transform.rotation;
    }
}
