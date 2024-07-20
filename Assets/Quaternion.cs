using System;
using UnityEngine;

namespace FThingSoftware
{
    [Serializable]
    public struct Quaternion
    {
        // ===============================
        // メンバー変数
        // ===============================

        public float x;
        public float y;
        public float z;
        public float w;

        // ===============================
        // コンストラクタ
        // ===============================

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // ===============================
        // static変数
        // ===============================

        // 無回転のクォータニオン(単位回転)
        public static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);

        // ===============================
        // 変数
        // ===============================

        // オイラー角表現を返したり、代入できるようにする
        public Vector3 eulerAngles
        {
            get
            {
                // Unityなので、z-x-y系のオイラー各に変換する

                // x/y/z軸の回転量
                float angle_x, angle_y, angle_z = 0;

                // 角度が90度のときの誤差を補正する
                float theta_x = (2 * (y * z + x * w) >= 0.9999999f) ? 1.0f : 2 * (y * z + x * w);
                angle_x = MathF.Asin(theta_x) * Mathf.Rad2Deg;

                if (MathF.Cos(angle_x * Mathf.Deg2Rad) != 0)
                {
                    float y_top = 2 * (x * z - y * w);
                    float y_bottom = 2 * (w * w + z * z) - 1;
                    angle_y = MathF.Atan2(-y_top, y_bottom) * Mathf.Rad2Deg;

                    float z_top = 2 * (x * y - z * w);
                    float z_bottom = 2 * (w * w + y * y) - 1;
                    angle_z = Mathf.Atan2(-z_top, z_bottom) * Mathf.Rad2Deg;
                }
                else
                {
                    angle_y = 0;

                    float z_top = 2 * (x * y + z * w);
                    float z_bottom = 2 * (w * w + x * x) - 1;
                    angle_z = Mathf.Atan2(z_top, z_bottom) * Mathf.Rad2Deg;
                }

                // 角度の範囲を0~360の範囲に補正する
                return MakePositive(new Vector3(angle_x, angle_y, angle_z));
            }

            // オイラー角を代入して回転できるようにする
            set
            {
                this = Euler(value.x, value.y, value.z);
            }
        }

        // 0 ~ 360度の範囲に補正する
        private static Vector3 MakePositive(Vector3 euler)
        {
            float min = 0f;
            float max = 360f;

            if (euler.x < min) { euler.x += 360f; }
            else if (euler.x > max) { euler.x -= 360f; }

            if (euler.y < min) { euler.y += 360f; }
            else if (euler.y > max) { euler.y -= 360f; }

            if (euler.z < min) { euler.z += 360f; }
            else if (euler.z > max) { euler.z -= 360f; }

            return euler;
        }

        // 正規化したクォータニオンを返す
        public Quaternion normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        // ===============================
        // public関数
        // ===============================

