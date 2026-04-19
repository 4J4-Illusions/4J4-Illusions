# 4J4_jeu-e2

projet de création de jeu en équipe pour le cours 4J4





## Stack technologique

- Unity 6.3 LTS (6000.3.7f1)
    - modules:
        - Microsoft Visual Studio Community 2022
        - Documentation (optionnel)
- Git 2.53.0 (2.53.0.windows.1)
- Git LFS 3.7.1
- GitHub



## Méthodologie

### Git

- **PERSONNE** ne change le code de la branche `dev` à part Pharly
- Toujours créer une branche à partir de la branche `dev` avant de travailler sur un nouveau module
- Nommer la branche selon la formule suivante `nom-module` où *nom* est la personne qui a créé la branche et *module* est la fonctionnalité à ajouter. Par exemple: `pharly-controles-perso`, `sara-musique`
- Si plusieurs personnes doivent travailler sur une même branche, le versionnage sera à être géré entre eux
- Faire un pull request une fois terminé


### Structure des fichiers

- Mettre chaque fichiers et éléments Unity dans le bon dossier
- Créer les dossiers manquants pour les ressources non catégorisées si nécessaire


### Structure du code

- Structure standard pour un script de type `MonoBehaviour`:

```c#
    // Objets Unity
    // Une catégorie de variables publiques qui seront associées a des objets ou composants Unity complexes
    [Header("Objets Unity")] // Ajouter un attribut Header pour les catégories de variables publiques afin de les identifier et les distinguer dans l'inspecteur. Le nom du header a peu d'importance tant que la catégorie est claire
    public GameObject obj;
    public VolumeProfile profile;

    // Variables publiques primitives et simples
    // Une catégorie de variables publiques pour les types de données simples dont la/les valeur(s) peu(ven)t être modifiée(s) directement dans l'inspecteur
    // Inclu aussi les variables dont la valeur devra être accessibles pour d'autres scripts
    // Inclu aussi les évènements C# (Ne pas confondre avec les évènements Unity)
    [Header("Variables publiques ajustables dans l'inspecteur")]
    [Range(0f, 1f)]
    public float vitesse = .1f;
    public Array<int> ages;
    public Vector3 monVecteur; // Un peu comme une liste de 3 float
    [Header("Variables publiques pour d'autres scripts")]
    public Transform trJoueur;

    // Variables de travail
    // Variables qui seront utilisées dans le script
    // Généralement privées, mais elles peuvent être publiques aussi
    RectTransform rectBarre;
    float maxWidth;
    // Constantes
    // Variables dont la valeur ne change pas
    // Peut être utile comme valeur par défaut ou de départ
    Vector2 DEFAULT_POS = new(0, -50);
    Vector3 DEFAULT_ROT = Vector3.zero;
```
- Les méthodes personnalisées devraient se retrouver après les méthodes propres à la Classe `MonoBehaviour`


#### Nomenclature:

- Le nom des classes devraient être en PascalCase
- Le nom des variables devraient généralement être en camelCase
    - Le nom des variables constantes devraient être en SCREAMING_SNAKE_CASE
    - Le nom des évènements devraient être en PascalCase et devraient toujours commencer par `On`. Ex: `OnEyeContact`



## Implémentation

### AudioManager
La classe `AudioManager` est le moyen officiel de gérer les sons d'ambience. Un clip est enregistré quand la méthode `JouerSon` est appelée puisqu'elle crée un composant `AudioSource` sur le `GameObject` auquel est rattaché le script et retourne une référence vers ce composant.

Pour associer un son qui est gére par un autre script (pour le volume, par exemple):
- Attacher le script `AudioManagerConnect` au même `GameObject`
- Récupérer `AudioManagerConnect.audsrc` dans l'autre script pour avoir la nouvelle instance d'`AudioSource` qui contient le clip souhaitée
- **ATTENTION** `AudioManagerConnect` s'attend à avoir l'`AudioSource` de base sur le même `GameObject` que lui et le supprime après avoir créé une nouvelle instance sur l'`AudioManager`

Pour lancer un son d'ambience dès le chargement de la scène:
- Lier le fichier source dans l'`Array` correspondant à la scène avec l'inspecteur
- **ATTENTION** Le numéro à la fin du nom de la variable correspond à l'étape de jeu selon l'enum `StageJeu`