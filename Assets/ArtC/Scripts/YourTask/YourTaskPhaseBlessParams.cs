using System;
using ArtC.Systems;
using ArtC.Utils;
using UnityEngine;

namespace ArtC.YourTask {
    [Serializable]
    public record YourTaskPhaseBlessParams {
        [SerializeField] private AnimationCurve _spawnIntervalCurve =
            AnimationCurve.Linear(0f, 1f, GameStates.Phase.Count.ToInt(), 0.1f);

        [SerializeField]
        private AnimationCurve _speedCurve = AnimationCurve.Linear(0f, 3f, GameStates.Phase.Count.ToInt(), 10f);

        [SerializeField]
        private AnimationCurve _maxHitPointCurve = AnimationCurve.Linear(0f, 7f, GameStates.Phase.Count.ToInt(), 50f);

        public float GetSpawnInterval(GameStates.Phase phase) {
            return _spawnIntervalCurve.Evaluate(phase.ToInt());
        }

        public float GetSpeed(GameStates.Phase phase) {
            return _speedCurve.Evaluate(phase.ToInt());
        }

        public float GetMaxHitPoint(GameStates.Phase phase) {
            return _maxHitPointCurve.Evaluate(phase.ToInt());
        }
    }
}
