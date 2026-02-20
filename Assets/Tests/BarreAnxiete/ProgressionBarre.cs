using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressionBarre : MonoBehaviour
{
    public GameObject conteneur;
    [Range(0f, 1f)]
    public float vitProgBarre = .1f;

    RectTransform rectBarre;
    float maxWidth;

    // constantes
    Vector2 defaultPos = new(0, -50);
    Vector3 defaultRot = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conteneur = transform.parent.gameObject;
        Debug.Log("Conteneur : " + conteneur.name);
        rectBarre = GetComponent<RectTransform>();
        Debug.Log(rectBarre.sizeDelta);

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
        else
        {

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
        conteneur.GetComponent<RectTransform>().anchoredPosition = defaultPos + new Vector2(randomX, randomY);
    }

    void Vibration(float min = -1, float max = 1)
    {
        float randomR = Random.Range(min, max);
        conteneur.GetComponent<RectTransform>().rotation = Quaternion.Euler(defaultRot + new Vector3(0, 0, randomR));
    }
}
