using UnityEngine;

namespace ArtC.Utils {
    public static class ColorExtension {
        public static Color SetAlpha(this Color color, float alpha) {
            alpha = Mathf.Clamp01(alpha);
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}
