using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUiSprites : MonoBehaviour
{
    public float margin = 1;
    public GameObject[] prefabs;
    private void Start()
    {
        UpdateBuildingUiSprites();
    }
    [ContextMenu("Update Sprites")]
    private void UpdateBuildingUiSprites()
    {
        Transform cameraHeightTransform = transform.GetChild(0);
        Camera cam = cameraHeightTransform.GetComponentInChildren<Camera>();
        MeshFilter mfilter = GetComponent<MeshFilter>();
        MeshRenderer mrenderer = GetComponent<MeshRenderer>();
        
        for (int i = 0; i < prefabs.Length; i++)
        {
            mfilter.sharedMesh = prefabs[i].GetComponent<MeshFilter>().sharedMesh;
            MeshRenderer meshRenderer = prefabs[i].GetComponent<MeshRenderer>();
            mrenderer.sharedMaterial = meshRenderer.sharedMaterial;
            float modelHeight=meshRenderer.bounds.extents.y;
            float size = Mathf.Max(modelHeight, meshRenderer.bounds.extents.x);

            Vector3 pos = cameraHeightTransform.localPosition;
            cameraHeightTransform.localPosition=new Vector3(pos.x,modelHeight/2f,pos.z);
           

            cam.orthographicSize = size + margin;
            cam.targetTexture = prefabs[i].GetComponent<Building>().renderTexture;
            cam.Render();

        }
    }
   

}
