using UnityEngine;

public class DialogueOnCollide : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    public string targetIdDialogue;
    [Header("Projet")]
    public AudioClip sonPorte;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sonPorte != null) AudioManager.Instance.JouerSon(Globals.CategorieSon.SFX, sonPorte);
            DialogueManager.Instance.ToggleOverlayDialogue(true, int.Parse(targetIdDialogue[^3..]) - 1);
            Destroy(gameObject);
        }
    }
}
