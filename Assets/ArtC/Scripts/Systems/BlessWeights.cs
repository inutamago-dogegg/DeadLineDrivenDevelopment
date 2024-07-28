using System;
using ArtC.Bless;

namespace ArtC.Systems {
    [Serializable]
    public record BlessWeights {
        public float ShootDirectionCountWeight = 1f;
        public float ShootAngleRangeWeight = 1f;
        public float ShootIntervalWeight = 1f;
        public float PenetrationWeight = 1f;
        public float ProjectileDamageWeight = 1f;
        public float ProjectileSpeedWeight = 1f;
        public float ProjectileSizeWeight = 1f;
        public float ProjectileLifeTimeSecondsWeight = 1f;

        private float[] _weights;
        private BlessTypes[] _types;

        public float[] Weights => _weights ??= new[] {
            ShootDirectionCountWeight,
            ShootAngleRangeWeight,
            ShootIntervalWeight,
            PenetrationWeight,
            ProjectileDamageWeight,
            ProjectileSpeedWeight,
            ProjectileSizeWeight,
            ProjectileLifeTimeSecondsWeight
        };

        public BlessTypes[] Types => _types ??= new[] {
            BlessTypes.ShootDirectionCount,
            BlessTypes.ShootAngleRange,
            BlessTypes.ShootInterval,
            BlessTypes.Penetration,
            BlessTypes.ProjectileDamage,
            BlessTypes.ProjectileSpeed,
            BlessTypes.ProjectileSize,
            BlessTypes.ProjectileLifeTimeSeconds
        };
    }
}
