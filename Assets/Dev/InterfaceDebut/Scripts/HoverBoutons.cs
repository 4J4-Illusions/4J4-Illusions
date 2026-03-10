using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverBoutonGlowX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animator du bouton")]
    public Animator animator;

    [Header("Glow derrière le bouton")]
    public Image glow;
    public float hoverDuration = 0.5f;   
    public float glowPulseAmount = 0.05f;
    public float glowPulseSpeed = 2f;
    public float xStretch = 0.5f;

    private Coroutine glowCoroutine;
    private bool hovering = false;
    private Vector3 glowOriginalScale;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (glow != null)
        {
            glowOriginalScale = glow.transform.localScale;
            Color c = glow.color;
            c.a = 0f;
            glow.color = c;
        }
    }

    void Update()
    {
        if (!hovering || glow == null) return;

        // Pulsation du glow
        float scaleX = 1f + glowPulseAmount * 0.4f * Mathf.Sin(Time.time * glowPulseSpeed);
        float scaleY = 1f + glowPulseAmount * Mathf.Sin(Time.time * glowPulseSpeed);
        Vector3 boutonScale = transform.localScale;

        glow.transform.localScale = new Vector3(
            glowOriginalScale.x * boutonScale.x * xStretch * scaleX,
            glowOriginalScale.y * boutonScale.y * scaleY,
            glowOriginalScale.z
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        if (animator != null) animator.SetBool("Hover", true);
        if (glow != null) StartGlowFade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        if (animator != null) animator.SetBool("Hover", false);
        if (glow != null)
        {
            StartGlowFade(0f);
            glow.transform.localScale = glowOriginalScale; // reset
        }
    }

    private void StartGlowFade(float targetAlpha)
    {
        if (glowCoroutine != null) StopCoroutine(glowCoroutine);
        glowCoroutine = StartCoroutine(FadeGlow(targetAlpha));
    }

    private System.Collections.IEnumerator FadeGlow(float targetAlpha)
    {
        float startAlpha = glow.color.a;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / hoverDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Color c = glow.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            glow.color = c;
            yield return null;
        }

        Color finalC = glow.color;
        finalC.a = targetAlpha;
        glow.color = finalC;
    }
}