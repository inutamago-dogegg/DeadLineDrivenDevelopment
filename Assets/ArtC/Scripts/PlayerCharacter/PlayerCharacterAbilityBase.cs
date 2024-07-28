using UnityEngine;

namespace ArtC.PlayerCharacter {
    public abstract class PlayerCharacterAbilityBase : MonoBehaviour {
        protected PlayerCharacterCore Core { get; private set; }

        public void InitializeCore(PlayerCharacterCore core) {
            Core = core;
        }
    }
}
