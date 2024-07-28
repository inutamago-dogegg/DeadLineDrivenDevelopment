using System;
using ArtC.Utils;
using ArtC.YourTask;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArtC.Systems {
    public class YourTaskSpawner : SingletonMonoBehaviour<YourTaskSpawner> {
        [SerializeField] private YourTask.YourTask _yourTaskPrefab;

        [Header("パラメーター")] [SerializeField] private YourTaskPhaseBlessParams _yourTaskPhaseBlessParams;
        [SerializeField] private Vector2 _spawnRadiusRange = new(10f, 15f);
        [SerializeField] private float _yourTaskSpeedScatter = 0.5f;
        [SerializeField] private float _yourTaskMaxHitPointScatter = 0.5f;
        [SerializeField] private float _currentSpawnInterval;
        [SerializeField] private float _currentSpeed;
        [SerializeField] private float _currentMaxHitPoint;

        private float _elapsedSpawnInterval = 0f;

        protected override void Awake() {
            base.Awake();

            _currentSpawnInterval = _yourTaskPhaseBlessParams.GetSpawnInterval(GameStates.Phase.Bachelor1_1Q);
            _currentSpeed = _yourTaskPhaseBlessParams.GetSpeed(GameStates.Phase.Bachelor1_1Q);
            _currentMaxHitPoint = _yourTaskPhaseBlessParams.GetMaxHitPoint(GameStates.Phase.Bachelor1_1Q);
        }

        private void Start() {
            GameManager.Instance.InGamePhase.Subscribe(phase => {
                _currentSpawnInterval = _yourTaskPhaseBlessParams.GetSpawnInterval(phase);
                _currentSpeed = _yourTaskPhaseBlessParams.GetSpeed(phase);
                _currentMaxHitPoint = _yourTaskPhaseBlessParams.GetMaxHitPoint(phase);
            }).AddTo(this);
        }

        private void FixedUpdate() {
            if (GameManager.Instance.GameStateMachine.CurrentState is not GameStates.InGameState.InGame) {
                return;
            }

            _elapsedSpawnInterval += Time.fixedDeltaTime;
            if (_elapsedSpawnInterval >= _currentSpawnInterval) {
                SpawnYourTask();
                _elapsedSpawnInterval = 0f;
            }
        }

        private void SpawnYourTask() {
            // 画面の外側の周囲のランダムな位置に生成
            var randomRadius = Random.Range(_spawnRadiusRange.x, _spawnRadiusRange.y);
            var randomAngle = Random.Range(0f, 360f);
            var spawnPosition = new Vector2(
                Mathf.Cos(randomAngle * Mathf.Deg2Rad) * randomRadius,
                Mathf.Sin(randomAngle * Mathf.Deg2Rad) * randomRadius
            );
            var newYourTask = Instantiate(_yourTaskPrefab, spawnPosition, Quaternion.identity);

            var speedScatter = Random.Range(-_yourTaskSpeedScatter, _yourTaskSpeedScatter);
            var speed = _currentSpeed + speedScatter;

            var maxHitPointScatter = Random.Range(-_yourTaskMaxHitPointScatter, _yourTaskMaxHitPointScatter);
            var maxHitPoint = _currentMaxHitPoint + maxHitPointScatter;
            newYourTask.Initialize(new YourTaskInitialParams(spawnPosition, speed, maxHitPoint));
        }
    }
}
