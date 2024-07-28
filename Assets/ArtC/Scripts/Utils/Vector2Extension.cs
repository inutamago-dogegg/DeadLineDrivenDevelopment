using System;
using UnityEngine;

namespace ArtC.Utils {
    public static class Vector2Extension {
        /// <summary>
        /// ベクトルを反時計回りに90度回す
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>ベクトルを反時計回りに90度回したベクトル</returns>
        public static Vector2 DEPerpendicular(this Vector2 vector2) {
            var x = vector2.x;
            var y = vector2.y;
            vector2.x = -y;
            vector2.y = x;
            return vector2;
        }

        /// <summary>
        /// ベクトルを時計回りに90度回す
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>ベクトルを時計回りに90度回したベクトル</returns>
        public static Vector2 DEPerpendicularClockwise(this Vector2 vector2) {
            var x = vector2.x;
            var y = vector2.y;
            vector2.x = y;
            vector2.y = -x;
            return vector2;
        }

        /// <summary>
        /// ベクトルを反時計回りに回す
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <param name="angleInDegree">回す角度(度数法)</param>
        /// <returns>回転後のベクトル</returns>
        public static Vector2 DERotate(this Vector2 vector2, float angleInDegree) {
            var rad = angleInDegree * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var x = vector2.x;
            var y = vector2.y;
            vector2.x = x * cos - y * sin;
            vector2.y = x * sin + y * cos;
            return vector2;
        }

        /// <summary>
        /// ベクトルを時計回りに回す
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <param name="angleInDegree">回す角度(度数法)</param>
        /// <returns>回転後のベクトル</returns>
        public static Vector2 DERotateClockWise(this Vector2 vector2, float angleInDegree) {
            var rad = angleInDegree * Mathf.Deg2Rad;
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);
            var x = vector2.x;
            var y = vector2.y;
            vector2.x = x * cos + y * sin;
            vector2.y = -x * sin + y * cos;
            return vector2;
        }

        /// <summary>
        /// 0未満の成分を0にする
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>0未満の成分を0にしたベクトル</returns>
        public static Vector2 DEToOnlyPositive(this Vector2 vector2) {
            vector2.x = Mathf.Max(vector2.x, 0f);
            vector2.y = Mathf.Max(vector2.y, 0f);
            return vector2;
        }

        /// <summary>
        /// 0超過の成分を0にする
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>0超過の成分を0にしたベクトル</returns>
        public static Vector2 DEToOnlyNegative(this Vector2 vector2) {
            vector2.x = Mathf.Min(vector2.x, 0f);
            vector2.y = Mathf.Min(vector2.y, 0f);
            return vector2;
        }

        /// <summary>
        /// 0未満の成分を正にする
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>0未満の成分を正にしたベクトル</returns>
        public static Vector2 DEToPositive(this Vector2 vector2) {
            vector2.x = Mathf.Abs(vector2.x);
            vector2.y = Mathf.Abs(vector2.y);
            return vector2;
        }

        /// <summary>
        /// 0超過の成分を負にする
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>0超過の成分を負にしたベクトル</returns>
        public static Vector2 DEToNegative(this Vector2 vector2) {
            vector2.x = -Mathf.Abs(vector2.x);
            vector2.y = -Mathf.Abs(vector2.y);
            return vector2;
        }

        /// <summary>
        /// 引数の関数を各成分に適用する
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <param name="func">適用させる関数</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DEApply(this Vector2 vector2, Func<float, float> func) {
            vector2.x = func(vector2.x);
            vector2.y = func(vector2.y);
            return vector2;
        }

        /// <summary>
        /// x成分に代入する
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <param name="x">代入する値</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DESetX(this Vector2 vector2, float x) {
            vector2.x = x;
            return vector2;
        }

        /// <summary>
        /// Y成分に代入する
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <param name="y">代入する値</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DESetY(this Vector2 vector2, float y) {
            vector2.y = y;
            return vector2;
        }

        /// <summary>
        /// X成分にマイナスをかける
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DEInverseX(this Vector2 vector2) {
            vector2.x = -vector2.x;
            return vector2;
        }

        /// <summary>
        /// Y成分にマイナスをかける
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DEInverseY(this Vector2 vector2) {
            vector2.y = -vector2.y;
            return vector2;
        }

        /// <summary>
        /// 逆向きのベクトルを返す
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>計算後のベクトル</returns>
        public static Vector2 DEInverse(this Vector2 vector2) {
            vector2.x = -vector2.x;
            vector2.y = -vector2.y;
            return vector2;
        }

        /// <summary>
        /// 厳密に同じベクトルかどうかを判定する
        /// </summary>
        /// <param name="vector2">比較するベクトル</param>
        /// <param name="other">比較するベクトル</param>
        /// <returns>厳密に同じかどうか</returns>
        public static bool DEStrictlyEquals(this Vector2 vector2, Vector2 other) {
            return vector2.x == other.x && vector2.y == other.y;
        }

        /// <summary>
        /// 2次元ベクトルを3次元ベクトルにする
        /// </summary>
        /// <param name="vector2">元のベクトル</param>
        /// <returns>z成分に0を追加したベクトル</returns>
        public static Vector3 DEToVector3(this Vector2 vector2) {
            return new Vector3(vector2.x, vector2.y, 0.0f);
        }
    }
}
