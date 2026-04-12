using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressionBarre : MonoBehaviour
{
    //      Objets Unity
    [Header("Objets Unity")]
    public GameObject conteneur;

    //      Variables publiques ajustables dans l'inspecteur
    [Header("Variables publiques ajustables dans l'inspecteur"), Range(0f, 1f)]
    public float vitProgBarre = .1f;

    //      Variables de travail
    RectTransform rectBarre;
    float maxWidth;
    //  Constantes
    Vector2 DEFAULT_POS = new(0, -50);
    Vector3 DEFAULT_ROT = Vector3.zero;


    private void Awake()
    {
        //Debug.Log("Conteneur : " + conteneur.name);
        rectBarre = GetComponent<RectTransform>();
        //Debug.Log(rectBarre.sizeDelta);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxWidth = conteneur.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (rectBarre.rect.width < maxWidth)
        {
            rectBarre.sizeDelta = new Vector2(rectBarre.sizeDelta.x + vitProgBarre, 25);

            if (rectBarre.rect.width >= maxWidth * .25f)
            {
                float progressiveShake;
                progressiveShake = rectBarre.rect.width / (maxWidth * .5f);
                //Debug.Log("progressive shake value: " + progressiveShake);

                Tremblement(-progressiveShake, progressiveShake);
                Vibration(-progressiveShake * .5f, progressiveShake * .5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rectBarre.sizeDelta = new Vector2(0, 25);
        }
    }


    void Tremblement(float min = -1, float max = 1)
    {
        float randomX = Random.Range(min, max);
        float randomY = Random.Range(min, max);
        conteneur.GetComponent<RectTransform>().anchoredPosition = DEFAULT_POS + new Vector2(randomX, randomY);
    }

    void Vibration(float min = -1, float max = 1)
    {
        float randomR = Random.Range(min, max);
        conteneur.GetComponent<RectTransform>().rotation = Quaternion.Euler(DEFAULT_ROT + new Vector3(0, 0, randomR));
    }
}
