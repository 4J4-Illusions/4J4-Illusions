using UnityEngine;
using UnityEngine.Splines;

public class Foule : MonoBehaviour
{
    SplineAnimate splanim;

    private void Awake()
    {
        splanim = GetComponent<SplineAnimate>();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Normalized time: " + splanim.NormalizedTime);
        if(splanim.NormalizedTime == 1)
        {
            Destroy(gameObject);
        }
    }
}
