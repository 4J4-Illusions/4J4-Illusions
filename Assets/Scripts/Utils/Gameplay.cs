using System;
using Globals;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Object;

namespace Utils
{
    public class Gameplay
    {
        // evenements
        public static Action<TypeInteraction> OnInteraction;


        /// <summary>
        /// Méthode générale pour gérer les interactions avec les objets interactifs. 
        /// </summary>
        /// <param name="typeInteraction">Le type d'interaction</param>
        /// <param name="obj">Le GameObject qui sera affecté (si applicable)</param>
        public static void Interaction(TypeInteraction typeInteraction, GameObject obj = null)
        {
            Debug.Log($"Gestion de l'interaction de type {typeInteraction} pour l'objet {obj}");
            switch (typeInteraction)
            {
                case TypeInteraction.None:
                    if (obj != null) Destroy(obj);
                    break;
                case TypeInteraction.Papier:
                    break;
                case TypeInteraction.Parler:
                    break;
                case TypeInteraction.Onde:
                    // fait jouer animation onde quand elle ne joue pas presentement
                    // active script indicateur lampadaire
                    if (obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default"))
                    {
                        // donner position du joueur à l'onde
                        obj.transform.position = GameManager.Instance.player.transform.position;

                        obj.GetComponent<Animator>().SetTrigger("TriggerOnde");
                        if (GameManager.Instance.indexLampCour < 5) 
                            GameManager.Instance.listeLampadaires[GameManager.Instance.indexLampCour].GetComponent<IndicateurLampadaireSurEcran>().enabled = true;

                        AudioSource ondeAudsrc = obj.GetComponent<AudioSource>();
                        ondeAudsrc.volume = AudioManager.Instance.SetClipVolume(ondeAudsrc.clip);
                        ondeAudsrc.Play();
                    }
                    break;
                case TypeInteraction.Lampadaire:
                    if (int.Parse(obj.name[^2..]) == GameManager.Instance.indexLampCour)
                    {
                        // detruit le component ObjectInteractif pour arreter la détection par le raycast du joueur sans empêcher les collisions
                        Destroy(obj.GetComponent<ObjetInteractif>());
                        // fait jouer la particule
                        obj.transform.Find("Lampadaire/Final_Candle1/ParticuleFeuLumiere").GetComponent<ParticleSystem>().Play();
                        // active la lumiere
                        obj.transform.Find("Lampadaire/Final_Candle1/PointLightLampadaire").gameObject.SetActive(true);

                        GameManager.Instance.indexLampCour++;
                    }
                    break;
                case TypeInteraction.Calibration:
                    // active l'état en mode calibration, l'overlay de calibration et empêche le joueur de bouger
                    GameManager.Instance.InCalibInterac = true;
                    GameManager.Instance.overlayCalibration.SetActive(true);
                    ControlesPersonnage.canMove = false;
                    break;
                case TypeInteraction.CalibrationStop:
                    // stop la roulette de calibration, cachant l'overlay par conséquent
                    GameManager.Instance.overlayCalibration.GetComponent<CalibRoulette>().StopRoulette();
                    break;
            }

            GameManager.Instance.player.GetComponent<ControlesPersonnage>().texteInteraction.SetActive(false);
            OnInteraction.Invoke(typeInteraction);
        }
        /// <summary>
        /// Lance la séquence de jumpscare, qui correspond à la fin du jeu.
        /// </summary>
        public static void Jumpscare()
        {
            GestionBarreAnxiete.Instance.conteneurBarre.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
            GameManager.Instance.FinDePartie();
        }
    }
}

