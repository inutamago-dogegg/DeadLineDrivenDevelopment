using UnityEngine;

namespace ArtC.Projectile {
    public struct ProjectileInitialParams {
        public Vector2 Direction { get; }

        public int PenetrateCount { get; }

        public float DamageValue { get; }

        public float Speed { get; }

        public float Size { get; }
        public float LifeTimeSeconds { get; }

        public ProjectileInitialParams(Vector2 direction, int penetrateCount, float damageValue, float speed,
            float size, float lifeTimeSeconds) {
            Direction = direction;
            PenetrateCount = penetrateCount;
            DamageValue = damageValue;
            Speed = speed;
            Size = size;
            LifeTimeSeconds = lifeTimeSeconds;
        }
    }
}
