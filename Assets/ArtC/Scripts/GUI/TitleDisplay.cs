using System;
using ArtC.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace ArtC.GUI {
    public class TitleDisplay : MonoBehaviour {
        [SerializeField] private Button _startButton;
        [SerializeField] private RectTransform _content;

        private void Start() {
            _startButton.onClick.AddListener(() => {
                Debug.Log("StartButton: クリックされました");
                GameManager.Instance.GameStateMachine.ChangeState(GameStates.InGameState.InGame);
                _content.gameObject.SetActive(false);
            });
        }
    }
}
