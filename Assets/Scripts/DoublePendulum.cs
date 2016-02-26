//-----------------------------------------------------------------------
// <copyright file="DoublePendulum.cs" company="dc1394's software">
//     Copyright ©  2016 @dc1394 All Rights Reserved.
//     This software is released under the BSD 2-Clause License.
// </copyright>
//-----------------------------------------------------------------------
namespace DoublePendulum
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 振り子クラスの実装
    /// </summary>
    internal class DoublePendulum : MonoBehaviour
    {
        #region フィールド

        /// <summary>
        /// 重力ベクトルの向き
        /// </summary>
        private static readonly Vector3 GravityDirection = Physics.gravity.normalized;

        /// <summary>
        /// 位置の履歴の最大数
        /// </summary>
        private static readonly Int32 PosHistoryMax = 10000;

        /// <summary>
        /// ボタンの「Start」テキスト
        /// </summary>
        private static readonly String StartText = "Start";

        /// <summary>
        /// ボタンの「Stop」テキスト
        /// </summary>
        private static readonly String StopText = "Stop";

        /// <summary>
        /// ボタンのテキスト
        /// </summary>
        private String buttontext = DoublePendulum.StartText;

        /// <summary>
        /// ロープの重心のスケール
        /// </summary>
        [SerializeField]
        private float centerOfGarvityForRopeScale = 0.4f;

        /// <summary>
        /// 実行中かどうかを示すフラグ
        /// </summary>
        private Boolean exec = false;

        /// <summary>
        /// 球1の初期座標
        /// </summary>
        private Vector3 firstsphere1pos;

        /// <summary>
        /// 球2の初期座標
        /// </summary>
        private Vector3 firstsphere2pos;

        /// <summary>
        /// θ1の初期値
        /// </summary>
        private float firsttheta1;

        /// <summary>
        /// θ2の初期値
        /// </summary>
        private float firsttheta2;

        /// <summary>
        /// ラインレンダラー
        /// </summary>
        [SerializeField]
        private LineRenderer lineRenderer = null;

        /// <summary>
        /// 球の質量
        /// </summary>
        [SerializeField]
        private float mass = 1.413141f;

        /// <summary>
        /// 原点の座標
        /// </summary>
        [SerializeField]
        private Vector3 origin = Vector3.zero;

        /// <summary>
        /// 位置の履歴を格納したList
        /// </summary>
        private List<Vector3> posHistoryList = new List<Vector3>();

        /// <summary>
        /// 位置の履歴の数
        /// </summary>
        private Int32 posHistoryIndex;
        
        /// <summary>
        /// 前回のフレームで取得した時間
        /// </summary>
        private float previousTime = 0.0f;

        /// <summary>
        /// ロープオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject rope1 = null;

        /// <summary>
        /// ロープオブジェクトその2
        /// </summary>
        [SerializeField]
        private GameObject rope2 = null;

        /// <summary>
        /// ロープの長さ
        /// </summary>
        private float ropeLength;

        /// <summary>
        /// 球オブジェクト
        /// </summary>
        [SerializeField]
        private GameObject sphere1 = null;

        /// <summary>
        /// 球オブジェクトその2
        /// </summary>
        [SerializeField]
        private GameObject sphere2 = null;

        /// <summary>
        /// 角度θ1（単位は度）
        /// </summary>
        private float theta1deg;

        /// <summary>
        /// 角度θ2（単位は度）
        /// </summary>
        private float theta2deg;

        /// <summary>
        /// 軌跡を表示するかどうか
        /// </summary>
        private Boolean drawtrajectory = false;
        
        /// <summary>
        /// 速度v1
        /// </summary>
        private float velocity1 = 0.0f;

        /// <summary>
        /// 速度v2
        /// </summary>
        private float velocity2 = 0.0f;

        #endregion フィールド

        #region メソッド

        /// <summary>
        /// 与えた座標から測定した、球の座標と重力ベクトルの成す角度を与える関数
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="sphere">球オブジェクト</param>
        /// <returns>球の座標と重力ベクトルの成す角度</returns>
        private float GetSphereAndGravityAngle(Vector3 pos, GameObject sphere)
        {
            return Vector3.Angle(sphere.transform.position - pos, DoublePendulum.GravityDirection);
        }

        /// <summary>
        /// 与えた座標から測定した、球の座標のz座標を与える関数
        /// </summary>
        /// <param name="pos">座標</param>
        /// <param name="sphere">球オブジェクト</param>
        /// <returns>球の座標のz座標</returns>
        private float GetSpherePosZ(Vector3 pos, GameObject sphere)
        {
            return (sphere.transform.position - pos).z;
        }

        /// <summary>
        /// 座標と球が成す角を計算する
        /// </summary>
        /// <param name="position">座標</param>
        /// <param name="sphere">球オブジェクト</param>
        /// <returns>座標と球が成す角</returns>
        private float GetThetaDeg(Vector3 position, GameObject sphere)
        {
            var theta = this.GetSphereAndGravityAngle(position, sphere);

            return theta * (this.GetSpherePosZ(position, sphere) > 0.0f ? 1.0f : -1.0f);
        }

        /// <summary>
        /// GUIイベントの処理
        /// </summary>
        private void OnGUI()
        {
            var ypos = 20.0f;

            var theta1 = this.GetThetaDeg(this.origin, this.sphere1);

            // ラベルに角度θ1の値を表示する
            GUI.Label(
                new Rect(20.0f, ypos, 160.0f, 20.0f),
                String.Format("角度θ1 = {0:F3}°", theta1));

            ypos += 20.0f;

            var theta2 = this.GetThetaDeg(this.sphere1.transform.position, this.sphere2);

            // ラベルに角度θ2の値を表示する
            GUI.Label(
                new Rect(20.0f, ypos, 160.0f, 20.0f),
                String.Format("角度θ2 = {0:F3}°", theta2));

            ypos += 20.0f;
            
            // ラベルに速度v1の値を表示する
            GUI.Label(
                new Rect(20.0f, ypos, 160.0f, 20.0f),
                String.Format("速度v1 = {0:F3}(m/s)", this.ropeLength * Solveeomcs.SolveEOMcs.GetV1()));

            ypos += 20.0f;

            // ラベルに速度v2の値を表示する
            GUI.Label(
                new Rect(20.0f, ypos, 160.0f, 20.0f),
                String.Format("速度v2 = {0:F3}(m/s)", this.ropeLength * Solveeomcs.SolveEOMcs.GetV2()));

            var ypos2 = 20.0f;

            // 軌跡を表示するかどうかのチェックボックスを表示する
            var drawtrajectorybefore = this.drawtrajectory;
            this.drawtrajectory = GUI.Toggle(new Rect(200.0f, ypos2, 150.0f, 20.0f), this.drawtrajectory, "軌跡の表示");
            if (drawtrajectorybefore != this.drawtrajectory)
            {
                this.PosHistoryInit();
            }

            ypos2 += 20.0f;

            // 「角度θ1」と表示する
            GUI.Label(new Rect(200.0f, ypos2, 100.0f, 20.0f), "角度θ1");

            ypos2 += 20.0f;

            // 角度θ1を変更するスライダーを表示する
            var theta1degbefore = this.theta1deg;
            this.theta1deg = GUI.HorizontalSlider(new Rect(200.0f, ypos2, 100.0f, 20.0f), this.theta1deg, -180.0f, 180.0f);
            if (Mathf.Abs(this.theta1deg - theta1degbefore) > Mathf.Epsilon)
            {
                var theta = Mathf.Deg2Rad * this.theta1deg;
                Solveeomcs.SolveEOMcs.SetTheta1(theta);
                this.Sphere1And2Rotate(theta, Mathf.Deg2Rad * theta2);
                this.RopeUpdate();
            }

            ypos2 += 20.0f;

            // 「角度θ1」と表示する
            GUI.Label(new Rect(200.0f, ypos2, 100.0f, 20.0f), "角度θ2");

            ypos2 += 20.0f;

            // 角度θ2を変更するスライダーを表示する
            var theta2degbefore = this.theta2deg;
            this.theta2deg = GUI.HorizontalSlider(new Rect(200.0f, ypos2, 100.0f, 20.0f), this.theta2deg, -180.0f, 180.0f);
            if (Mathf.Abs(this.theta2deg - theta2degbefore) > Mathf.Epsilon)
            {
                var theta = Mathf.Deg2Rad * this.theta2deg;
                Solveeomcs.SolveEOMcs.SetTheta2(theta);
                this.Sphere1And2Rotate(Mathf.Deg2Rad * theta1, theta);
                this.RopeUpdate();
            }

            ypos2 += 20.0f;

            // 「速度v1」と表示する
            GUI.Label(new Rect(200.0f, ypos2, 100.0f, 20.0f), "速度v1");

            ypos2 += 20.0f;

            // 速度v1を変更するスライダーを表示する
            var velocity1before = this.velocity1;
            this.velocity1 = GUI.HorizontalSlider(new Rect(200.0f, ypos2, 100.0f, 20.0f), this.velocity1, -20.0f, 20.0f);
            if (Mathf.Abs(this.velocity1 - velocity1before) > Mathf.Epsilon)
            {
                Solveeomcs.SolveEOMcs.SetV1(this.velocity1 / this.ropeLength);
            }

            ypos2 += 20.0f;

            // 「速度v2」と表示する
            GUI.Label(new Rect(200.0f, ypos2, 100.0f, 20.0f), "速度v2");

            ypos2 += 20.0f;

            // 速度v2を変更するスライダーを表示する
            var velocity2before = this.velocity2;
            this.velocity2 = GUI.HorizontalSlider(new Rect(200.0f, ypos2, 100.0f, 20.0f), this.velocity2, -20.0f, 20.0f);
            if (Mathf.Abs(this.velocity2 - velocity2before) > Mathf.Epsilon)
            {
                Solveeomcs.SolveEOMcs.SetV1(this.velocity2 / this.ropeLength);
            }

            ypos2 += 20.0f;

            var ypos3 = 20.0f;

            // 「Start」か「Stop」ボタンを表示する
            if (GUI.Button(new Rect(320.0f, ypos3, 110.0f, 20.0f), this.buttontext))
            {
                if (this.exec)
                {
                    this.exec = false;
                    this.buttontext = DoublePendulum.StartText;
                }
                else
                {
                    this.exec = true;
                    this.buttontext = DoublePendulum.StopText;
                }
            }

            ypos3 += 30.0f;

            // 「Reset」ボタンを表示する
            if (GUI.Button(new Rect(320.0f, ypos3, 110.0f, 20.0f), "Reset"))
            {
                Solveeomcs.SolveEOMcs.SetTheta1(this.firsttheta1);
                Solveeomcs.SolveEOMcs.SetTheta2(this.firsttheta2);
                Solveeomcs.SolveEOMcs.SetV1(0.0f);
                Solveeomcs.SolveEOMcs.SetV2(0.0f);

                this.Sphere1And2Rotate(this.firsttheta1, this.firsttheta2);
                this.RopeUpdate();

                this.PosHistoryInit();
            }

            ypos3 += 30.0f;

            // 「Exit」ボタンを表示する
            if (GUI.Button(new Rect(320.0f, ypos3, 110.0f, 20.0f), "Exit"))
            {
                Application.Quit();
            }
        }

        /// <summary>
        /// 与えられた座標を中心にして、球の方向を向くようにロープを回転する
        /// </summary>
        /// <param name="position">回転の中心座標</param>
        /// <param name="rope">ロープオブジェクト</param>
        /// <param name="sphere">球オブジェクト</param>
        private void RopeRotate(Vector3 position, GameObject rope, GameObject sphere)
        {
            // ロープ1の角度を初期化
            rope.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            // 角度が正かどうか
            if (this.GetSpherePosZ(position, sphere) > 0.0f)
            {
                // ロープ1の角度を更新
                rope.transform.Rotate(new Vector3(-this.GetSphereAndGravityAngle(position, sphere), 0.0f, 0.0f));
            }
            else
            {
                // ロープ1の角度を更新
                rope.transform.Rotate(new Vector3(this.GetSphereAndGravityAngle(position, sphere), 0.0f, 0.0f));
            }
        }

        /// <summary>
        /// ロープ1の座標と角度を更新する
        /// </summary>
        private void RopeUpdate()
        {
            // ロープ1の座標を更新
            this.rope1.transform.position = new Vector3(
                0.0f,
                this.centerOfGarvityForRopeScale * this.sphere1.transform.position.y,
                this.centerOfGarvityForRopeScale * this.sphere1.transform.position.z);

            this.RopeRotate(this.origin, this.rope1, this.sphere1);

            // ロープ2の座標を更新
            this.rope2.transform.position = new Vector3(
                0.0f,
                (this.sphere2.transform.position.y + this.sphere1.transform.position.y) * 0.5f,
                (this.sphere2.transform.position.z + this.sphere1.transform.position.z) * 0.5f);

            this.RopeRotate(this.sphere1.transform.position, this.rope2, this.sphere2);
        }
        
        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            this.PendulumInit();
        }

        /// <summary>
        /// フレーム処理
        /// </summary>
        private void Update()
        {
            // 時間差を取得
            var frameTime = Time.time - this.previousTime;

            // 新しく取得した時間を代入
            this.previousTime = Time.time;

            if (this.exec)
            {
                // 球の座標を更新
                this.SphereUpdate(frameTime);

                Action fill = () =>
                {
                    // 履歴に現在の球2の位置を追加
                    this.posHistoryList.Add(this.sphere2.transform.position);

                    for (var i = 0; i < posHistoryIndex; i++)
                    {
                        // 頂点を履歴で埋める
                        this.lineRenderer.SetPosition(i, posHistoryList[i]);
                    }
                };

                if (this.drawtrajectory)
                {
                    if (this.posHistoryIndex < DoublePendulum.PosHistoryMax)
                    {
                        // 頂点数を変更
                        this.lineRenderer.SetVertexCount(++this.posHistoryIndex);

                        fill();
                    }
                    else
                    {
                        // 履歴の最初の要素を削除
                        this.posHistoryList.RemoveAt(0);

                        fill();
                    }
                }

                // ロープの座標と角度を更新
                this.RopeUpdate();
            }
        }

        /// <summary>
        /// 振り子の状態を初期化する
        /// </summary>
        private void PendulumInit()
        {
            this.ropeLength = Vector3.Distance(this.origin, this.sphere1.transform.position);

            this.theta1deg = this.GetThetaDeg(this.origin, this.sphere1);
            this.theta2deg = this.GetThetaDeg(this.firstsphere1pos = this.sphere1.transform.position, this.sphere2);

            this.firstsphere2pos = this.sphere2.transform.position;

            Solveeomcs.SolveEOMcs.Init(
                this.ropeLength,
                this.mass,
                this.firsttheta1 = Mathf.Deg2Rad * this.theta1deg,
                this.firsttheta2 = Mathf.Deg2Rad * this.theta2deg);
        }

        /// <summary>
        /// posHistoryListを初期化する
        /// </summary>
        private void PosHistoryInit()
        {
            // posHistryListを消去
            this.posHistoryList.Clear();

            this.lineRenderer.SetVertexCount(this.posHistoryIndex = 0);
        }
        
        /// <summary>
        /// 球の座標を回転する
        /// </summary>
        /// <param name="sphere">球オブジェクト</param>
        /// <param name="theta">θの角度</param>
        private void SphereRotate(GameObject sphere, float theta)
        {
            // rcosθの計算
            var rcostheta = this.ropeLength * Mathf.Cos(theta);

            // rsinθの計算
            var rsintheta = this.ropeLength * Mathf.Sin(theta);

            // 球1の座標をθ回転
            sphere.transform.position = new Vector3(
                0.0f,
                (rsintheta * DoublePendulum.GravityDirection.z) + (rcostheta * DoublePendulum.GravityDirection.y),
                (rcostheta * DoublePendulum.GravityDirection.z) - (rsintheta * DoublePendulum.GravityDirection.y));
        }

        /// <summary>
        /// 球1と球2の座標を回転する
        /// </summary>
        /// <param name="theta1">θ1</param>
        /// <param name="theta2">θ2</param>
        private void Sphere1And2Rotate(float theta1, float theta2)
        {
            this.SphereRotate(this.sphere1, theta1);
            this.SphereRotate(this.sphere2, theta2);

            // 球2の重心座標を球1分平行移動
            this.sphere2.transform.position += this.sphere1.transform.position;
        }

        /// <summary>
        /// 球の座標を更新する
        /// </summary>
        /// <param name="frameTime">経過時間</param>
        private void SphereUpdate(float frameTime)
        {
            float theta1, theta2;

            unsafe
            {
                // 運動方程式を解いてθを求める
                Solveeomcs.SolveEOMcs.NextStep(frameTime, &theta1, &theta2);
            }

            this.Sphere1And2Rotate(theta1, theta2);
        }

        #endregion メソッド
    }
}
