using System;
using UnityEngine;

namespace ArtC.PlayerCharacter.Abilities {
    public class PlayerCharacterWalk : PlayerCharacterAbilityBase {
        [SerializeField] private Rigidbody2D _playerCharacterRigidbody2D = null;

        [Header("パラメーター")] [SerializeField] private float _walkZeroThreshold = 0.1f;
        [SerializeField] private float _dashThreshold = 5f;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _dashSpeed = 6f;

        private Camera _mainCamera;
        private Vector2 _walkVector = Vector2.zero;

        private void Awake() {
            _mainCamera = Camera.main;
        }

        private void Update() {
            _walkVector = ComputeWalkVector();
        }

        private void FixedUpdate() {
            Walk();
        }

        private Vector2 ComputeWalkVector() {
            var mousePosition = (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var currentPosition = (Vector2)transform.position;

            var walkVector = mousePosition - currentPosition;
            var sqrMagnitude = walkVector.sqrMagnitude;

            if (sqrMagnitude < _walkZeroThreshold * _walkZeroThreshold) {
                walkVector = Vector2.zero;
            }
            else if (sqrMagnitude < _dashThreshold * _dashThreshold) {
                walkVector = walkVector.normalized * _walkSpeed;
            }
            else {
                walkVector = walkVector.normalized * _dashSpeed;
            }

            return walkVector;
        }

        private void Walk() {
            _playerCharacterRigidbody2D.velocity = _walkVector;
        }
    }
}
