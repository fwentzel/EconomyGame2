using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PersonalityManager : MonoBehaviour
{

    public static PersonalityManager instance { get; private set; }
    public AiPersonality defaultPersonality ;
    public AiPersonality[] personalities { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        if (defaultPersonality == null)
        {
            defaultPersonality = ScriptableObject.CreateInstance<AiPersonality>();
            defaultPersonality.name="Default";
        }

        List<AiPersonality> personalitiesList = new List<AiPersonality>();
        foreach (var item in Utils.GetAssetsAtPath<AiPersonality>("/AiPersonality"))
        {
            if (item != defaultPersonality)
                personalitiesList.Add(item);
        }
        personalities = personalitiesList.ToArray();
    }
}
