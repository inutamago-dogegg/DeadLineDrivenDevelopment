using UnityEngine;

namespace ArtC.YourTask {
    public struct YourTaskInitialParams {
        public Vector2 SpawnPosition { get; }
        public float Speed { get; }
        public float MaxHitPoint { get; }

        public YourTaskInitialParams(Vector2 spawnPosition, float speed, float maxHitPoint) {
            SpawnPosition = spawnPosition;
            Speed = speed;
            MaxHitPoint = maxHitPoint;
        }
    }
}
