using System;
using ArtC.Systems;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArtC.GUI {
    public class PhaseDisplay : MonoBehaviour {
        [SerializeField] private TMP_Text _phaseText;
        [SerializeField] private Image _phaseRemainingTimer;

        private void Start() {
            GameManager.Instance.InGamePhase.Subscribe(newPhase => { _phaseText.text = newPhase.ToJapanese(); })
                .AddTo(this);

            GameManager.Instance.PhaseElapsedSeconds.Subscribe(elapsedSeconds => {
                var phaseDuration = GameManager.Instance.PhaseDurationSeconds;
                _phaseRemainingTimer.fillAmount = 1f - elapsedSeconds / phaseDuration;
            }).AddTo(this);
        }
    }
}
