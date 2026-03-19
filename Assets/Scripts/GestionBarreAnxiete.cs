using System;
using System.Collections.Generic;
using UnityEngine;

public class GestionBarreAnxiete : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public bool modeBarreProgression = false;
    [Range(0, 1)] public float progressionBarre = .1f;
    // gestions, trackage et acces pour autres scripts
    public static Dictionary<int, float> collectionStressPoints = new();

    RectTransform rectConteneur, rectBarre;
    float MAX_WIDTH;
    private void Awake()
    {
        rectConteneur = GetComponent<RectTransform>();
        //Debug.Log(rectConteneur.rect);
        //Debug.Log(rectConteneur.sizeDelta);
        MAX_WIDTH = rectConteneur.rect.width;
        //Debug.Log(MAX_WIDTH);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectBarre = transform.GetChild(0).GetComponent<RectTransform>();
        //Debug.Log(rectBarre.rect);
        //Debug.Log(rectBarre.sizeDelta);
    }

    // Update is called once per frame
    void Update()
    {
        if(modeBarreProgression)
        {
            if (rectBarre.rect.width < MAX_WIDTH)
            {
                //Debug.Log("Increasing...");
                rectBarre.sizeDelta += new Vector2(progressionBarre, 0);
                //rectBarre.sizeDelta = new Vector2(rectBarre.sizeDelta.x + (MAX_DIMENSIONS.x * progressionBarrePourcent), 0);
            }
        }
        else
        {
            foreach (float stressValue in collectionStressPoints.Values)
            {
                rectBarre.sizeDelta += new Vector2(stressValue, 0);
            }
        }
    }
}
