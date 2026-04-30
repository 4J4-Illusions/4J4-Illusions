using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueBoucle : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public GameObject boutonNon;
    public GameObject boutonOuiPrefab;
    public Transform canvasParent;

    [Header("Dialogues")]
    [TextArea]
    public string[] dialogues;

    [Header("Spawn Oui")]
    public float spawnInterval = 0.8f;

    private int index = 0;
    private bool spawning = false;
    private Coroutine spawnRoutine;

    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = canvasParent.GetComponent<RectTransform>();

        dialogueText.text = dialogues[index];
    }

    public void OnClickNon()
    {
        // Avant-dernier dialogue → phase finale
        if (index == dialogues.Length - 2)
        {
            index++;
            dialogueText.text = dialogues[index];

            LancerPhaseFinale();
            return;
        }

        index++;
        if (index >= dialogues.Length)
            index = 0;

        dialogueText.text = dialogues[index];
    }

    void LancerPhaseFinale()
    {
        boutonNon.SetActive(false);

        spawning = true;
        spawnRoutine = StartCoroutine(SpawnOuiProgressif());
    }

    IEnumerator SpawnOuiProgressif()
    {
        while (spawning)
        {
            SpawnUnOui();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnUnOui()
    {
        GameObject clone = Instantiate(boutonOuiPrefab, canvasParent);

        RectTransform rt = clone.GetComponent<RectTransform>();

        Vector2 size = canvasRect.rect.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = Random.Range(-size.y / 2f, size.y / 2f);

        rt.anchoredPosition = new Vector2(x, y);

        // sécurité : évite empilement logique
        clone.transform.SetAsLastSibling();

        // bouton stop
        clone.GetComponent<Button>().onClick.AddListener(StopSpawning);
    }

    public void StopSpawning()
    {
        spawning = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        Debug.Log("Le joueur a cliqué sur OUI");
    }
}