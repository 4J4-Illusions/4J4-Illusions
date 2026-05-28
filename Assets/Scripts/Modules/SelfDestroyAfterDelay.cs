using UnityEngine;

public class SelfDestroyAfterDelay : MonoBehaviour
{
    [Header("Affectation inspecteur"), Space(30)]
    [Header("Ajustement inspecteur")]
    public float delay = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DestroyAfterDelay), delay);
    }



    /// <summary>
    /// Supprime le GameObject après un délai spécifié. Utilise la fonction Invoke pour appeler cette méthode après le délai défini dans l'inspecteur.
    /// </summary>
    void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}
