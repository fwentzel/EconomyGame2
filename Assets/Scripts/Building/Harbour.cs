using System.Collections;
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
        spotType = buildSpotType.harbour;
    }
    protected override void SetupPossiblePlacements()
    {
        if (PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType].Count > 0)
        {
            //just copy previous spots, since they are identical
            possiblePlacementsCache=PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team];
             base.SetupPossiblePlacements();
             return;
        }

        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
       
        for (int x = 0; x < mapGenerator.xSize; x++)
        {
            for (int z = 0; z < mapGenerator.zSize; z++)
            {
                Vector2 pos = new Vector2(x, z);
                //Waterdepth/2 because there should be atleast 2 tiles with water next to harbour
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h < mapGenerator.waterVertexHeight/2))
                {
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }        
        base.SetupPossiblePlacements();
    }


    //TODO
    public override void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            PlacementController.instance.SetCanBuild(spotType, team,new Vector2(transform.position.x, transform.position.z));
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
