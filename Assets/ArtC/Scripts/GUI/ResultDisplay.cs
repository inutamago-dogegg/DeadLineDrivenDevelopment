using System;
using ArtC.Systems;
using UnityEngine;
using R3;

namespace ArtC.GUI {
    public class ResultDisplay : MonoBehaviour {
        [SerializeField] private GameObject _graduateContent;
        [SerializeField] private GameObject _deadLineContent;

        private void Awake() {
            _graduateContent.SetActive(false);
            _deadLineContent.SetActive(false);
        }

        private void Start() {
            GameManager.Instance.GameStateMachine.OnStateChange
                .Where(pair => pair.next is GameStates.InGameState.Result)
                .Subscribe(_ => {
                    var resultReason = GameManager.Instance.ResultData.Reason;

                    switch (resultReason) {
                        case GameStates.ResultReason.Graduate:
                            DisplayGraduateResult();
                            break;
                        case GameStates.ResultReason.DeadLine:
                            DisplayDeadLineResult();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                })
                .AddTo(this);
        }

        private void DisplayGraduateResult() {
            Debug.Log("ResultDisplay: DisplayGraduateResult");
            _graduateContent.SetActive(true);
        }

        private void DisplayDeadLineResult() {
            Debug.Log("ResultDisplay: DisplayDeadLineResult");
            _deadLineContent.SetActive(true);
        }
    }
}
