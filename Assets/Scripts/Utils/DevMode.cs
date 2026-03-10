using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class DevMode : MonoBehaviour
{
    public bool devMode = false;

    [SerializedDictionary("Fonction à débogger", "Actif")]
    public SerializedDictionary<string, bool> devModeSettings;
}
