using R3;

namespace ArtC.Damage {
    public interface IDamageApplicable {
        public DamageApplicableTypes DamageApplicableType { get; }
        public ReadOnlyReactiveProperty<float> CurrentHitPoint { get; }
        public float MaxHitPoint { get; }
        public void ApplyDamage(DamageData damageData);
    }
}
