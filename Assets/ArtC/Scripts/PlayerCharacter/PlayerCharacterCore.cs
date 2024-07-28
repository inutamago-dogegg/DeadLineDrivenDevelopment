using ArtC.Systems;
using ArtC.Utils;
using UnityEngine;

namespace ArtC.PlayerCharacter {
    public class PlayerCharacterCore : MonoBehaviour {
        private PlayerCharacterAbilityBase[] _abilities;

        private void Awake() {
            Initialize();
        }

        public PlayerCharacterAbilityBase FindAbility<T>() where T : PlayerCharacterAbilityBase {
            foreach (var ability in _abilities) {
                if (ability is T) {
                    return ability;
                }
            }

            Debug.LogWarning($"PlayerCharacterAbility of type {typeof(T)} not found!");
            return null;
        }

        private void Initialize() {
            _abilities = GetComponentsInChildren<PlayerCharacterAbilityBase>();

            foreach (var ability in _abilities) {
                ability.InitializeCore(this);
            }
        }
    }
}
