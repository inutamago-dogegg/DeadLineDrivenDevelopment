using System;

namespace ArtC.Systems {
    public abstract class GameStates {
        public enum PlayerMode {
            Normal,
            Stone
        }

        public enum InGameState {
            Title,
            InGame,
            Result,
            Blessing
        }

        public enum Phase {
            Bachelor1_1Q = 0,
            Bachelor1_2Q,
            Bachelor1_3Q,
            Bachelor1_4Q,
            Bachelor2_1Q,
            Bachelor2_2Q,
            Bachelor2_3Q,
            Bachelor2_4Q,
            Bachelor3_1Q,
            Bachelor3_2Q,
            Bachelor3_3Q,
            Bachelor3_4Q,
            Bachelor4_1Q,
            Bachelor4_2Q,
            Bachelor4_3Q,
            Bachelor4_4Q,
            Master1_1Q,
            Master1_2Q,
            Master1_3Q,
            Master1_4Q,
            Master2_1Q,
            Master2_2Q,
            Master2_3Q,
            Master2_4Q,
            Count
        }

        public enum ResultReason {
            DeadLine,
            Graduate
        }
    }

    public static class GameStatesExtension {
        public static string ToJapanese(this GameStates.PlayerMode playerMode) {
            return playerMode switch {
                GameStates.PlayerMode.Normal => "通常",
                GameStates.PlayerMode.Stone => "石ころ",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static string ToJapanese(this GameStates.Phase phase) {
            return phase switch {
                GameStates.Phase.Bachelor1_1Q => "学士1年1Q",
                GameStates.Phase.Bachelor1_2Q => "学士1年2Q",
                GameStates.Phase.Bachelor1_3Q => "学士1年3Q",
                GameStates.Phase.Bachelor1_4Q => "学士1年4Q",
                GameStates.Phase.Bachelor2_1Q => "学士2年1Q",
                GameStates.Phase.Bachelor2_2Q => "学士2年2Q",
                GameStates.Phase.Bachelor2_3Q => "学士2年3Q",
                GameStates.Phase.Bachelor2_4Q => "学士2年4Q",
                GameStates.Phase.Bachelor3_1Q => "学士3年1Q",
                GameStates.Phase.Bachelor3_2Q => "学士3年2Q",
                GameStates.Phase.Bachelor3_3Q => "学士3年3Q",
                GameStates.Phase.Bachelor3_4Q => "学士3年4Q",
                GameStates.Phase.Bachelor4_1Q => "学士4年1Q",
                GameStates.Phase.Bachelor4_2Q => "学士4年2Q",
                GameStates.Phase.Bachelor4_3Q => "学士4年3Q",
                GameStates.Phase.Bachelor4_4Q => "学士4年4Q",
                GameStates.Phase.Master1_1Q => "修士1年1Q",
                GameStates.Phase.Master1_2Q => "修士1年2Q",
                GameStates.Phase.Master1_3Q => "修士1年3Q",
                GameStates.Phase.Master1_4Q => "修士1年4Q",
                GameStates.Phase.Master2_1Q => "修士2年1Q",
                GameStates.Phase.Master2_2Q => "修士2年2Q",
                GameStates.Phase.Master2_3Q => "修士2年3Q",
                GameStates.Phase.Master2_4Q => "修士2年4Q",
                _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
            };
        }

        public static GameStates.Phase Next(this GameStates.Phase phase) {
            var currentInt = (int)phase;
            var nextInt = currentInt + 1;
            const int maxInt = (int)GameStates.Phase.Count - 1;
            if (currentInt == maxInt) {
                throw new ArgumentOutOfRangeException();
            }

            return (GameStates.Phase)nextInt;
        }

        public static bool IsBachelor(this GameStates.Phase phase) {
            const string bachelorString = "Bachelor";
            return phase.ToString().Contains(bachelorString);
        }

        public static bool IsMaster(this GameStates.Phase phase) {
            const string masterString = "Master";
            return phase.ToString().Contains(masterString);
        }

        public static bool IsLastPhase(this GameStates.Phase phase) {
            var phaseInt = (int)phase;
            return phaseInt == (int)GameStates.Phase.Count - 1;
        }
    }
}
