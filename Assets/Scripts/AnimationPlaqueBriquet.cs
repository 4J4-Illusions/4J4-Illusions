using Globals;
using UnityEngine;
using Utils;

public class AnimationPlaqueBriquet : MonoBehaviour
{
    [Header("Ajustement inspecteur"), Space]
    public ParticleSystem particuleFeuBriquet;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Gameplay.OnInteraction += TriggerAnimationFeu;
    }
    private void OnDisable()
    {
        Gameplay.OnInteraction -= TriggerAnimationFeu;
    }



    /// <summary>
    /// Joue la particule de feu du briquet.
    /// </summary>
    public void AllumerFeu()
    {
        particuleFeuBriquet.Play();
    }
    /// <summary>
    /// Arrête la particule de feu du briquet.
    /// </summary>
    public void EteindreFeu()
    {
        particuleFeuBriquet.Stop();
    }
    /// <summary>
    /// Trigger l'animation du briquet qui s'allume si l'interaction est de type Lampadaire.
    /// </summary>
    /// <param name="interaction">Le type d'interaction</param>
    void TriggerAnimationFeu(TypeInteraction interaction)
    {
        if (interaction == TypeInteraction.Lampadaire) anim.SetTrigger("TriggerFeu");
    }
}
