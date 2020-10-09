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
    public override void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            bool isNearWater = PlacementController.instance.harbourPlacements[(int)transform.position.x, (int)transform.position.z];
            PlacementController.instance.SetCanBuild(isNearWater);
            return;
        }
        if (other.CompareTag("Ground")) return;

        //entered a collieder, so disable build
        if (onEnter)
        {
            PlacementController.instance.SetCanBuild(false);
        }
        else
        {

            PlacementController.instance.SetCanBuild(true);
        }
    }
}
