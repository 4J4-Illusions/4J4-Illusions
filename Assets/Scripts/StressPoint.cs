using Globals;
using UnityEngine;

public class StressPoint : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    [Range(0, 100)] public float porteeStress = 50;
    public float[] intervalleValeursStressPourcent = new float[2] { 0, 1f };
    public TypeStress type;

    StressPointEntry valeurDict;
    bool inRange = false;

    private void Awake()
    {
        valeurDict.type = type;
        valeurDict.valeurStress = 0;
        GestionBarreAnxiete.collectionStressPoints.Add(GetInstanceID(), valeurDict);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log(distance);
        if (distance <= porteeStress)
        {
            float calculValeurStress = 0;
            if (type == TypeStress.Proportionnel)
            {
                calculValeurStress = Mathf.Clamp(
                    (1 - Mathf.Clamp(distance / porteeStress, 0, 1)) * intervalleValeursStressPourcent[1],
                    intervalleValeursStressPourcent[0],
                    intervalleValeursStressPourcent[1]);
                //Debug.Log(calculValeurStress);
            }
            else
            {
                if (!inRange)
                {
                    inRange = true;
                    calculValeurStress = intervalleValeursStressPourcent[1];
                }
                GestionBarreAnxiete.multiplierProgBarre = 0;
            }
            valeurDict.valeurStress = calculValeurStress / 100;
        }
        else
        {
            inRange = false;
            valeurDict.valeurStress = intervalleValeursStressPourcent[0];
            GestionBarreAnxiete.multiplierProgBarre = 1;
        }
        GestionBarreAnxiete.collectionStressPoints[GetInstanceID()] = valeurDict;
    }
}
