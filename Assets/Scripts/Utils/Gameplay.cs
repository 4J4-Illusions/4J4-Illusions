using System;
using UnityEngine.InputSystem.Controls;
using Globals;
using UnityEngine;
using static UnityEngine.Object;

namespace Utils
{
    public class Gameplay
    {
        // evenements
        public static Action<TypeInteraction> OnInteraction;

        public static void KeyDependantAction(KeyControl key, Action onPressCallback, Action onReleaseCallback)
        {
            if (key.isPressed)
            {
                onPressCallback();
            }
            else
            {
                onReleaseCallback();
            }
        }


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
                        obj.GetComponent<Animator>().SetTrigger("TriggerOnde");
                        if (GameManager.Instance.indexLampCour < 5) GameManager.Instance.listeLampadaires[GameManager.Instance.indexLampCour].GetComponent<IndicateurLampadaireSurEcran>().enabled = true;
                    }
                    break;
                case TypeInteraction.Lampadaire:
                    if (int.Parse(obj.name[^2..]) == GameManager.Instance.indexLampCour)
                    {
                        // detruire le component ObjectInteractif pour arreter la détection par le raycast du joueur sans empêcher les collisions
                        Destroy(obj.GetComponent<ObjetInteractif>());
                        // desactiver mesh sphere mis comme placeholder pour lumiere
                        MeshRenderer meshrender = obj.transform.Find("Lumiere").GetComponent<MeshRenderer>();
                        meshrender.enabled = false;
                        // jouer particule
                        meshrender.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                        // activer lumiere
                        obj.transform.Find("PointLightLampadaire").gameObject.SetActive(true);

                        GameManager.Instance.indexLampCour++;
                    }
                    break;
                case TypeInteraction.Calibration:
                    GameManager.Instance.InCalibInterac = true;
                    GameManager.Instance.overlayCalibration.SetActive(true);
                    ControlesPersonnage.canMove = false;
                    //obj.SetActive(true);
                    //obj.GetComponent<CalibRoulette>().enabled = true;
                    break;
                case TypeInteraction.CalibrationStop:
                    GameManager.Instance.overlayCalibration.GetComponent<CalibRoulette>().StopRoulette();
                    //GameManager.InCalibInterac = false;
                    //GameManager.overlayCalibration.SetActive(false);
                    break;
            }

            GameManager.Instance.player.GetComponent<ControlesPersonnage>().texteInteraction.SetActive(false);
            OnInteraction.Invoke(typeInteraction);
        }
    }
}

