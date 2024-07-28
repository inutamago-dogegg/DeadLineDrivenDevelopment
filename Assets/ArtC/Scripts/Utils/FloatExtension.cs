using UnityEngine;

namespace ArtC.Utils {
    public static class FloatExtension {
        public static int ToInt(this float value) {
            return (int)value;
        }

        public static int ToRoundInt(this float value) {
            return Mathf.RoundToInt(value);
        }
    }
}
