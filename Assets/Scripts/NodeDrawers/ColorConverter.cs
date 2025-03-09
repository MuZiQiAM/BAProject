using System;
using UnityEngine;
using XNodeEditor;


public class ColorConverter
{
    public static Color HexToColor(string hex)
    {
        if (hex.Length != 6)
        {
            throw new ArgumentException("wrong hex code");
        }

        var r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        var g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        var b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        return new Color(r, g, b, 1f);
    }
    
    
}