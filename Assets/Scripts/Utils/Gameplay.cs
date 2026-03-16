using System;
using UnityEngine.InputSystem.Controls;
using Globals;
using UnityEngine;
using static UnityEngine.Object;

namespace Utils
{
    public class Gameplay
    {
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
            //Debug.Log($"Called to process {typeInteraction} interaction with {obj} object");
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
                        if (GameManager.indexLampCour < 5) GameManager.listeLampadaires[GameManager.indexLampCour].GetComponent<IndicateurLampadaireSurEcran>().enabled = true;
                    }
                    break;
                case TypeInteraction.Lampadaire:
                    // code placeholder, a changer eventuellement
                    if (int.Parse(obj.name[^2..]) == GameManager.indexLampCour)
                    {
                        Destroy(obj);
                        GameManager.indexLampCour++;
                    }
                    break;
                case TypeInteraction.Calibration:
                    GameManager.InCalibInterac = true;
                    obj.SetActive(true);
                    //obj.GetComponent<CalibRoulette>().enabled = true;
                    break;
                case TypeInteraction.CalibrationStop:
                    GameManager.InCalibInterac = false;
                    break;
            }
        }
    }
}

