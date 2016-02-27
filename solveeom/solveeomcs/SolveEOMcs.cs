//-----------------------------------------------------------------------
// <copyright file="SolveEOMcs.cs" company="dc1394's software">
//     Copyright ©  2016 @dc1394 All Rights Reserved.
//     This software is released under the BSD 2-Clause License.
// </copyright>
//-----------------------------------------------------------------------

namespace Solveeomcs
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// C++で書かれたSolveEOMクラスをC#からアクセスするためのラッパークラス
    /// </summary>
    public sealed class SolveEOMcs
    {
        #region メソッド

        /// <summary>
        /// 角度θの値に対するgetter
        /// </summary>
        /// <returns>角度θ</returns>
        [DllImport("solveeom", EntryPoint = "gettheta1")]
        public static extern float GetTheta1();

        /// <summary>
        /// 角度θの値に対するgetter
        /// </summary>
        /// <returns>角度θ</returns>
        [DllImport("solveeom", EntryPoint = "gettheta2")]
        public static extern float GetTheta2();
        
        /// <summary>
        /// 速度vの値に対するgetter
        /// </summary>
        /// <returns>速度v</returns>
        [DllImport("solveeom", EntryPoint = "getv1")]
        public static extern float GetV1();

        /// <summary>
        /// 速度vの値に対するgetter
        /// </summary>
        /// <returns>速度v</returns>
        [DllImport("solveeom", EntryPoint = "getv2")]
        public static extern float GetV2();

        /// <summary>
        /// seオブジェクトを初期化する
        /// </summary>
        /// <param name="l">ロープの長さ</param>
        /// <param name="m">球の質量</param>
        /// <param name="theta1_0">θ1の初期値</param>
        /// <param name="theta2_0">θ2の初期値</param>
        [DllImport("solveeom", EntryPoint = "init")]
        public static extern void Init(float l, float m, float theta1_0, float theta2_0);
        
        /// <summary>
        /// 運動エネルギーを求める
        /// </summary>
        /// <returns>運動エネルギー</returns>
        [DllImport("solveeom", EntryPoint = "kinetic_energy")]
        public static extern float Kinetic_Energy();
        
        /// <summary>
        /// 次のステップを計算する
        /// </summary>
        /// <param name="dt">経過した時間</param>
        /// <param name="theta1">θ1の値</param>
        /// <param name="theta2">θ2の値</param>
        [DllImport("solveeom", EntryPoint = "nextstep")]
        public static unsafe extern float NextStep(float dt, float * theta1, float * theta2);
        
        /// <summary>
        /// ポテンシャルエネルギーを求める
        /// </summary>
        /// <returns>ポテンシャルエネルギー</returns>
        [DllImport("solveeom", EntryPoint = "potential_energy")]
        public static extern float Potential_Energy();

        /// <summary>
        /// 角度θ1の値に対するsetter
        /// </summary>
        /// <param name="theta">設定する角度θ1</param>
        [DllImport("solveeom", EntryPoint = "settheta1")]
        public static extern void SetTheta1(float theta);

        /// <summary>
        /// 角度θ2の値に対するsetter
        /// </summary>
        /// <param name="theta">設定する角度θ2</param>
        [DllImport("solveeom", EntryPoint = "settheta2")]
        public static extern void SetTheta2(float theta);

        /// <summary>
        /// 速度v1の値に対するsetter
        /// </summary>
        /// <param name="v">設定する速度v</param>
        [DllImport("solveeom", EntryPoint = "setv1")]
        public static extern void SetV1(float v);

        /// <summary>
        /// 速度v2の値に対するsetter
        /// </summary>
        /// <param name="v">設定する速度v</param>
        [DllImport("solveeom", EntryPoint = "setv2")]
        public static extern void SetV2(float v);
        
        #endregion メソッド
    }
}