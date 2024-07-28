using System;
using UnityEngine;

namespace ArtC.Projectile {
    [Serializable]
    public record ProjectileCurrentParams {
        public Vector2? Direction = null;
        public float? Speed = null;
        public float? Size = null;
        public int? RemainingPenetrateCount = null;
        public float? DamageValue = null;
    }
}
