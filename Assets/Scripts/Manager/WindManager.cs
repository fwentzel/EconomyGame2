using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WindManager:MonoBehaviour
{

    [SerializeField] Material material;
    private void Start()
    {
        //GameManager.instance.OnCalculateIntervall += ChangeWindDir;
    }

    private void ChangeWindDir()
    {
        if(material==null){
            Debug.LogWarning("No material assigned. Wind not changed");
            return;
        }
        Vector4 currentDir = material.GetVector("WindDir");
        float x=Mathf.Clamp( currentDir.x + UnityEngine.Random.Range(-.1f, .1f),-2,2);
        float y=Mathf.Clamp( currentDir.y + UnityEngine.Random.Range(-.1f, .1f),-2,2);
        material.SetVector("WindDir",new Vector4( x, y, 0, 0));
    }
}