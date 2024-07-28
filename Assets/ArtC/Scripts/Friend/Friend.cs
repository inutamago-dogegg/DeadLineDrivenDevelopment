using System;
using ArtC.Bless;
using ArtC.PlayerCharacter;
using ArtC.Projectile;
using ArtC.Systems;
using ArtC.Utils;
using R3;
using UnityEngine;

namespace ArtC.Friend {
    public class Friend : MonoBehaviour {
#region Fields

        [SerializeField] private Projectile.Projectile _projectilePrefab;
        [SerializeField] private SpriteRenderer _model;

        [Header("パラメーター")] [SerializeField] private float _stoneColorAlpha = 0.2f;
        [SerializeField] [Range(-180f, 180f)] private float _shootAngle = 0f;


        [SerializeField] private ShootParamEnhanceCurves _basicCurves;
        [SerializeField] private ShootParamEnhanceCurves _blessCurves;

        private Camera _mainCamera;
        private Vector2 _shootDirection = Vector2.up;
        private readonly ReactiveProperty<float?> _currentElapsedSecondsFromLastShoot = new(null);

        private readonly ShootParams _basicShootParams = new();
        private readonly ShootParams _blessedShootParams = new();

        private ReactiveProperty<int> _shootDirectionCount;
        private ReactiveProperty<float> _shootAngleRange;
        private ReactiveProperty<float> _shootIntervalSeconds;
        private ReactiveProperty<int> _projectilePenetrateCount;
        private ReactiveProperty<float> _projectileDamageValue;
        private ReactiveProperty<float> _projectileSpeed;
        private ReactiveProperty<float> _projectileSize;
        private ReactiveProperty<float> _projectileLifeTimeSeconds;

        private readonly ReactiveProperty<int> _shootDirectionCountBlessCount = new(0);
        private readonly ReactiveProperty<int> _shootAngleRangeBlessCount = new(0);
        private readonly ReactiveProperty<int> _shootIntervalSecondsBlessCount = new(0);
        private readonly ReactiveProperty<int> _projectilePenetrateCountBlessCount = new(0);
        private readonly ReactiveProperty<int> _projectileDamageValueBlessCount = new(0);
        private readonly ReactiveProperty<int> _projectileSpeedBlessCount = new(0);
        private readonly ReactiveProperty<int> _projectileSizeBlessCount = new(0);
        private readonly ReactiveProperty<int> _projectileLifeTimeSecondsBlessCount = new(0);

#endregion

#region Properties

        public float ShootAngle => _shootAngle;

        public ReadOnlyReactiveProperty<float?> CurrentElapsedSecondsFromLastShoot =>
            _currentElapsedSecondsFromLastShoot;

        public ReadOnlyReactiveProperty<int> ShootDirectionCount => _shootDirectionCount;
        public ReadOnlyReactiveProperty<float> ShootAngleRange => _shootAngleRange;
        public ReadOnlyReactiveProperty<float> ShootIntervalSeconds => _shootIntervalSeconds;
        public ReadOnlyReactiveProperty<int> ProjectilePenetrateCount => _projectilePenetrateCount;
        public ReadOnlyReactiveProperty<float> ProjectileDamageValue => _projectileDamageValue;
        public ReadOnlyReactiveProperty<float> ProjectileSpeed => _projectileSpeed;
        public ReadOnlyReactiveProperty<float> ProjectileSize => _projectileSize;
        public ReadOnlyReactiveProperty<float> ProjectileLifeTimeSeconds => _projectileLifeTimeSeconds;

