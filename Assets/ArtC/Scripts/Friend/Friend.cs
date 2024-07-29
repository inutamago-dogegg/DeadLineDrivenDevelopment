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

        private Vector2 _shootDirection = Vector2.up;
        private readonly ReactiveProperty<float?> _currentElapsedSecondsFromLastShoot = new(null);

#endregion

#region Properties

        public float ShootAngle => _shootAngle;

#endregion

#region Unity Callbacks

        private void Awake() {
            _shootDirection = Vector2.right.DERotate(_shootAngle);
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

        private void ShootProcess(float deltaTime) {
            if (_currentElapsedSecondsFromLastShoot == null) {
                return;
            }

            _currentElapsedSecondsFromLastShoot.Value += deltaTime;

            var friendShootParams = GameManager.Instance.FriendShootParams;
            if (_currentElapsedSecondsFromLastShoot.Value >= friendShootParams.ShootInterval) {
                _currentElapsedSecondsFromLastShoot.Value = 0f;
                Shoot(new ShootProjectileParams(_shootDirection, friendShootParams.ShootDirectionCount, friendShootParams.ShootAngleRange));
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
            var friendParams = GameManager.Instance.FriendShootParams;
            var newParams = new ProjectileInitialParams(
                direction,
                friendParams.ProjectilePenetrateCount,
                friendParams.ProjectileDamageValue,
                friendParams.ProjectileSpeed,
                friendParams.ProjectileSize,
                friendParams.ProjectileLifeTimeSeconds
            );
            newProjectile.Initialize(newParams);
        }

#endregion
    }
}
