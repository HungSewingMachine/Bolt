using System;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct Grid
{
    public int x;
    public int y;
    public int z;

    public Grid((int,int, int) index)
    {
        x = index.Item1;
        y = index.Item2;
        z = index.Item3;
    }

    public override string ToString()
    {
        return $"X: {x}. Y: {y} Z: {z}";
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

/// <summary>
/// Form a 3d grid support layout map
/// </summary>
public class HexaTool : MonoBehaviour
{
#if UNITY_EDITOR
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

            float largestZ = Mathf.NegativeInfinity;
            //
            // // Print(parentTransform);
            // // Iterate through each child transform
            foreach (Transform childTransform in parentTransform)
            {
                // Check if current childTransform's position is the smallest X and largest Z
                if (childTransform.position.z > largestZ)
                {
                    largestZ             = childTransform.position.z;
                    minMaxChildTransform = childTransform;
                }
            }

            if (minMaxChildTransform != null)
            {
                // Output the Transform with smallest X and largest Z
                Debug.Log("Transform with smallest X and largest Z: " + minMaxChildTransform.name);
                var offsetPos = minMaxChildTransform.position;
                // Offset all
                var listPosition = new List<Vector3>();
                foreach (Transform childTransform in parentTransform)
                {
                    listPosition.Add(childTransform.position - offsetPos);
                }

                //Print(parentTransform);
                var data = CalculateSaveData(listPosition, offsetPos);
                JsonHandler.WriteAsset(data, fileName);
            }
            else
            {
                Debug.LogWarning("No child Transform found with smallest X and largest Z.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject found with the name '" + "HexaTool" + "'.");
        }
    }

    public static SavedData CalculateSaveData(List<Vector3> positions, Vector3 offset)
    {
        var saveData = new SavedData
        {
            offset = offset
        };

        // Handle Data
        foreach (var index in positions.Select(GetIndex))
        {
            saveData.gridIndex.Add(index);
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
    /// Convert from position to gridIndex
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Grid GetIndex(Vector3 pos)
    {
        var offset = 0.1f;
        var height = Mathf.FloorToInt(pos.y / GameConstant.Y);
        int col = Mathf.FloorToInt((pos.z - offset) / GameConstant.Z);
        var rowResult = (pos.x + (col % 2 == 0 ? 0 : -GameConstant.X) + offset) / GameConstant.X;
        int row = Mathf.FloorToInt(rowResult);
        return new Grid()
        {
            x = row,
            y = height,
            z = col
        };
    }
#endif
}