        public ReadOnlyReactiveProperty<int> ShootDirectionCountBlessCount => _shootDirectionCountBlessCount;
        public ReadOnlyReactiveProperty<int> ShootAngleRangeBlessCount => _shootAngleRangeBlessCount;
        public ReadOnlyReactiveProperty<int> ShootIntervalSecondsBlessCount => _shootIntervalSecondsBlessCount;
        public ReadOnlyReactiveProperty<int> ProjectilePenetrateCountBlessCount => _projectilePenetrateCountBlessCount;
        public ReadOnlyReactiveProperty<int> ProjectileDamageValueBlessCount => _projectileDamageValueBlessCount;
        public ReadOnlyReactiveProperty<int> ProjectileSpeedBlessCount => _projectileSpeedBlessCount;
        public ReadOnlyReactiveProperty<int> ProjectileSizeBlessCount => _projectileSizeBlessCount;

        public ReadOnlyReactiveProperty<int> ProjectileLifeTimeSecondsBlessCount =>
            _projectileLifeTimeSecondsBlessCount;

#endregion

#region Unity Callbacks

        private void Awake() {
            _mainCamera = Camera.main;

            _shootDirection = Vector2.right.DERotate(_shootAngle);

            _basicShootParams.ShootDirectionCount =
                _basicCurves.GetValue(BlessTypes.ShootDirectionCount, 0).ToRoundInt();
            _basicShootParams.ShootAngleRange = _basicCurves.GetValue(BlessTypes.ShootAngleRange, 0);
            _basicShootParams.ShootInterval = _basicCurves.GetValue(BlessTypes.ShootInterval, 0);
            _basicShootParams.ProjectilePenetrateCount =
                _basicCurves.GetValue(BlessTypes.Penetration, 0).ToRoundInt();
            _basicShootParams.ProjectileDamageValue = _basicCurves.GetValue(BlessTypes.ProjectileDamage, 0);
            _basicShootParams.ProjectileSpeed = _basicCurves.GetValue(BlessTypes.ProjectileSpeed, 0);
            _basicShootParams.ProjectileSize = _basicCurves.GetValue(BlessTypes.ProjectileSize, 0);
            _basicShootParams.ProjectileLifeTimeSeconds =
                _basicCurves.GetValue(BlessTypes.ProjectileLifeTimeSeconds, 0);

            _blessedShootParams.ShootDirectionCount =
                _blessCurves.GetValue(BlessTypes.ShootDirectionCount, 0).ToRoundInt();
            _blessedShootParams.ShootAngleRange = _blessCurves.GetValue(BlessTypes.ShootAngleRange, 0);
            _blessedShootParams.ShootInterval = _blessCurves.GetValue(BlessTypes.ShootInterval, 0);
            _blessedShootParams.ProjectilePenetrateCount =
                _blessCurves.GetValue(BlessTypes.Penetration, 0).ToRoundInt();
            _blessedShootParams.ProjectileDamageValue = _blessCurves.GetValue(BlessTypes.ProjectileDamage, 0);
            _blessedShootParams.ProjectileSpeed = _blessCurves.GetValue(BlessTypes.ProjectileSpeed, 0);
            _blessedShootParams.ProjectileSize = _blessCurves.GetValue(BlessTypes.ProjectileSize, 0);
            _blessedShootParams.ProjectileLifeTimeSeconds =
                _blessCurves.GetValue(BlessTypes.ProjectileLifeTimeSeconds, 0);

            InitializeReactiveParams();
        }

        private void Start() {
            GameManager.Instance.GameStateMachine.OnStateChange.Subscribe(pair => {
                if (pair.next is GameStates.InGameState.InGame) {
                    _currentElapsedSecondsFromLastShoot.Value = 0f;
                }
                else {
                    _currentElapsedSecondsFromLastShoot.Value = null;
                }
            }).AddTo(this);

            GameManager.Instance.InGamePhase.Subscribe(phase => { EnhanceBasicParams(phase); }).AddTo(this);
            GameManager.Instance.SelectedBlessType.Subscribe(bless => {
                EnhanceBlessParams(bless);
                ApplyShootParams();
            }).AddTo(this);

            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                _model.color.SetAlpha(pair.next is GameStates.PlayerMode.Stone ? _stoneColorAlpha : 1f);
            }).AddTo(this);
        }

        private void FixedUpdate() {
            if (GameManager.Instance.PlayerModeStateMachine.CurrentState is not GameStates.PlayerMode.Normal) {
                return;
            }

            ShootProcess(Time.fixedDeltaTime);
        }

