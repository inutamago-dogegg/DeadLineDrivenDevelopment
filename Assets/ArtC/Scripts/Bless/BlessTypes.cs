namespace ArtC.Bless {
    public enum BlessTypes {
        None = -1,
        ShootDirectionCount,
        ShootAngleRange,
        ShootInterval,
        Penetration,
        ProjectileDamage,
        ProjectileSpeed,
        ProjectileSize,
        ProjectileLifeTimeSeconds
    }

    public static class BlessTypesExtension {
        public static string ToJapanese(this BlessTypes blessType) {
            return blessType switch {
                BlessTypes.ShootDirectionCount => "軌道の数",
                BlessTypes.ShootAngleRange => "発射角度の範囲",
                BlessTypes.ShootInterval => "発射間隔",
                BlessTypes.Penetration => "貫通",
                BlessTypes.ProjectileDamage => "弾のダメージ",
                BlessTypes.ProjectileSpeed => "弾の速さ",
                BlessTypes.ProjectileSize => "弾の大きさ",
                BlessTypes.ProjectileLifeTimeSeconds => "弾の寿命",
                _ => "不明"
            };
        }
    }
}
