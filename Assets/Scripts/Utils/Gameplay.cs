using System;
using UnityEngine.InputSystem.Controls;

namespace Utils
{
    public class Gameplay
    {
        public void KeyDependantAction(KeyControl key, Action onPressCallback, Action onReleaseCallback)
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
    }
}

