using System;
using ArtC.Systems;
using R3;
using TMPro;
using UnityEngine;

namespace ArtC.GUI {
    public class PlayerModeDisplay : MonoBehaviour {
        [SerializeField] private TMP_Text _playerModeText;
        [SerializeField] private GameObject _onlyInStoneModeObjectsParent;
        [SerializeField] private TMP_Text _ignoreDeadLineText;

        private DeadLine.DeadLine _deadLine;

        private void Awake() {
            _playerModeText.text = $"{GameStates.PlayerMode.Normal.ToJapanese()}";

            _onlyInStoneModeObjectsParent.SetActive(false);
        }

        private void Start() {
            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                _playerModeText.text = $"{pair.next.ToJapanese()}";
            }).AddTo(this);

            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                var isStoneMode = pair.next is GameStates.PlayerMode.Stone;
                _onlyInStoneModeObjectsParent.SetActive(isStoneMode);
            }).AddTo(this);

            _deadLine = GameManager.Instance.DeadLine;
        }

        private void Update() {
            _ignoreDeadLineText.gameObject.SetActive(_deadLine.IsExistingAttackerInArea);
        }
    }
}
