using System;
using UnityEngine.InputSystem.Controls;
using Globals;
using UnityEngine;
using static UnityEngine.Object;

namespace Utils
{
    public static class Gameplay
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
            Debug.Log($"Called to process {typeInteraction} interaction with {obj} object");
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
                    obj.GetComponent<Animator>().SetTrigger("TriggerOnde");
                    break;
                case TypeInteraction.Lampadaire:
                    break;
                case TypeInteraction.Calibration:
                    GameManager.InCalibInterac = true;
                    break;
                case TypeInteraction.CalibrationStop:
                    break;
            }
        }
    }
}

