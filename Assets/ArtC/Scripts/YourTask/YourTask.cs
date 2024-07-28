using System;
using ArtC.Damage;
using ArtC.Systems;
using R3;
using UnityEngine;

namespace ArtC.YourTask {
    public class YourTask : MonoBehaviour, IAttacker, IDamageApplicable {
        [SerializeField] private Rigidbody2D _yourTaskRigidbody2D;

        [Header("パラメーター")] [SerializeField] private float _currentSpeed = 3f;
        [SerializeField] private Vector2 _currentDirection = Vector2.zero;
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _stopThreshold = 0.3f;
        [SerializeField] private float _accelerateMultiplier = 0.6f;

        private float _initializedSpeed = 0f;
        private Vector2 _initializedDirection = Vector2.zero;
        private float _maxHitPoint = 1f;
        private ReactiveProperty<float> _currentHitPoint;

        public AttackerTypes Type => AttackerTypes.YourTask;
        public DamageApplicableTypes DamageApplicableType => DamageApplicableTypes.YourTask;
        public ReadOnlyReactiveProperty<float> CurrentHitPoint => _currentHitPoint;
        public float MaxHitPoint => _maxHitPoint;

        private void Awake() {
            _currentHitPoint = new ReactiveProperty<float>(_maxHitPoint);
        }

        private void Start() {
            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                if (pair.next is not GameStates.PlayerMode.Stone) {
                    var elapsedTime = GameManager.Instance.StoneModeElapsedSeconds.CurrentValue;
                    var previousSpeed = _currentSpeed;
                    var newSpeed = previousSpeed + elapsedTime * _accelerateMultiplier;
                    _currentSpeed = Mathf.Min(newSpeed, _maxSpeed);
                }
            }).AddTo(this);

            _currentHitPoint
                .Where(hp => hp <= 0f)
                .Subscribe(_ => { Die(); }).AddTo(this);
        }

        private void FixedUpdate() {
            var deadLinePosition = (Vector2)GameManager.Instance.DeadLine.transform.position;
            var currentPosition = (Vector2)transform.position;
            var sqrDistanceFromDeadLine = (currentPosition - deadLinePosition).sqrMagnitude;

            if (sqrDistanceFromDeadLine < _stopThreshold * _stopThreshold) {
                _yourTaskRigidbody2D.velocity = Vector2.zero;
                return;
            }

            _yourTaskRigidbody2D.velocity = _currentDirection * _currentSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            //Debug.Log("YourTask OnTriggerEnter2D!");
            var attachedRigidbody = other.attachedRigidbody;

            if (attachedRigidbody == null) {
                //Debug.Log("YourTask OnTriggerEnter2D: attachedRigidbody is null!");
                return;
            }

            var damageApplicable = attachedRigidbody.GetComponent<IDamageApplicable>();

            if (damageApplicable == null) {
                //Debug.Log("YourTask OnTriggerEnter2D: damageApplicable is null!");
                return;
            }

            if (damageApplicable.DamageApplicableType is not DamageApplicableTypes.DeadLine) {
                //Debug.Log("YourTask OnTriggerEnter2D: damageApplicable is not DeadLine!");
                return;
            }

            const float damageValue = 10f;
            var damageData = new DamageData(this, damageValue);
            damageApplicable.ApplyDamage(damageData);

            //Debug.Log("YourTask Attacked!");
        }

        public void Initialize(YourTaskInitialParams initialParams) {
            _currentSpeed = initialParams.Speed;
            _initializedSpeed = initialParams.Speed;
            var deadLinePosition = (Vector2)GameManager.Instance.DeadLine.transform.position;
            var direction = (deadLinePosition - initialParams.SpawnPosition).normalized;
            _initializedDirection = direction;
            _currentDirection = direction;

            _yourTaskRigidbody2D.velocity = direction * _currentSpeed;

            _maxHitPoint = initialParams.MaxHitPoint;
            _currentHitPoint = new ReactiveProperty<float>(_maxHitPoint);
        }

        public void ApplyDamage(DamageData damageData) {
            //Debug.Log("YourTask ApplyDamage!");
            if (damageData.Attacker.Type is not AttackerTypes.Projectile) {
                // Debug.Log("YourTask ApplyDamage: damageData.Type is not Projectile!");
                return;
            }

            //Debug.Log("YourTask Damaged!");

            _currentHitPoint.Value -= damageData.DamageValue;
        }

        private void Die() {
            //Debug.Log("YourTask Die!");
            Destroy(gameObject);
        }
    }
}
