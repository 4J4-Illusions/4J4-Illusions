using System;
using UnityEngine.InputSystem.Controls;
using Globals;
using UnityEngine;

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
            switch (typeInteraction)
            {
                case TypeInteraction.None:
                    if (obj != null) UnityEngine.Object.Destroy(obj);
                    break;
            }
        }
    }
}

