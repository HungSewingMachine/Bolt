using System;
using System.Collections.Generic;
using System.IO;
using Main.Scripts;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Grid
{
    public int x;
    public int y;

    public Grid((int, int) index)
    {
        x = index.Item1;
        y = index.Item2;
    }
}

[Serializable]
public class SavedData
{
    public List<Grid> gridIndex;

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    public Vector3 offset;
    
    public SavedData()
    {
        gridIndex = new List<Grid>();
    }
}

public class HexaTool : MonoBehaviour
{
#if UNITY_EDITOR

    public const float X = 0.5f;          // Kc tam -> edge 
    public const float Z = -0.58f / 2;    // kc tam -> vertex /2
    
    public List<Vector2> index; 
    public GameObject    prefab;
    public string        fileName;

    private void Start()
    {
        var saveData = JsonHandler.GetLevelData(fileName);
        for (int i = 0; i < saveData.gridIndex.Count; i++)
        {
            Instantiate(prefab, GetPosition(saveData.gridIndex[i], saveData.offset), Quaternion.identity);
        }
    }

    [MenuItem("Tools/ExportHexaData", false, 51)]
    public static void ExportHexaData()
    {
        // Find the GameObject by its name
        GameObject parentObject = GameObject.Find("Tool");

        // Check if the GameObject was found
        if (parentObject != null)
        {
            // Get the Transform component of the parent GameObject
            Transform parentTransform = parentObject.transform;
            var fileName = parentTransform.parent.gameObject.name;

            Transform minMaxChildTransform = null;
            
            // float smallestX = Mathf.Infinity;
            float largestZ = Mathf.NegativeInfinity;
            //
            // // Print(parentTransform);
            // // Iterate through each child transform
            foreach (Transform childTransform in parentTransform)
            {
                // Check if current childTransform's position is the smallest X and largest Z
                if (childTransform.position.z > largestZ)
                {
                    largestZ = childTransform.position.z;
                    minMaxChildTransform = childTransform;
                }
            }
            
            if (minMaxChildTransform != null)
            {
                // Output the Transform with smallest X and largest Z
                Debug.Log("Transform with smallest X and largest Z: " + minMaxChildTransform.name);
            }
            else
            {
                Debug.LogWarning("No child Transform found with smallest X and largest Z.");
            }
            
            var offsetPos = minMaxChildTransform.position;
            // Offset all
            var result = new List<Vector3>();
            foreach (Transform childTransform in parentTransform)
            {
                result.Add(childTransform.position - offsetPos); 
            }

            //Print(parentTransform);
            var data = CalculateSaveData(result, offsetPos);
            JsonHandler.WriteAsset(data, fileName);
        }
        else
        {
            Debug.LogWarning("No GameObject found with the name '" + "HexaTool" + "'.");
        }
    }

    // public static void Print(Transform parentTransform)
    // {
    //     // Iterate through each child transform
    //     foreach (Transform childTransform in parentTransform)
    //     {
    //         // Do something with the child transform, for example, print its name
    //         Debug.Log($"Child Name: {childTransform.name} index {GetIndex(childTransform.position)}");
    //     }
    // }

    public static SavedData CalculateSaveData(List<Vector3> positions, Vector3 offset)
    {
        var saveData = new SavedData
        {
            offset = offset
        };
        
        // Handle Data
        foreach (var pos in positions)
        {
            // Do something with the child transform, for example, print its name
            var index = GetIndex(pos);
            // Debug.Log($"Child Name: {childTransform.name} index {index}");
            saveData.gridIndex.Add(new Grid(index));
        }

        var camera = Camera.main;
        if (camera == null)
        {
            Debug.LogError($"RedFlag You should add a camera to scene!");
        }
        else
        {
            var cameraTransform = camera.transform;
            saveData.cameraPosition = cameraTransform.position;
            saveData.cameraRotation = cameraTransform.rotation.eulerAngles;
        }
        
        
        return saveData;
    }
    
    /// <summary>
    /// Create Save file in json format
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    public static void WriteAsset(SavedData data, string fileName)
    {
        string jsonString = JsonUtility.ToJson(data);
        string directoryPath = Application.dataPath + "/SavedData";

        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        // Construct the file path
        string filePath = directoryPath + $"/{fileName}.json";
        File.WriteAllText(filePath, jsonString);
        Debug.Log("JSON file saved to: " + filePath);
    }

    /// <summary>
    /// Convert from position to gridIndex
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static (int, int) GetIndex(Vector3 pos)
    {
        var offset = 0.1f;
        // int col = (int)((pos.z - offset) / -0.87);
        int col = Mathf.FloorToInt((pos.z - offset) / Z);
        var rowResult = (pos.x + (col%2 == 0 ? 0 : -X) + offset) / X;
        // var rowResult = (pos.x + offset) / X;
        int row = Mathf.FloorToInt(rowResult);
        return (row, col);
    }
    
    /// <summary>
    /// Convert from gridIndex to position
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public static Vector3 GetPosition(Grid grid, Vector3 offset)
    {
        var offsetX = Mathf.Approximately(grid.y % 2, 1) ? X : 0;
        return new Vector3(grid.x * X + offsetX, 0, grid.y * Z) + offset;
    }
#endif

}
