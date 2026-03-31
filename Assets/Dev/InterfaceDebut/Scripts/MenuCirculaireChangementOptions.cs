using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCirculaireChangementOptions : MonoBehaviour
{
    [Header("Rotation (parent commun)")]
    public Transform radialRoot;

    [Header("Sous-menus")]
    public GameObject[] subMenus;

    [Header("Settings")]
    public float rotationStep = 40f;
    public float scrollCooldown = 0.15f;
    public float rotationSpeed = 10f;

    private int currentIndex;
    private float lastScrollTime = 0f;
    private float initialZRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialZRotation = radialRoot.eulerAngles.z;
        currentIndex = 4;
        targetRotation = radialRoot.rotation;
        UpdateSubMenus();
    }

    void Update()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Time.time - lastScrollTime >= scrollCooldown)
        {
            if (scroll > 0f)
            {
                currentIndex = (currentIndex - 1 + subMenus.Length) % subMenus.Length;
                ApplyChange();
            }
            else if (scroll < 0f)
            {
                currentIndex = (currentIndex + 1) % subMenus.Length;
                ApplyChange();
            }
        }

        radialRoot.rotation = Quaternion.Lerp(
            radialRoot.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    void ApplyChange()
    {
        UpdateRotation();
        UpdateSubMenus();
        lastScrollTime = Time.time;
    }

    void UpdateRotation()
    {
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