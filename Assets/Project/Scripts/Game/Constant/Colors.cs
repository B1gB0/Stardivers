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

            return _palette != null ? _palette.GetColor(colorName) : Color.magenta;
        }

        private static void LoadPaletteIfNeeded()
        {
            if (_palette != null)
                return;

            _palette = Resources.Load<CustomPalette>(PalettePath);
        }
    }
}