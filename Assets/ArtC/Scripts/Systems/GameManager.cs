using System;
using System.Collections.Generic;
using ArtC.Bless;
using ArtC.Utils;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArtC.Systems {
    public class GameManager : SingletonMonoBehaviour<GameManager> {
#region Fields

        [SerializeField] private List<GameObject> _awakeActiveObjects;
        [SerializeField] private DeadLine.DeadLine _deadLine;
        [SerializeField] private List<BlessCard> _blessCards;

        [Header("パラメーター")] [SerializeField] private BlessWeights _blessWeights;
        [SerializeField] private float _phaseDurationSeconds = 10f;

        private StateMachine<GameStates.PlayerMode> _playerModeStateMachine;
        private StateMachine<GameStates.InGameState> _gameStateMachine;
        private readonly AliasMethod _blessGacha = new();
        private readonly ReactiveProperty<float> _stoneModeElapsedSeconds = new(0f);
        private readonly ReactiveProperty<float> _inGameElapsedSeconds = new(0f);
        private readonly ReactiveProperty<float> _inGameNotStoneElapsedSeconds = new(0f);
        private readonly ReactiveProperty<GameStates.Phase> _inGamePhase = new(GameStates.Phase.Bachelor1_1Q);
        private readonly ReactiveProperty<float> _phaseElapsedSeconds = new(0f);

        public readonly Subject<BlessTypes> SelectedBlessType = new();

#endregion

#region Properties

        public ResultData ResultData { get; private set; }
        public StateMachine<GameStates.PlayerMode> PlayerModeStateMachine => _playerModeStateMachine;
        public StateMachine<GameStates.InGameState> GameStateMachine => _gameStateMachine;
        public ReadOnlyReactiveProperty<float> StoneModeElapsedSeconds => _stoneModeElapsedSeconds;
        public ReadOnlyReactiveProperty<float> InGameElapsedSeconds => _inGameElapsedSeconds;
        public ReadOnlyReactiveProperty<float> InGameNotStoneElapsedSeconds => _inGameNotStoneElapsedSeconds;
        public ReadOnlyReactiveProperty<GameStates.Phase> InGamePhase => _inGamePhase;
        public ReadOnlyReactiveProperty<float> PhaseElapsedSeconds => _phaseElapsedSeconds;
        public DeadLine.DeadLine DeadLine => _deadLine;
        public float PhaseDurationSeconds => _phaseDurationSeconds;
        public BlessWeights BlessWeights => _blessWeights;

#endregion

#region Unity Callbacks

        protected override void Awake() {
            base.Awake();
            Initialize();

            foreach (var obj in _awakeActiveObjects) {
                obj.SetActive(true);
            }
        }

        private void Start() {
            _playerModeStateMachine.OnStateChange
                .DelayFrame(2, UnityFrameProvider.Update)
                .Subscribe(pair => {
                    if (pair.next is not GameStates.PlayerMode.Stone && _stoneModeElapsedSeconds.Value > 0f) {
                        _stoneModeElapsedSeconds.Value = 0f;
                    }
                }).AddTo(this);

            _phaseElapsedSeconds
                .Where(seconds => seconds >= _phaseDurationSeconds)
                .Subscribe(_ => { TransitPhase(); }).AddTo(this);

            _inGamePhase.Subscribe(_ => { StartBlessing(); }).AddTo(this);

            SelectedBlessType.Subscribe(type => {
                if (type == BlessTypes.None) {
                    return;
                }

                EndBlessing();
            }).AddTo(this);

            _gameStateMachine.OnStateChange
                .Subscribe(pair => {
                    switch (pair.next) {
                        case GameStates.InGameState.Title:
                            break;
                        case GameStates.InGameState.InGame:
                            StartInGame();
                            break;
                        case GameStates.InGameState.Result:
                            break;
                        case GameStates.InGameState.Blessing:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }).AddTo(this);
        }

        private void OnDestroy() {
            _playerModeStateMachine.Dispose();
            _gameStateMachine.Dispose();
        }

        private void Update() {
            if (_playerModeStateMachine.CurrentState is GameStates.PlayerMode.Stone) {
                _stoneModeElapsedSeconds.Value += Time.deltaTime;
            }

            if (_gameStateMachine.CurrentState is GameStates.InGameState.Result && Input.GetKeyDown(KeyCode.R)) {
                Restart();
            }
        }

        private void FixedUpdate() {
            if (_gameStateMachine.CurrentState is GameStates.InGameState.InGame) {
                _inGameElapsedSeconds.Value += Time.fixedDeltaTime;
                _phaseElapsedSeconds.Value += Time.fixedDeltaTime;

                if (_playerModeStateMachine.CurrentState is not GameStates.PlayerMode.Stone) {
                    _inGameNotStoneElapsedSeconds.Value += Time.fixedDeltaTime;
                }
            }
        }

#endregion

#region Functions

        private void Initialize() {
            _playerModeStateMachine = new StateMachine<GameStates.PlayerMode>(gameObject, GameStates.PlayerMode.Normal);
            _gameStateMachine = new StateMachine<GameStates.InGameState>(gameObject, GameStates.InGameState.Title);
            _blessGacha.Constructor(_blessWeights.Weights);
        }

        private void TransitPhase() {
            Debug.Log("Transit Phase!");
            _phaseElapsedSeconds.Value = 0f;

            if (_inGamePhase.Value.IsLastPhase()) {
                EndInGame(GameStates.ResultReason.Graduate);
            }
            else {
                _inGamePhase.Value = _inGamePhase.Value.Next();
            }
        }

        private void StartInGame() {
            Debug.Log("InGame Start!");
            _gameStateMachine.ChangeState(GameStates.InGameState.InGame);
            Time.timeScale = 1f;
            _inGameElapsedSeconds.Value = 0f;
        }

        public void EndInGame(GameStates.ResultReason reason) {
            Debug.Log($"InGame End! Reason: {reason}");

            ResultData = new ResultData(reason);
            _gameStateMachine.ChangeState(GameStates.InGameState.Result);
            Time.timeScale = 0f;
        }

        private void StartBlessing() {
            foreach (var blessCard in _blessCards) {
                ApplyRandomBless(blessCard);
                blessCard.gameObject.SetActive(true);
            }

            Debug.Log("Blessing Start!");
            _gameStateMachine.ChangeState(GameStates.InGameState.Blessing);
            Time.timeScale = 0f;
            return;

            void ApplyRandomBless(BlessCard blessCard) {
                _blessGacha.Constructor(_blessWeights.Weights);
                var randomBless = _blessWeights.Types[_blessGacha.Roll()];

                blessCard.SetBlessType(randomBless);
                blessCard.SetBlessDescription(randomBless.ToJapanese());
            }
        }

        private void EndBlessing() {
            Debug.Log("Blessing End!");

            foreach (var blessCard in _blessCards) {
                blessCard.gameObject.SetActive(false);
            }

            _gameStateMachine.ChangeState(GameStates.InGameState.InGame);
            Time.timeScale = 1f;
        }

        private void Restart() {
            Debug.Log("Restart!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
            _inGameElapsedSeconds.Value = 0f;
        }

#endregion
    }
}
