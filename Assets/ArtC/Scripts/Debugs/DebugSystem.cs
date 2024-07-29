using System;
using System.Text;
using ArtC.PlayerCharacter;
using ArtC.PlayerCharacter.Abilities;
using TMPro;
using UnityEngine;

namespace ArtC.Debugs {
    public class DebugSystem : MonoBehaviour {
        [SerializeField] private TMP_Text _debugText;
        private PlayerCharacterShoot _playerCharacterShoot;
        private ShootParams _basicShootParams;
        private ShootParams _blessedShootParams;
        private readonly StringBuilder _debugStringBuilder = new();

        private void Awake() {
            _playerCharacterShoot = FindObjectOfType<PlayerCharacterShoot>();
            _basicShootParams = _playerCharacterShoot.BasicShootParams;
            _blessedShootParams = _playerCharacterShoot.BlessedShootParams;
        }

        private void Update() {
            UpdateDebugText();
        }

        private void UpdateDebugText() {
            _debugStringBuilder.Clear();
            _debugStringBuilder.AppendLine($"ShootDirectionCount: {_playerCharacterShoot.ShootDirectionCount.CurrentValue}");
            _debugStringBuilder.AppendLine($"ShootAngleRange: {_playerCharacterShoot.ShootAngleRange.CurrentValue}");
            _debugStringBuilder.AppendLine($"ShootIntervalSeconds: {_playerCharacterShoot.ShootIntervalSeconds.CurrentValue}");
            _debugStringBuilder.AppendLine($"ProjectilePenetrateCount: {_playerCharacterShoot.ProjectilePenetrateCount.CurrentValue}");
            _debugStringBuilder.AppendLine($"ProjectileDamageValue: {_playerCharacterShoot.ProjectileDamageValue.CurrentValue}");
            _debugStringBuilder.AppendLine($"ProjectileSpeed: {_playerCharacterShoot.ProjectileSpeed.CurrentValue}");
            _debugStringBuilder.AppendLine($"ProjectileSize: {_playerCharacterShoot.ProjectileSize.CurrentValue}");
            _debugStringBuilder.AppendLine($"ProjectileLifeTimeSeconds: {_playerCharacterShoot.ProjectileLifeTimeSeconds.CurrentValue}");

            _debugStringBuilder.AppendLine();
            _debugStringBuilder.AppendLine($"BasicShootParams - ShootDirectionCount: {_basicShootParams.ShootDirectionCount}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ShootAngleRange: {_basicShootParams.ShootAngleRange}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ShootIntervalSeconds: {_basicShootParams.ShootInterval}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ProjectilePenetrateCount: {_basicShootParams.ProjectilePenetrateCount}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ProjectileDamageValue: {_basicShootParams.ProjectileDamageValue}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ProjectileSpeed: {_basicShootParams.ProjectileSpeed}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ProjectileSize: {_basicShootParams.ProjectileSize}");
            _debugStringBuilder.AppendLine($"BasicShootParams - ProjectileLifeTimeSeconds: {_basicShootParams.ProjectileLifeTimeSeconds}");

            _debugStringBuilder.AppendLine();
            _debugStringBuilder.AppendLine($"BlessedShootParams - ShootDirectionCount: {_blessedShootParams.ShootDirectionCount}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ShootAngleRange: {_blessedShootParams.ShootAngleRange}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ShootIntervalSeconds: {_blessedShootParams.ShootInterval}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ProjectilePenetrateCount: {_blessedShootParams.ProjectilePenetrateCount}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ProjectileDamageValue: {_blessedShootParams.ProjectileDamageValue}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ProjectileSpeed: {_blessedShootParams.ProjectileSpeed}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ProjectileSize: {_blessedShootParams.ProjectileSize}");
            _debugStringBuilder.AppendLine($"BlessedShootParams - ProjectileLifeTimeSeconds: {_blessedShootParams.ProjectileLifeTimeSeconds}");

            _debugText.text = _debugStringBuilder.ToString();
        }
    }
}
