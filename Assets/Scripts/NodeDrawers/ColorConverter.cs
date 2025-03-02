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

        if (hex.Length == 6)
        {
            float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
            float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

            return new Color(r, g, b, 1f);
        }

        return Color.black; // if something went wrong, the color will be set to black
    }
}