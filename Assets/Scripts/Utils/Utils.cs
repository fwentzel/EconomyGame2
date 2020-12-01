using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static LayerMask defaultMask = LayerMask.GetMask("Default");
    static int groundMask = LayerMask.GetMask("Ground");
    public static Vector3 GetMouseGroundPosition(Vector2 mouseScreenPos)
    {
        //Get Mouseposition in World coordinates on Ground Collider
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    public static GameObject GetObjectAtMousePos(Vector2 mouseScreenPos)
    {
        //Get Mouseposition in World coordinates on Ground Collider
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, defaultMask))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }

    public static BuildingPlacementInfo GetBuildInfoForTeam(Type type, Team team)
    {
        return PlacementSpotsManager.spots[type].Find(spots => spots.team == team);
    }

    /// <summary>Gets an array of assets of type T at a given path. This path is relative to /Assets.</summary>
    /// <returns>An array of assets of type T.</returns>
    /// <param name="path">The file path relative to /Assets.</param>
    public static T[] GetAssetsAtPath<T>(string path) where T : UnityEngine.Object
    {
        List<T> returnList = new List<T>();

        //get the contents of the folder's full path (excluding any meta files) sorted alphabetically
        IEnumerable<string> fullpaths = Directory.GetFiles(Application.dataPath, "*.asset", SearchOption.AllDirectories);
        //loop through the folder contents
        foreach (string fullpath in fullpaths)
        {
            //determine a path starting with Assets
            string assetPath = "Assets" + fullpath.Replace(Application.dataPath, "").Replace('\\', '/');
            //load the asset at this relative path
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            //and add it to the list if it is of type T
            if (obj is T) { returnList.Add(obj as T); }
        }

        return returnList.ToArray();
    }
}
