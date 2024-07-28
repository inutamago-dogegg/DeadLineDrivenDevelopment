using System;
using ArtC.Bless;
using ArtC.Systems;
using ArtC.Utils;
using UnityEngine;

namespace ArtC.PlayerCharacter {
    [Serializable]
    public record ShootParamEnhanceCurves {
        [SerializeField] private AnimationCurve _shootDirectionCountCurve =
            AnimationCurve.Linear(0f, 1f, GameStates.Phase.Count.ToInt(), GameStates.Phase.Count.ToInt());

        [SerializeField] private AnimationCurve _shootAngleRangeCurve =
            AnimationCurve.Linear(0f, 30f, GameStates.Phase.Count.ToInt(), 90f);

        [SerializeField] private AnimationCurve _shootIntervalSecondsCurve =
            AnimationCurve.Linear(0f, 0.6f, GameStates.Phase.Count.ToInt(), 0.2f);

        [SerializeField] private AnimationCurve _projectilePenetrateCountCurve =
            AnimationCurve.Linear(0f, 1f, GameStates.Phase.Count.ToInt(), GameStates.Phase.Count.ToInt());

        [SerializeField] private AnimationCurve _projectileDamageValueCurve =
            AnimationCurve.Linear(0f, 3f, GameStates.Phase.Count.ToInt(), 20f);

        [SerializeField] private AnimationCurve _projectileSpeedCurve =
            AnimationCurve.Linear(0f, 5f, GameStates.Phase.Count.ToInt(), 20f);

        [SerializeField] private AnimationCurve _projectileSizeCurve =
            AnimationCurve.Linear(0f, 1.3f, GameStates.Phase.Count.ToInt(), 3f);

        [SerializeField] private AnimationCurve _projectileLifeTimeSecondsCurve =
            AnimationCurve.Linear(0f, 1f, GameStates.Phase.Count.ToInt(), 5f);

        public float GetValue(BlessTypes blessType, int blessCount) {
            return blessType switch {
                BlessTypes.ShootDirectionCount => _shootDirectionCountCurve.Evaluate(blessCount),
                BlessTypes.ShootAngleRange => _shootAngleRangeCurve.Evaluate(blessCount),
                BlessTypes.ShootInterval => _shootIntervalSecondsCurve.Evaluate(blessCount),
                BlessTypes.Penetration => _projectilePenetrateCountCurve.Evaluate(blessCount),
                BlessTypes.ProjectileDamage => _projectileDamageValueCurve.Evaluate(blessCount),
                BlessTypes.ProjectileSpeed => _projectileSpeedCurve.Evaluate(blessCount),
                BlessTypes.ProjectileSize => _projectileSizeCurve.Evaluate(blessCount),
                BlessTypes.ProjectileLifeTimeSeconds => _projectileLifeTimeSecondsCurve.Evaluate(blessCount),
                BlessTypes.None => throw new ArgumentOutOfRangeException(nameof(blessType), blessType, null),
                _ => throw new ArgumentOutOfRangeException(nameof(blessType), blessType, null)
            };
        }
    }
}
