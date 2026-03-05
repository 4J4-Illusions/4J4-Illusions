using UnityEngine;

public class CameraJoueur : MonoBehaviour
{
    public GameObject joueur;
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
        transform.rotation = new Quaternion();
    }
}
