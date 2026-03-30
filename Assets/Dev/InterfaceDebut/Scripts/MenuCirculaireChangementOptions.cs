using UnityEngine;

public class MenuCirculaireChangementOptions : MonoBehaviour
{
    [Header("Rotation (parent commun)")]
    public Transform radialRoot;

    [Header("Sous-menus")]
    public GameObject[] subMenus; // ordre du tableau = haut → bas sur la roue

    [Header("Settings")]
    public float rotationStep = 40f; // 360 / 9 options
    public float scrollCooldown = 0.15f;
    public float rotationSpeed = 10f;

    private int currentIndex; // index actif réel
    private float lastScrollTime = 0f;
    private float initialZRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Rotation initiale de la roue (Audio au centre)
        initialZRotation = radialRoot.eulerAngles.z;

        // Index de départ sur Audio
        currentIndex = 4; // Audio est au centre (index 4 dans ton tableau)

        // Initialiser la rotation cible pour éviter le saut
        targetRotation = radialRoot.rotation;

        // Afficher la bonne option
        UpdateSubMenus();
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Time.time - lastScrollTime >= scrollCooldown)
        {
            if (scroll > 0f) // scroll haut → roue tourne vers le bas → option au-dessus
            {
                currentIndex = (currentIndex - 1 + subMenus.Length) % subMenus.Length;
                ApplyChange();
            }
            else if (scroll < 0f) // scroll bas → roue tourne vers le haut → option en-dessous
            {
                currentIndex = (currentIndex + 1) % subMenus.Length;
                ApplyChange();
            }
        }

        // Rotation fluide vers l'angle cible
        radialRoot.rotation = Quaternion.Lerp(radialRoot.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void ApplyChange()
    {
        UpdateRotation();
        UpdateSubMenus();
        lastScrollTime = Time.time;
    }

    void UpdateRotation()
    {
        // Calculer rotation relative à Audio (index 4)
        float angle = initialZRotation - (currentIndex - 4) * rotationStep;
        targetRotation = Quaternion.Euler(0, 0, angle);
    }

    void UpdateSubMenus()
    {
        for (int i = 0; i < subMenus.Length; i++)
        {
            subMenus[i].SetActive(i == currentIndex);
        }
    }
}