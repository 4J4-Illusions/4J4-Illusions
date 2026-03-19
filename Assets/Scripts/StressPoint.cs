using UnityEngine;

public class StressPoint : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space]
    [Range(0, 100)] public float porteeStress = 50;
    public float[] intervalleValeursStress = new float[2] { .01f, .1f };

    private void Awake()
    {
        Debug.Log(GetInstanceID());
    }

    private void Start()
    {
        GestionBarreAnxiete.collectionStressPoints.Add(GetInstanceID(), 0);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log(distance);
        if (distance < porteeStress)
        {
            float calculValeurStress = Mathf.Clamp(
                (1 - Mathf.Clamp(distance / porteeStress, 0, 1)) * intervalleValeursStress[1], 
                intervalleValeursStress[0], 
                intervalleValeursStress[1]);
            //Debug.Log(calculValeurStress);
            GestionBarreAnxiete.collectionStressPoints[GetInstanceID()] = calculValeurStress;
        }
        else
        {
            GestionBarreAnxiete.collectionStressPoints[GetInstanceID()] = 0;
        }
    }
}
