using System;
using ArtC.Systems;
using R3;
using UnityEngine;

namespace ArtC.PlayerCharacter.Abilities {
    public class PlayerCharacterAnimation : PlayerCharacterAbilityBase {
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;

        [Header("パラメーター")] [SerializeField] private Color _playerCharacterNormalColor = Color.cyan;
        [SerializeField] private Color _playerCharacterStoneColor = Color.gray;

        private void Awake() {
            _playerSpriteRenderer.color = _playerCharacterNormalColor;
        }

        private void Start() {
            GameManager.Instance.PlayerModeStateMachine.OnStateChange.Subscribe(pair => {
                _playerSpriteRenderer.color = pair.next switch {
                    GameStates.PlayerMode.Normal => _playerCharacterNormalColor,
                    GameStates.PlayerMode.Stone => _playerCharacterStoneColor,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }).AddTo(this);
        }
    }
}
