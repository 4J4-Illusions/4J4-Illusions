using UnityEngine;

public class SableSuitJoueur : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public Vector3 positionAjustement;

    // Update is called once per frame
    void Update()
    {
        transform.position = GameManager.Instance.player.transform.position + positionAjustement;
    }
}
