using ArtC.Utils;
using UnityEditor;
using UnityEngine;

namespace ArtC.Friend.Editor {
    public class EnemyGizmoEditor {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected, typeof(Friend))]
        private static void DrawGizmo(Friend friend, GizmoType gizmoType) {
            // ギズモの描画処理
            Gizmos.color = Color.yellow;

            var pos = friend.transform.position;
            var direction = (Vector3)Vector2.right.DERotate(friend.ShootAngle);
            DrawArrow.ForGizmo(pos, direction, Color.yellow, 0.2f);
        }
    }
}
