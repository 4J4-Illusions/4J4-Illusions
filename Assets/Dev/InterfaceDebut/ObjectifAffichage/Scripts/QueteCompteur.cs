using UnityEngine;

public class QueteCompteur : MonoBehaviour
{
    public string nomQuete = "Objectif";
    public int valeurActuelle = 0;
    public int valeurMax = 5;

    public delegate void OnValeurChange();
    public event OnValeurChange onValeurChange;

    public void Ajouter(int montant)
    {
        valeurActuelle += montant;

        if (valeurActuelle > valeurMax)
            valeurActuelle = valeurMax;

        onValeurChange?.Invoke();
    }

    public string GetTexte()
    {
        return nomQuete + " : " + valeurActuelle + " / " + valeurMax;
    }
}