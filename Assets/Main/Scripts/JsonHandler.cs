using System.IO;
using UnityEngine;

namespace Main.Scripts
{
    public static class JsonHandler
    {
        public static SavedData GetLevelData(string fileName)
        {
            var result = new SavedData();
            
            string filePath = Path.Combine(Application.dataPath, "Resources", $"{fileName}.json");
            
            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the JSON file
                var jsonString = File.ReadAllText(filePath);

                // Parse the JSON data into a JSON object
                result = JsonUtility.FromJson<SavedData>(jsonString);

                Debug.Log($"RedFlag data {JsonUtility.ToJson(result)}");
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }

            return result;
        }
        
        public static void WriteAsset(SavedData data, string fileName)
        {
            string jsonString = JsonUtility.ToJson(data);
            string directoryPath = Path.Combine(Application.dataPath , "Resources");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        
            // Construct the file path
            string filePath = Path.Combine(directoryPath, $"{fileName}.json");
            File.WriteAllText(filePath, jsonString);
            Debug.Log("JSON file saved to: " + filePath);
        }
    }
}