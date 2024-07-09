using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace FThingSoftware
{
    [Serializable]
    public struct Quaternion
    {
        // メンバー変数
        public float x;
        public float y;
        public float z;
        public float w;

        // コンストラクタ
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // static変数
        // 無回転のクォータニオン(単位回転)
        public static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);

        public Vector3 eulerAngle
        {
            get
            {
                // Unityなので、z-x-y系のオイラー各に変換する

                // x/y/z軸の回転量
                float angle_x, angle_y, angle_z = 0;

                // 角度が90度のときの誤差を補正する
                float theta_x = (2 * (y*z + x*w) >= 0.9999999f) ? 1.0f : 2 * (y*z + x*w);
                angle_x = MathF.Asin(theta_x) * Mathf.Rad2Deg;

                if(MathF.Cos(angle_x * Mathf.Deg2Rad) != 0)
                {
                    float y_top = 2 * (x*z - y*w);
                    float y_bottom = 2 * (w*w + z*z) - 1;
                    angle_y = MathF.Atan2(-y_top, y_bottom) * Mathf.Rad2Deg;
                    
                    float z_top = 2 * (x*y - z*w);
                    float z_bottom = 2 * (w*w + y*y) - 1;
                    angle_z = Mathf.Atan2(-z_top, z_bottom) * Mathf.Rad2Deg;
                }
                else
                {
                    angle_y = 0;

                    float z_top = 2 * (x*y + z*w);
                    float z_bottom = 2 * (w*w + x*x) - 1;
                    angle_z = Mathf.Atan2(z_top, z_bottom) * Mathf.Rad2Deg;
                } 

                return new Vector3(Math.Abs(angle_x), Math.Abs(angle_y), Math.Abs(angle_z));
            }

            // オイラー角を代入して回転できるようにする            
            // z-x-y系なので、z-x-yの順番で回転させる
            set
            {
                // それぞれの軸を中心とした回転を生成
                Quaternion qua_x = AngleAxis(value.x, Vector3.right);
                Quaternion qua_y = AngleAxis(value.y, Vector3.up);
                Quaternion qua_z = AngleAxis(value.z, Vector3.forward);
                // クォータニオンの乗算を行う(右から掛けていく)
                this = qua_y * qua_x * qua_z;
            }
        }

        // 正規化したクォータニオンを返す
        public Quaternion normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        // public関数
        public void Set(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // fromDirectionからtoDirectionへの回転を作成する
        public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
        {
            this = FromToRotation(fromDirection, toDirection);
        }

        // 指定した forward と upwards 方向に回転する
        public static void SetLookRotation(Vector3 view, Vector3 up)
        {

        }

        // static関数
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
                    axis.x * MathF.Sin(rad/2),
                    axis.y * MathF.Sin(rad/2),
                    axis.z * MathF.Sin(rad/2),
                    MathF.Cos(rad/2)
                );
        }

        // 2つの回転の内積を返す(Unityの実装を引用)
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }


        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards, Transform objTransform)
        {
            // forwardが向きたい方向で、upwardsがどの軸を上にするか
            // オブジェクトの正面(forward)を引数のforwardの向きに回転させる回転を生成する

            // Vector3.forwardからforwardに向ける回転を取得
            Quaternion look = FromToRotation(Vector3.forward, forward);

            // 水平面と青軸との角度を求める
            // 回転後の青軸の向きを求める
            Vector3 zAxisAfterRotate = look.ToUnity() * Vector3.forward;
            // 水平方向のみの成分にする
            Vector3 zAxisHorizontal = new Vector3(zAxisAfterRotate.x, 0f, zAxisAfterRotate.z);

            // デバッグ
            Debug.DrawRay (objTransform.position, zAxisHorizontal * 2.0f, Color.red, 0.1f, false);
            Debug.DrawRay (objTransform.position, zAxisAfterRotate * 2.0f, Color.blue, 0.1f, false);

            // 角度を求める
            float rad = MathF.Acos(Vector3.Dot(zAxisHorizontal, zAxisAfterRotate) / (zAxisHorizontal.magnitude * zAxisAfterRotate.magnitude));
            Debug.Log(rad * Mathf.Rad2Deg);

            // 裏にあるか表にあるか求める
            // OP zAxisAfterRotate
            // 法線ベクトルn Vector3.up
            float reverse = Vector3.Dot(zAxisAfterRotate, Vector3.up);
            Debug.Log(reverse);
            // 正の値なら+, 負の値なら-なので、角度を変更する
            rad = reverse >= 0 ? rad : -rad;

            Debug.Log(rad * Mathf.Rad2Deg);

            // return look;

            // 真上の緑軸を赤軸を中心に角度だけ回転させた軸を計算する

            // 回転後の赤軸の水平方向のみの成分を求める
            Vector3 xAxisAfterRotate = look.ToUnity() * Vector3.right;
            Vector3 xAxisHorizontal = new Vector3(xAxisAfterRotate.x, 0, xAxisAfterRotate.z);
            // これを軸として、緑軸を回転させる
            Quaternion yModify = Quaternion.AngleAxis(-rad * Mathf.Rad2Deg, xAxisHorizontal);
            // 上を修正した緑軸の向きを求める
            Vector3 yModifyRotate = yModify.ToUnity() * Vector3.up;

            // 回転後の緑軸の向きを求める
            Vector3 yAxisAfterRotate = look.ToUnity() * Vector3.up;

            // 回転後の緑軸から、上を修正した緑軸へ向かう回転を求める
            Quaternion modify = Quaternion.FromToRotation(yAxisAfterRotate, yModifyRotate);

            return modify * look;
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

        // UnityEngine.Quaternionに変換する
        public UnityEngine.Quaternion ToUnity()
        {
            return new UnityEngine.Quaternion(this.x, this.y, this.z, this.w);
        }

        // 演算子オーバーロードを用いて、クォータニオン同士の乗算と比較ができるようにする
        public static Quaternion operator * (Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.w*b.x - a.z*b.y + a.y*b.z + a.x*b.w,
                a.z*b.x + a.w*b.y - a.x*b.z + a.y*b.w,
                -a.y*b.x + a.x*b.y + a.w*b.z + a.z*b.w,
                -a.x*b.x - a.y*b.y - a.z*b.z + a.w*b.w
            );
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