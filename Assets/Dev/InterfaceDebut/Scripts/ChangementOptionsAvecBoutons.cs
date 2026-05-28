using UnityEngine;
using TMPro;

public class ChangementOptionsAvecBoutons : MonoBehaviour
{
    public TMP_Text texteOption;   // Texte affiché
    public string[] options;       // Tableau des options
    private int index = 0;         // Index courant
    //Aller lire l'index publiquement pour ne pas faire de problèmes avec le changement de langue.
    public int Index
{
    get { return index; }
}

    void Start()
    {
        // Affiche la première option au démarrage
        if (options.Length > 0)
            texteOption.text = options[index];
    }

    // Bouton → suivant
    public void Suivant()
    {
        index++;
        if (index >= options.Length)
            index = 0; // revient au début

        texteOption.text = options[index];
    }

    // Bouton → précédent
    public void Precedent()
    {
        index--;
        if (index < 0)
            index = options.Length - 1; // revient à la fin

        texteOption.text = options[index];
    }
}