using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[CreateAssetMenu]
public class SaveTest : ScriptableObject
{
    [Serializable]
    private class ScriptableObjectData
    {
        public string type;
        public string json;
    }

    [Serializable]
    private class SaveDataContainer
    {
        public List<ScriptableObjectData> saveData = new();
    }

    public List<ScriptableObject> saveData = new();

    private Dictionary<string, Type> typeMap;

    private void InitializeTypeMap()
    {
        typeMap = new Dictionary<string, Type>();
        foreach (var scriptableObject in saveData)
        {
            string typeName = scriptableObject.GetType().AssemblyQualifiedName;
            if (!typeMap.ContainsKey(typeName))
            {
                typeMap.Add(typeName, scriptableObject.GetType());
            }
        }
    }

    public void SaveData()
    {
        InitializeTypeMap();
        SaveDataContainer container = new SaveDataContainer();

        foreach (var scriptableObject in saveData)
        {
            ScriptableObjectData data = new ScriptableObjectData
            {
                type = scriptableObject.GetType().AssemblyQualifiedName,
                json = JsonUtility.ToJson(scriptableObject)
            };
            container.saveData.Add(data);
        }

        string path = Application.persistentDataPath + "/saveData.json";
        string json = JsonUtility.ToJson(container, true);
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataContainer container = JsonUtility.FromJson<SaveDataContainer>(json);

            InitializeTypeMap();

            // Create a dictionary to map types to their instances in the existing list
            Dictionary<string, ScriptableObject> existingObjects = new Dictionary<string, ScriptableObject>();
            foreach (var scriptableObject in saveData)
            {
                string typeName = scriptableObject.GetType().AssemblyQualifiedName;
                existingObjects[typeName] = scriptableObject;
            }

            foreach (var data in container.saveData)
            {
                if (typeMap.TryGetValue(data.type, out Type type))
                {
                    if (existingObjects.TryGetValue(data.type, out ScriptableObject existingObj))
                    {
                        // Update existing object
                        JsonUtility.FromJsonOverwrite(data.json, existingObj);
                    }
                    else
                    {
                        // Create new object if it doesn't exist
                        ScriptableObject obj = ScriptableObject.CreateInstance(type);
                        JsonUtility.FromJsonOverwrite(data.json, obj);
                        saveData.Add(obj);
                    }
                }
                else
                {
                    Debug.LogError($"Tipo non riconosciuto: {data.type}");
                }
            }
        }
    }

    public void DeleteData()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
