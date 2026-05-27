using System;

namespace Globals
{
    /// <summary>
    /// Les différents types d'interactions possibles dans le jeu. Ils sont utilisés pour différencier les interactions entre elles et ainsi faire des actions différentes selon le type d'interaction.
    /// </summary>
    public enum TypeInteraction
    {
        None,
        Papier,
        Dialogue,
        Onde,
        Lampadaire,
        Calibration,
        CalibrationStop,
        Recompense
    }
    /// <summary>
    /// Décrit les différentes étapes du jeu.
    /// </summary>
    public enum StageJeu
    {
        Menu,
        Intro,
        Desert,
        Foret,
        Theatre,
        Fin,
        Lobby
    }
    /// <summary>
    /// Les types de stress possibles pour un point de stress.
    /// </summary>
    public enum TypeStress
    {
        /// <summary>
        /// Augmente le niveau de stress de manière inversement proportionnelle à la distance entre le joueur et le point de stress.
        /// <para></para>
        /// Augmentation continue sur le tmeps tant que le joueur est dans la portée du point de stress.
        /// </summary>
        Proportionnel,
        /// <summary>
        /// Augmente le niveau de stress instantanément une seule fois quand le joueur est à portée.
        /// <para></para>
        /// Au lieu de continuellement augmenter le stress, empêche plutôt la diminution naturelle tant que le joueur est à portée.
        /// </summary>
        Instant
    }
    public enum CategorieSon
    {
        Ambience,
        SFX
    }



    /// <summary>
    /// Défini une entrée de donnée pour un point de stress.
    /// <para></para>
    /// Cette structure se retrouve dans un dictionnaire dans la classe <see cref="GestionBarreAnxiete"/> pour permettre de stocker et d'accéder facilement aux différentes valeurs de stress associées à chaque point de stress dans le jeu."/>
    /// </summary>
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