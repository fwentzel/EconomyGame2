using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Graphic))]
public class ColorPaletteForImage : MonoBehaviour
{
    [SerializeField] string paletteName = "UI";
    Colorpalette palette;
    [Range(0,5)][SerializeField] int level = 0;
    Image image;
    Graphic graphic;


    void OnValidate()
    {
        if (graphic == null || palette == null)
            Reset();
        if (level > palette.levelMapping.Length - 1)
            level = 0;
        if (level < 0)
            level = palette.levelMapping.Length - 1;

        Color newColor = palette.colors[palette.levelMapping[level]];
        newColor.a = graphic.color.a;
        graphic.color = newColor;
    }

    private void Reset()
    {
        graphic = GetComponent<Graphic>();
        GetPalette();
    }

    void GetPalette()
    {
        Colorpalette pal = AssetDatabase.LoadAssetAtPath($"Assets/Scriptable Objects/Colorpalette/{paletteName}.asset", typeof(Colorpalette)) as Colorpalette;
        if (pal == null) return;
        palette = pal;
    }
    public static void RefreshAll(){
        foreach (var item in FindObjectsOfType<ColorPaletteForImage>())
        {
            item.OnValidate();
        }
    }

    
}


