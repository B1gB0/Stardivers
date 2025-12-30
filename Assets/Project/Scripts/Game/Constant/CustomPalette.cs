using UnityEngine;

namespace Project.Scripts.Game.Constant
{
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "Colors/Color Palette")]
    public class CustomPalette : ScriptableObject
    {
        [SerializeField] private ColorEntry[] colors;

        public Color GetColor(ColorName colorName)
        {
            foreach (var entry in colors)
            {
                if (entry.Name == colorName)
                {
                    return entry.Color;
                }
            }

            return Color.magenta;
        }
    }
}