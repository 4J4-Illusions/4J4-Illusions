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
        valeurActuelle = Mathf.Clamp(valeurActuelle, 0, valeurMax);

        OnValeurChange?.Invoke();
    }

    public string GetTexte()
    {
        return nomQuete + " : " + valeurActuelle + " / " + valeurMax;
    }
}