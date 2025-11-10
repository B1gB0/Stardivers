using System;
using UnityEngine;

namespace Project.Scripts.Game.Constant
{
    public static class Colors
    {
        private static CustomPalette _palette;
        private const string PalettePath = "ColorPalette";

        public static Color GetColor(ColorName colorName)
        {
            LoadPaletteIfNeeded();

            if (_palette == null)
            {
                Debug.LogError("Custom Palette not found!");
                return Color.magenta;
            }

            return _palette.GetColor(colorName);
        }

        public static Color GetColor(string colorName)
        {
            LoadPaletteIfNeeded();

            if (_palette == null)
            {
                Debug.LogError("Custom Palette not found!");
                return Color.magenta;
            }
            
            if (Enum.TryParse(colorName, true, out ColorName name))
                return _palette.GetColor(name);

            Debug.LogError($"Color name '{colorName}' is not valid!");
            return Color.magenta;
        }

        private static void LoadPaletteIfNeeded()
        {
            if (_palette == null)
            {
                _palette = Resources.Load<CustomPalette>(PalettePath);

                if (_palette == null)
                    Debug.LogError($"Failed to load palette at path: {PalettePath}");
            }
        }
    }
}