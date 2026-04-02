using UnityEngine;
using System.Collections.Generic;

public class MultipleCordes : MonoBehaviour
{
    [System.Serializable]
    public class CordeData
    {
        public Transform startPoint;
        public Transform endPoint;
        public GameObject segmentPrefab;
        public int segmentCount = 15;
        public float sagAmount = 1.5f;
    }

    [Header("Toutes les cordes")]
    public List<CordeData> cordes;

    void Start()
    {
        foreach (CordeData corde in cordes)
        {
            CreateCorde(corde);
        }
    }

    void CreateCorde(CordeData corde)
    {
        GameObject previousSegment = null;

        for (int i = 0; i < corde.segmentCount; i++)
        {
            GameObject segment = Instantiate(corde.segmentPrefab, transform);
            float t = (float)i / (corde.segmentCount - 1);

            // Position interpolée avec sag
            Vector3 pos = Vector3.Lerp(corde.startPoint.position, corde.endPoint.position, t);
            pos.y -= Mathf.Sin(t * Mathf.PI) * corde.sagAmount;
            segment.transform.position = pos;

            // Rigidbody
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (rb == null)
                rb = segment.AddComponent<Rigidbody>();

            rb.mass = 0.2f;
            rb.linearDamping = 0.1f;     // Remplace drag obsolète
            rb.angularDamping = 0.05f;   // Remplace angularDrag obsolète

            // HingeJoint
            HingeJoint joint = segment.AddComponent<HingeJoint>();
            joint.axis = Vector3.forward;
            joint.useLimits = false;

            if (i == 0)
            {
                if (!corde.startPoint.GetComponent<Rigidbody>())
                {
                    Rigidbody rbStart = corde.startPoint.gameObject.AddComponent<Rigidbody>();
                    rbStart.isKinematic = true;
                }
                joint.connectedBody = corde.startPoint.GetComponent<Rigidbody>();
            }
            else
            {
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            }

            previousSegment = segment;

            if (i == corde.segmentCount - 1)
            {
                if (!corde.endPoint.GetComponent<Rigidbody>())
                {
                    Rigidbody rbEnd = corde.endPoint.gameObject.AddComponent<Rigidbody>();
                    rbEnd.isKinematic = true;
                }
                joint.connectedBody = corde.endPoint.GetComponent<Rigidbody>();
            }

            /*
            // Fanions optionnels
            // if (flagPrefab != null && i % 3 == 0)
            // {
            //     GameObject flag = Instantiate(flagPrefab, segment.transform);
            //     flag.transform.localPosition = Vector3.up * 0.5f;
            // }
            */
        }
    }
}