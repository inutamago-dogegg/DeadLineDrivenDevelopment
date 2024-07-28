namespace ArtC.Damage {
    public record DamageData {
        public DamageData(IAttacker attacker, float damageValue) {
            _attacker = attacker;
            _damageValue = damageValue;
        }

        private readonly IAttacker _attacker;
        private readonly float _damageValue;

        public IAttacker Attacker => _attacker;
        public float DamageValue => _damageValue;
    }
}
