using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class AffichageQueteUI : MonoBehaviour
{
    public Animator[] animators;
    public TMP_Text[] textes;
    public QueteCompteur quete;

    private bool estAffiche = false;

    void Start()
    {
        SetEtat(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            estAffiche = !estAffiche;
            SetEtat(estAffiche);

            if (estAffiche)
                MettreAJourTextes();
        }
    }
    private void OnEnable()
    {
        QueteCompteur.OnValeurChange += MettreAJourTextes;
    }
    private void OnDisable()
    {
        QueteCompteur.OnValeurChange -= MettreAJourTextes;
    }



    void SetEtat(bool value)
    {
        foreach (Animator anim in animators)
        {
            if (anim != null)
                anim.SetBool("estAffiche", value);
        }
    }

    void MettreAJourTextes()
    {
        if (quete == null) return;

        string contenu = quete.GetTexte();

        foreach (TMP_Text t in textes)
        {
            if (t != null)
                t.text = contenu;
        }
    }
}