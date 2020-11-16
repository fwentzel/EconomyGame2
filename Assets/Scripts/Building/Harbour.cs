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
        UseMaxPlacementRange = false;
    }
    protected override void SetupPossiblePlacements(Team t)
    {
        Harbour[] harbours = FindObjectsOfType<Harbour>();
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        for (int x = 0; x < mapGenerator.xSize; x++)
        {
            for (int z = 0; z < mapGenerator.zSize; z++)
            {
                Vector2 pos = new Vector2(x, z);
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h < 0))
                {
                    // bool foundHarbourOnSpot = false;
                    // for (int i = 0; i < harbours.Length; i++)
                    // {
                    //     if (harbours[i].transform.position.x == x && harbours[i].transform.position.z == z)
                    //     {
                    //         foundHarbourOnSpot = true;
                    //     }
                    // }
                    // if(!foundHarbourOnSpot)
                        possibleDefaultPlacements.Add(new Vector2(x, z));
                }
            }
        }
    }

    //TODO
    public override void CheckCanBuild(Collider other, bool onEnter)
    {

        //only distance check
        if (other == null)
        {
            PlacementController.instance.SetCanBuild(possibleDefaultPlacements.Contains(new Vector2(transform.position.x, transform.position.z)));
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
