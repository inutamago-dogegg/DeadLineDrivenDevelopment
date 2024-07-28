using UnityEngine;

namespace ArtC.PlayerCharacter {
    public struct ShootProjectileParams {
        public Vector2 Direction { get; }
        public int DirectionCount { get; }
        public float AngleRange { get; }

        public ShootProjectileParams(Vector2 direction, int directionCount, float angleRange) {
            DirectionCount = directionCount;
            AngleRange = angleRange;
            Direction = direction;
        }
    }
}
