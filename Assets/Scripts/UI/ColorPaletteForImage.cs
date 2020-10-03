using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Graphic))]
public class ColorPaletteForImage : MonoBehaviour
{
    [SerializeField] string paletteName = "UI";
    Colorpalette palette;
    [Range(0, 19)] [SerializeField] int colorlevel = 0;
    Image image;
    Graphic graphic;

    void OnValidate()
    {
        if (graphic == null || palette == null)
            Reset();
        if (colorlevel > palette.colors.Length - 1)
            colorlevel = 0;
        else if (colorlevel < 0)
            colorlevel = palette.colors.Length - 1;

        Color newColor = palette.colors[colorlevel];
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
}


