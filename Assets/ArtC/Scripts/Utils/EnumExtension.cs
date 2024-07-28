using ArtC.Systems;

namespace ArtC.Utils {
    public static class EnumExtension {
        public static int ToInt(this GameStates.Phase phase) {
            return (int)phase;
        }
    }
}
