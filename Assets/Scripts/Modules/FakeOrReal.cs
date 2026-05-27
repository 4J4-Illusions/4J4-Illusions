using UnityEngine;

public class FakeOrReal : MonoBehaviour
{
    [Header("Accès pour autres scripts"), Space(30)]
    public float distance;

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
        //Debug.Log(distance, gameObject);
    }
}
