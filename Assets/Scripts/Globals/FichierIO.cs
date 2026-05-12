using System;
using System.IO;
using UnityEngine;

namespace Globals
{
    public static class FichierIO
    {
        public static void Create(string path, string data)
        {
            File.WriteAllText(path, data);
        }
        public static T Read<T>(string path)
        {
            if (File.Exists(path))
            {
                Debug.Log($"fichier \'settings.json\' éxiste:\n{path}");
                return JsonUtility.FromJson<T>(File.ReadAllText(path));
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        public static object Read(string path, Type type)
        {
            if (File.Exists(path))
            {
                Debug.Log($"fichier \'settings.json\' éxiste:\n{path}");
                return JsonUtility.FromJson(File.ReadAllText(path), type);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        public static void Update() { }
        public static void Delete() { }
    }
}