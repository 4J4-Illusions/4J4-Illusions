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
### Structure des fichiers



### Structure du code

```c#
    //      Objets Unity
    [Header("Objets Unity")]
    public GameObject conteneur;

    //      Variables publiques ajustables dans l'inspecteur
    [Header("Variables publiques ajustables dans l'inspecteur"), Range(0f, 1f)]
    public float vitProgBarre = .1f;

    //      Variables de travail
    RectTransform rectBarre;
    float maxWidth;
    //  Constantes
    Vector2 DEFAULT_POS = new(0, -50);
    Vector3 DEFAULT_ROT = Vector3.zero;
```