#endregion

#region Functions

        private void InitializeReactiveParams() {
            _shootDirectionCount =
                new ReactiveProperty<int>(_basicShootParams.ShootDirectionCount +
                                          _blessedShootParams.ShootDirectionCount);
            _shootAngleRange =
                new ReactiveProperty<float>(_basicShootParams.ShootAngleRange + _blessedShootParams.ShootAngleRange);
            _shootIntervalSeconds =
                new ReactiveProperty<float>(_basicShootParams.ShootInterval + _blessedShootParams.ShootInterval);
            _projectilePenetrateCount = new ReactiveProperty<int>(_basicShootParams.ProjectilePenetrateCount +
                                                                  _blessedShootParams.ProjectilePenetrateCount);
            _projectileDamageValue = new ReactiveProperty<float>(_basicShootParams.ProjectileDamageValue +
                                                                 _blessedShootParams.ProjectileDamageValue);
            _projectileSpeed =
                new ReactiveProperty<float>(_basicShootParams.ProjectileSpeed + _blessedShootParams.ProjectileSpeed);
            _projectileSize =
                new ReactiveProperty<float>(_basicShootParams.ProjectileSize * _blessedShootParams.ProjectileSize);
            _projectileLifeTimeSeconds = new ReactiveProperty<float>(_basicShootParams.ProjectileLifeTimeSeconds +
                                                                     _blessedShootParams.ProjectileLifeTimeSeconds);
        }

        private void ApplyShootParams() {
            _shootDirectionCount.Value =
                _basicShootParams.ShootDirectionCount + _blessedShootParams.ShootDirectionCount;
            _shootAngleRange.Value = _basicShootParams.ShootAngleRange + _blessedShootParams.ShootAngleRange;
            _shootIntervalSeconds.Value = _basicShootParams.ShootInterval + _blessedShootParams.ShootInterval;
            _projectilePenetrateCount.Value =
                _basicShootParams.ProjectilePenetrateCount + _blessedShootParams.ProjectilePenetrateCount;
            _projectileDamageValue.Value =
                _basicShootParams.ProjectileDamageValue + _blessedShootParams.ProjectileDamageValue;
            _projectileSpeed.Value = _basicShootParams.ProjectileSpeed + _blessedShootParams.ProjectileSpeed;
            _projectileSize.Value = _basicShootParams.ProjectileSize * _blessedShootParams.ProjectileSize;
            _projectileLifeTimeSeconds.Value =
                _basicShootParams.ProjectileLifeTimeSeconds + _blessedShootParams.ProjectileLifeTimeSeconds;
        }

        private void EnhanceBasicParams(GameStates.Phase phase) {
            var count = (int)phase - 1;
            _basicShootParams.ShootDirectionCount =
                _basicCurves.GetValue(BlessTypes.ShootDirectionCount, count).ToRoundInt();
            _basicShootParams.ShootAngleRange = _basicCurves.GetValue(BlessTypes.ShootAngleRange, count);
            _basicShootParams.ShootInterval = _basicCurves.GetValue(BlessTypes.ShootInterval, count);
            _basicShootParams.ProjectilePenetrateCount =
                _basicCurves.GetValue(BlessTypes.Penetration, count).ToRoundInt();
            _basicShootParams.ProjectileDamageValue = _basicCurves.GetValue(BlessTypes.ProjectileDamage, count);
            _basicShootParams.ProjectileSpeed = _basicCurves.GetValue(BlessTypes.ProjectileSpeed, count);
            _basicShootParams.ProjectileSize = _basicCurves.GetValue(BlessTypes.ProjectileSize, count);
            _basicShootParams.ProjectileLifeTimeSeconds =
                _basicCurves.GetValue(BlessTypes.ProjectileLifeTimeSeconds, count);
        }

        private void EnhanceBlessParams(BlessTypes blessType) {
            switch (blessType) {
                case BlessTypes.None:
                    break;
                case BlessTypes.ShootDirectionCount:
                    _shootDirectionCountBlessCount.Value++;
                    _basicShootParams.ShootDirectionCount = _blessCurves
                        .GetValue(blessType, _shootDirectionCountBlessCount.Value).ToRoundInt();
                    break;
                case BlessTypes.ShootAngleRange:
                    _shootAngleRangeBlessCount.Value++;
                    _basicShootParams.ShootAngleRange =
                        _blessCurves.GetValue(blessType, _shootAngleRangeBlessCount.Value);
                    break;
                case BlessTypes.ShootInterval:
                    _shootIntervalSecondsBlessCount.Value++;
                    _basicShootParams.ShootInterval =
                        _blessCurves.GetValue(blessType, _shootIntervalSecondsBlessCount.Value);
                    break;
                case BlessTypes.Penetration:
                    _projectilePenetrateCountBlessCount.Value++;
                    _basicShootParams.ProjectilePenetrateCount = _blessCurves
                        .GetValue(blessType, _projectilePenetrateCountBlessCount.Value).ToRoundInt();
                    break;
                case BlessTypes.ProjectileDamage:
                    _projectileDamageValueBlessCount.Value++;
                    _basicShootParams.ProjectileDamageValue =
                        _blessCurves.GetValue(blessType, _projectileDamageValueBlessCount.Value);
                    break;
                case BlessTypes.ProjectileSpeed:
                    _projectileSpeedBlessCount.Value++;
                    _basicShootParams.ProjectileSpeed =
                        _blessCurves.GetValue(blessType, _projectileSpeedBlessCount.Value);
                    break;
                case BlessTypes.ProjectileSize:
                    _projectileSizeBlessCount.Value++;
                    _basicShootParams.ProjectileSize =
                        _blessCurves.GetValue(blessType, _projectileSizeBlessCount.Value);
                    break;
                case BlessTypes.ProjectileLifeTimeSeconds:
                    _projectileLifeTimeSecondsBlessCount.Value++;
                    _basicShootParams.ProjectileLifeTimeSeconds =
                        _blessCurves.GetValue(blessType, _projectileLifeTimeSecondsBlessCount.Value);
                    break;
                default:
                    Debug.LogError("未対応のBlessTypeです: " + blessType);
                    break;
            }
        }

        private void ShootProcess(float deltaTime) {
            if (_currentElapsedSecondsFromLastShoot == null) {
                return;
            }

            _currentElapsedSecondsFromLastShoot.Value += deltaTime;

            if (_currentElapsedSecondsFromLastShoot.Value >= _shootIntervalSeconds.Value) {
                _currentElapsedSecondsFromLastShoot.Value = 0f;
                Shoot(new ShootProjectileParams(_shootDirection, _shootDirectionCount.Value, _shootAngleRange.Value));
            }
        }

        private void Shoot(ShootProjectileParams shootProjectileParams) {
            if (shootProjectileParams.DirectionCount == 1) {
                ShootProjectile(shootProjectileParams.Direction);
                return;
            }

            var intervalAngle = shootProjectileParams.AngleRange / (shootProjectileParams.DirectionCount - 1);
            var initialDirection = shootProjectileParams.Direction.DERotate(-shootProjectileParams.AngleRange / 2f);
            for (var i = 0; i < shootProjectileParams.DirectionCount; i++) {
                var direction = initialDirection.DERotate(intervalAngle * i);
                ShootProjectile(direction);
            }
        }

        private void ShootProjectile(Vector2 direction) {
            var newProjectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            var newParams = new ProjectileInitialParams(
                direction,
                _projectilePenetrateCount.Value,
                _projectileDamageValue.Value,
                _projectileSpeed.Value,
                _projectileSize.Value,
                _projectileLifeTimeSeconds.Value
            );
            newProjectile.Initialize(newParams);
        }

#endregion
    }
}
