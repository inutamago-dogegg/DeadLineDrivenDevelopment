using System;
using ArtC.Systems;
using UnityEngine;

namespace ArtC.PlayerCharacter.Abilities {
    public class PlayerCharacterModeChange : PlayerCharacterAbilityBase {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space) &&
                GameManager.Instance.GameStateMachine.CurrentState == GameStates.InGameState.InGame) {
                ModeChange();
            }
        }

        private void ModeChange() {
            if (GameManager.Instance.PlayerModeStateMachine.CurrentState == GameStates.PlayerMode.Normal) {
                GameManager.Instance.PlayerModeStateMachine.ChangeState(GameStates.PlayerMode.Stone);
            }
            else {
                GameManager.Instance.PlayerModeStateMachine.ChangeState(GameStates.PlayerMode.Normal);
            }

            Debug.Log($"Game Mode Changed to {GameManager.Instance.PlayerModeStateMachine.CurrentState}!");
        }
    }
}
