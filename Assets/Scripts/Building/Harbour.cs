﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Harbour : Building
{
    public override void OnBuild(bool subtractResource = true)
    {
        if (PlacementController.instance == null) return;//Only occurs during mapgeneration
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.up) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 90);
        }
        else if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.down) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, -90);
        }
        else if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.right) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 180);
        }
        base.OnBuild();
    }
    private void Awake()
    {
        UseMaxPlacementRange = false;
    }
    protected override void SetupPossiblePlacements(Team t)
    {
        if (PlacementSpotsManager.spots[GetType()].Count > 0)
        {
            //just copy previous spots, since they are identical
            possiblePlacementsCache=PlacementSpotsManager.spots[GetType()][0].possibleSpots;
             base.SetupPossiblePlacements(t);
             return;
        }
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        int waterDepth=Array.Find<ColorToHeight>(mapGenerator.colorToHeightMapping.colorHeightMappings, r => r.name.Equals("Water") ).vertexHight;
       
        for (int x = 0; x < mapGenerator.xSize; x++)
        {
            for (int z = 0; z < mapGenerator.zSize; z++)
            {
                Vector2 pos = new Vector2(x, z);
                //Waterdepth/2 because there should be atleast 2 tiles with water next to harbour
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h < waterDepth/2))
                {
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }
        
       
        
        base.SetupPossiblePlacements(t);
    }


    //TODO
    public override void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            PlacementController.instance.SetCanBuild(Utils.GetBuildInfoForTeam(GetType(), team).possibleSpots.Contains(new Vector2(transform.position.x, transform.position.z)));
            return;
        }

        //entered a collieder, so disable build
        if (onEnter)
        {
            PlacementController.instance.SetCanBuild(false);
        }
        else
        {
            //TODO
            PlacementController.instance.SetCanBuild(true);
        }
    }




}
