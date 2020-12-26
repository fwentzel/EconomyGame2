using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    public GameObject levelUpEffect;
    public GameObject buildEffect;

    public static VFXManager instance;

    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public void PlayEffect(Vector3 pos, effect eff)
    {
        GameObject chosenEffect=null;
        if (eff == effect.LEVEL_UP)
            chosenEffect = levelUpEffect;
        if (eff == effect.BUILD)
            chosenEffect = buildEffect;

        GameObject newEffect = Instantiate(chosenEffect, new Vector3(pos.x, chosenEffect.transform.position.y, pos.z), Quaternion.identity);

        Destroy(newEffect, 6);

    }
}
public enum effect
{
    LEVEL_UP,
    BUILD
}