using UnityEngine;

public class Corde : MonoBehaviour
{
    [Header("Points de la corde")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Segment de la corde")]
    public GameObject segmentPrefab; // Pivot à la base du cylindre

    [Header("Réglages")]
    public int segmentCount = 15;
    public float sagAmount = 1.5f;

    private GameObject[] segments;
    //private GameObject[] flags;
    //public GameObject flagPrefab;
    //public int flagSpacing = 2;

    void Start()
    {
        segments = new GameObject[segmentCount];
        //flags = new GameObject[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            segments[i] = Instantiate(segmentPrefab, transform);

            /**
            if (i % flagSpacing == 0)
            {
                flags[i] = Instantiate(flagPrefab, transform);
            }
            **/
        }
    }

    void Update()
    {
        float segmentHeight = segmentPrefab.transform.localScale.y;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 pos;

            if (i == 0)
            {
                // Premier segment : base sur startPoint
                pos = startPoint.position;
            }
            else
            {
                // Base sur le sommet du segment précédent
                Vector3 prevTop = segments[i - 1].transform.position + segments[i - 1].transform.up * segmentHeight;
                pos = prevTop;
            }

            // Sag naturel
            float t = (float)i / (segmentCount - 1);
            pos.y -= Mathf.Sin(t * Mathf.PI) * sagAmount;

            segments[i].transform.position = pos;

            // Rotation du segment
            Vector3 dirRot;
            if (i < segmentCount - 1)
                dirRot = (segments[i + 1].transform.position - pos).normalized;
            else
                dirRot = (pos - segments[i - 1].transform.position).normalized;

            if (dirRot != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dirRot);
                targetRot *= Quaternion.Euler(90, 0, 0); // garder cylindre vertical
                segments[i].transform.rotation = targetRot;
            }

            /**
            // LOGIQUE DES DRAPEAUX (désactivée)
            if (flags[i] != null)
            {
                flags[i].transform.position = pos;

                if (i < segmentCount - 1)
                {
                    Vector3 dirFlag = (segments[i + 1].transform.position - pos).normalized;
                    if (dirFlag != Vector3.zero)
                        flags[i].transform.rotation = Quaternion.LookRotation(dirFlag);

                    flags[i].transform.Rotate(90, 0, 0); // pendage
                    float wave = Mathf.Sin(Time.time * 2f + i) * 10f; // vent
                    flags[i].transform.Rotate(wave, 0, 0);
                }
            }
            **/
        }
    }
}