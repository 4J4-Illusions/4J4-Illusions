using System;
using UnityEngine;

public class QueteCompteur : MonoBehaviour
{
    public string nomQuete = "Objectif";
    public static int valeurActuelle = 0;
    public static int valeurMax = 5;

    public static event Action OnValeurChange;

    public static void Ajouter(int montant)
    {
        valeurActuelle += montant;

        if (valeurActuelle > valeurMax)
            valeurActuelle = valeurMax;

        OnValeurChange?.Invoke();
    }

    public string GetTexte()
    {
        return nomQuete + " : " + valeurActuelle + " / " + valeurMax;
    }
}