        public void Set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // fromDirectionからtoDirectionへの回転を作成して代入する
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }

        // 指定した forward と upwards 方向に回転する
        public void SetLookRotation(Vector3 view)
        {
            // this = LookRotation(view);
        }

        // 回転を座標に対する角度の値(AngleAxis)に変換する
        public void ToAngleAxis(out float angle, out Vector3 axis)
        {
            // クォータニオンの定義より求める

            // cos(rad/2) = w より
            float rad = Mathf.Acos(w) * 2.0f;
            angle = rad * Mathf.Rad2Deg;

            // x = axis.x * sin(rad/2), y = axis.y * sin(rad/2), z = axis.z * sin(rad/2)より
            axis = new Vector3(this.x / Mathf.Sin(rad / 2), this.y / Mathf.Sin(rad / 2), this.z / Mathf.Sin(rad / 2));
        }

        // クォータニオンの値を見やすくした文字列を返す
        // ToStringメソッドをオーバーライドする
        public override string ToString()
        {
            string format = "F5";
            return $"({x.ToString(format)}, {y.ToString(format)}, {z.ToString(format)}, {w.ToString(format)})";
        }

        // ===============================
        // static関数
        // ===============================

        //  2つの回転aとb間の角度を返す(Unityの実装を引用)
        public static float Angle(Quaternion a, Quaternion b)
        {
            // a・b = cos(θ/2) となることを利用する
            float dot = Mathf.Min(Mathf.Abs(Dot(a, b)), 1f);
            return IsEqualUsingDot(dot) ? 0f : (Mathf.Acos(dot) * 2f * Mathf.Rad2Deg);
        }
        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 0.999999f;
        }

        // axisの周りをangle度回転する回転を生成して返す
        public static Quaternion AngleAxis(float angle, Vector3 axis)
        {
            // クォータニオンの定義
            // 正規化された軸ベクトルλ(x, y, z)と回転量Θのとき
            // (x, y, z, w) = ( λx * sin(Θ/2), λy * sin(Θ/2), λz * sin(Θ/2), cos(Θ/2) )

            // ベクトルの正規化
            axis.Normalize();
            // degree から radian への変換
            float rad = angle * Mathf.Deg2Rad;

            // 定義に従って成分を設定する
            return new Quaternion(
                    axis.x * MathF.Sin(rad / 2),
                    axis.y * MathF.Sin(rad / 2),
                    axis.z * MathF.Sin(rad / 2),
                    MathF.Cos(rad / 2)
                );
        }

        // 2つの回転の内積を返す(Unityの実装を引用)
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        // z軸を中心にz度、x軸を中心にx度、y軸を中心にy度回転する回転を返す。回転はこの順序で適用される
        public static Quaternion Euler(float x, float y, float z)
        {
            // それぞれの軸を中心とした回転を生成
            Quaternion qua_x = AngleAxis(x, Vector3.right);
            Quaternion qua_y = AngleAxis(y, Vector3.up);
            Quaternion qua_z = AngleAxis(z, Vector3.forward);

            // z-x-y系なので、z-x-yの順番で回転させる
            // クォータニオンの乗算を行う(右から掛けていく)
            return qua_y * qua_x * qua_z;
        }

        // fromDirection から toDirection への回転を作成して返す
        public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            // 外積を用いて、軸ベクトルを求める
            Vector3 axis = Vector3.Cross(fromDirection, toDirection);

            // 外積が(0,0,0)の時は、無回転のクォータニオン(0,0,0,1)にする
            if (axis == Vector3.zero) { return identity; }

            // 内積の定義から回転量を求める a・b = |a||b|cosθ なので
            float rad = MathF.Acos(Vector3.Dot(fromDirection, toDirection) / (fromDirection.magnitude * toDirection.magnitude));

            // 求めた軸と回転量でクォータニオンの生成
            return AngleAxis(Mathf.Rad2Deg * rad, axis);
        }

        // rotationの逆(共役)クォータニオンを返す
        public static Quaternion Inverse(Quaternion rotation)
        {
            return new Quaternion(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }

        // aとbの間をtで補間して正規化する、tは[0,1]の範囲
        public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
        {
            // tの範囲の制限
            t = Mathf.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }

        // aとbの間をtで補間して正規化する、tは[0,1]の範囲にクランプされない
        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            // 2つの回転の内積を求める
            float dot = Dot(a, b);
            // 内積が負の時、最短距離での補間を得るために片方を負にする
            if (dot < 0)
            {
                b = new Quaternion(-b.x, -b.y, -b.z, -b.w);
            }

            // 直線経路で補完する a*(1-t) + b*t
            Quaternion lerp = new Quaternion(
                    a.x + (-a.x + b.x) * t,
                    a.y + (-a.y + b.y) * t,
                    a.z + (-a.z + b.z) * t,
                    a.w + (-a.w + b.w) * t
                );
            return lerp.normalized;
        }

        // オブジェクトの正面(forward)を引数のforwardの向きに回転させる回転を生成する
        public static Quaternion LookRotation(Vector3 forward, Transform objTransform)
        {
            // オブジェクトの正面からforwardに向ける回転を取得
            Quaternion lookRotation = FromToRotation(Vector3.forward, forward);

            // Look後のz軸(青)を求める
            Vector3 zAxisAfterLook = lookRotation * Vector3.forward;
            // 水平方向のみの成分にする(upwardsに垂直なベクトルにする)
            Vector3 zAxisHorizontal = new Vector3(zAxisAfterLook.x, 0f, zAxisAfterLook.z);

            // 回転後のx軸(赤)を求めるために
            // Look後のz軸(青)の水平成分のみのベクトルを、垂直を軸にして90度回転させる
            Quaternion getXAxisRotationFromZHorizontal = Quaternion.AngleAxis(90, Vector3.up);
            Vector3 xAxisAfterRotate = getXAxisRotationFromZHorizontal * zAxisHorizontal;

            // 回転後のy軸(緑)を求めるために
            // 回転後のx軸(赤)を、Look後のz軸(青)を軸にして90度回転させる
            Quaternion getYAxisRotationFromXAxisAfterRotate = Quaternion.AngleAxis(90, zAxisAfterLook);
            Vector3 yAxisAfterRotate = getYAxisRotationFromXAxisAfterRotate * xAxisAfterRotate;

            // Look後のy軸(緑) から 回転後のy軸(緑) へ修正する回転を求める
            Vector3 yAxisBeforeModify = lookRotation * Vector3.up;
            Quaternion modifyRotation = Quaternion.FromToRotation(yAxisBeforeModify, yAxisAfterRotate);

            // 回転を合成して返す
            return modifyRotation * lookRotation;
        }

        // 正規化を行ったものを返す(Unityの実装を引用)
        public static Quaternion Normalize(Quaternion q)
        {
            float num = Mathf.Sqrt(Dot(q, q));
            if (num < Mathf.Epsilon)
            {
                return identity;
            }
            return new Quaternion(q.x / num, q.y / num, q.z / num, q.w / num);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }

        // fromからtoへの回転を得る(Unityの実装を引用)
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float num = Angle(from, to);
            if (num == 0f)
            {
                return to;
            }
            return Slerp(from, to, Mathf.Min(1f, maxDegreesDelta / num));
        }

        // 球面線形補完を得る、tは[0,1]の範囲
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            // tの範囲の制限
            t = Mathf.Clamp01(t);
            return SlerpUnclamped(a, b, t);
        }

        // 球面線形補完を得る、tは[0,1]の範囲にクランプされない
        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            // 2つの回転の内積を求める
            float dot = Dot(a, b);
            // 内積が負の時、最短距離での補間を得るために片方を負にする
            if (dot < 0)
            {
                b = new Quaternion(-b.x, -b.y, -b.z, -b.w);
            }

            // 2つの回転の角度を求める
            float rad = IsEqualUsingDot(dot) ? 0f : Mathf.Acos(dot);

            // 球面線形補完
            float bottom = Mathf.Sin(rad);
            float a_rate = Mathf.Sin((1 - t) * rad) / bottom;
            float b_rate = Mathf.Sin(t * rad) / bottom;

            Quaternion slerp = new Quaternion(
                    a.x * a_rate + b.x * b_rate,
                    a.y * a_rate + b.y * b_rate,
                    a.z * a_rate + b.z * b_rate,
                    a.w * a_rate + b.w * b_rate
                );

            return slerp.normalized;
        }

        // 球面四角形補間（Spherical and Quadrangle Interpolation）
        // 4つのクォータニオンを補間する
        // tは[0,1]の範囲でクランプされる
        public static Quaternion Squad(Quaternion q1, Quaternion q2, Quaternion a, Quaternion b, float t)
        {
            t = Mathf.Clamp01(t);
            return SquadUnclamped(q1, q2, a, b, t);
        }

        // 球面四角形補間（Spherical and Quadrangle Interpolation）
        // tは[0,1]の範囲にクランプされない
        public static Quaternion SquadUnclamped(Quaternion q1, Quaternion q2, Quaternion a, Quaternion b, float t)
        {
            Quaternion slerp1 = Slerp(q1, q2, t);
            Quaternion slerp2 = Slerp(a, b, t);
            return Slerp(slerp1, slerp2, 2 * t * (1 - t));
        }

        // ===============================
        // Operator
        // ===============================

        // 暗黙的な型変換演算子の定義
        // UnityEngine.Quaternionに自動的に変換して
        // transform.rotation = Quaternion; ができるようにする
        public static implicit operator UnityEngine.Quaternion(Quaternion q)
        {
            return new UnityEngine.Quaternion(q.x, q.y, q.z, q.w);
        }

        // 演算子オーバーロードを用いて
        // クォータニオン同士の乗算ができるようにする
        public static Quaternion operator * (Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.w*b.x - a.z*b.y + a.y*b.z + a.x*b.w,
                a.z*b.x + a.w*b.y - a.x*b.z + a.y*b.w,
                -a.y*b.x + a.x*b.y + a.w*b.z + a.z*b.w,
                -a.x*b.x - a.y*b.y - a.z*b.z + a.w*b.w
            );
        }

        // 演算子オーバーロードを用いて
        // Quaternion * Vector3 が計算できるようにする
        public static Vector3 operator * (Quaternion q, Vector3 v)
        {
            // 回転後のベクトルv'について
            // v' = q * v * q_inverse となる
            // 3次元座標をクォータニオンで表す
            Quaternion vecQuaternion = new Quaternion(v.x, v.y, v.z, 0);
            // ベクトルに回転を施す
            Quaternion vecAfterRotate = q * vecQuaternion * Quaternion.Inverse(q);
            // クォータニオンを3次元座標に変換する
            return new Vector3(vecAfterRotate.x, vecAfterRotate.y, vecAfterRotate.z);
        }

        // 比較(Unityの実装を引用)
        public override bool Equals(object other)
        {
            if (!(other is Quaternion))
            {
                return false;
            }

            return Equals((Quaternion)other);
        }

        public bool Equals(Quaternion other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }
    }
}