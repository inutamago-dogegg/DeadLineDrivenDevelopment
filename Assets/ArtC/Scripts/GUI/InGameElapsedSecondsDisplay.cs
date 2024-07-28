using System;
using ArtC.Systems;
using TMPro;
using UnityEngine;
using R3;

namespace ArtC.GUI {
    public class InGameElapsedSecondsDisplay : MonoBehaviour {
        [SerializeField] private TMP_Text _elapsedSecondsText;

        private void Start() {
            GameManager.Instance.InGameElapsedSeconds.Subscribe(seconds => {
                _elapsedSecondsText.text = $"{seconds:F2}";
            }).AddTo(this);
        }
    }
}
