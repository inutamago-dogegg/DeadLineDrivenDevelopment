using System;
using ArtC.Systems;
using R3;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ArtC.GUI {
    public class VolumeChanger : MonoBehaviour {
        [SerializeField] private Volume _globalVolume;

        private Vignette _vignette;

        private void Awake() {
            _globalVolume.profile.TryGet(out _vignette);
            _vignette.active = false;
        }

        private void Start() {
            GameManager.Instance.PlayerModeStateMachine.OnStateChange
                .Subscribe(pair => { _vignette.active = pair.next is GameStates.PlayerMode.Stone; }).AddTo(this);
        }
    }
}
