using System;
using System.Collections.Generic;
using ArtC.Damage;
using ArtC.Systems;
using R3;
using UnityEngine;

namespace ArtC.DeadLine {
    public class DeadLine : MonoBehaviour, IDamageApplicable {
        [SerializeField] private float _maxHitPoint = 1f;

        private readonly List<IAttacker> _tasksInArea = new();
        private ReactiveProperty<float> _currentHitPoint;

        public DamageApplicableTypes DamageApplicableType => DamageApplicableTypes.DeadLine;
        public ReadOnlyReactiveProperty<float> CurrentHitPoint => _currentHitPoint;
        public float MaxHitPoint => _maxHitPoint;
        public bool IsExistingAttackerInArea => _tasksInArea.Count > 0;

        private void Awake() {
            _currentHitPoint = new ReactiveProperty<float>(_maxHitPoint);
        }

        private void Start() {
            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                if (pair.next is not GameStates.PlayerMode.Stone && IsExistingAttackerInArea) {
                    GameManager.Instance.EndInGame(GameStates.ResultReason.DeadLine);
                }
            }).AddTo(this);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.attachedRigidbody == null) {
                return;
            }

            var attacker = other.attachedRigidbody.GetComponent<IAttacker>();

            if (attacker == null) {
                return;
            }

            if (attacker.Type is not AttackerTypes.YourTask) {
                return;
            }

            _tasksInArea.Remove(attacker);
        }

        public void ApplyDamage(DamageData damageData) {
            if (damageData.Attacker.Type is not AttackerTypes.YourTask) {
                return;
            }

            if (GameManager.Instance.PlayerModeStateMachine.CurrentState is GameStates.PlayerMode.Stone) {
                _tasksInArea.Add(damageData.Attacker);
                return;
            }

            GameManager.Instance.EndInGame(GameStates.ResultReason.DeadLine);
        }
    }
}
