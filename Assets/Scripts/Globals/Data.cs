using UnityEngine;

namespace Globals
{
    /// <summary>
    /// Les différents types d'interactions possibles dans le jeu. Ils sont utilisés pour différencier les interactions entre elles et ainsi faire des actions différentes selon le type d'interaction.
    /// </summary>
    public enum TypeInteraction
    {
        None,
        Papier,
        Parler,
        Onde,
        Lampadaire,
        Calibration,
        CalibrationStop
    }

    public enum StageJeu
    {
        Intro,
        Desert,
        Foret,
        Theatre,
        Fin,
        Lobby
    }

    public enum TypeStress
    {
        Proportionnel,
        Instant
    }



    public struct StressPointEntry
    {
        public float valeurStress;
        public TypeStress type;
        public bool pauseProgBarre;

        public override readonly string ToString()
        {
            return $"Valeur Stresss: {valeurStress}    Type: {type}    Pause Barre Prog?: {pauseProgBarre}";
        }
    }
}