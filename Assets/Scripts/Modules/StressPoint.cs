using Globals;
using UnityEngine;

public class StressPoint : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    public float porteeStress = 50;
    public float[] intervalleValeursStressPourcent = new float[2] { 0, 1f };
    public TypeStress type;

    [Header("Accès pour autres scripts"), Space]
    public bool inRange = false;

    StressPointEntry valeurDict;

    private void Awake()
    {
        valeurDict.type = type;
        valeurDict.valeurStress = 0;
        valeurDict.pauseProgBarre = true;
        GestionBarreAnxiete.collectionStressPoints.Add(GetInstanceID(), valeurDict);
    }
    private void Update()
    {
        // récupère la distance entre le joueur et le point de stress
        float distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log(distance);
        if (distance <= porteeStress)
        {
            // si le joueur est dans la portée du stress, on calcule la valeur de stress en fonction du type de stress
            float calculValeurStress = 0;
            if (type == TypeStress.Proportionnel)
            {
                inRange = true;
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
            }
            valeurDict.valeurStress = calculValeurStress / 100;
        }
        else
        {
            inRange = false;
            valeurDict.valeurStress = intervalleValeursStressPourcent[0];
        }
        valeurDict.pauseProgBarre = inRange;
        //Debug.Log($"GameObject: {gameObject.name}    Dictionnary value: {valeurDict}");
        GestionBarreAnxiete.collectionStressPoints[GetInstanceID()] = valeurDict;
    }
    private void OnDisable()
    {
        GestionBarreAnxiete.collectionStressPoints.Remove(GetInstanceID());
    }
    private void OnDestroy()
    {
        GestionBarreAnxiete.collectionStressPoints.Remove(GetInstanceID());
    }
}
