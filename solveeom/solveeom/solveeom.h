/*! \file solveeom.h
    \brief 単振り子に対して運動方程式を解くクラスの宣言

    Copyright ©  2016 @dc1394 All Rights Reserved.
    This software is released under the BSD 2-Clause License.
*/
#ifndef _SOLVEEOM_H_
#define _SOLVEEOM_H_

#include "utility/property.h"
#include <array>                        // for std::array
#include <functional>                   // for std::function
#include <string>                       // for std::string
#include <boost/numeric/odeint.hpp>     // for boost::numeric::odeint

namespace solveeom {
    using namespace utility;

    //! A function.
    /*!
        値を二乗する関数
        \param x 値
        \return xを二乗した値
    */
    template <typename T>
    T sqr(T x);

    //! A class.
    /*!
        二重振り子に対して運動方程式を解くクラス
    */
    class SolveEOM final {
        // #region 列挙型

        //!  A enumerated type
        /*!
            方程式を表す列挙型
        */
        enum Num_eqns {
            THETA_1 = 0,
            OMEGA_1,
            THETA_2,
            OMEGA_2,
            NUM_EQNS
        };

        // #endregion 列挙型

        //! A typedef.
        /*!
        */
        using state_type = std::array<double, Num_eqns::NUM_EQNS>;

        // #region コンストラクタ・デストラクタ

    public:
        //! A constructor.
        /*!
            唯一のコンストラクタ
            \param l ロープの長さ
            \param m 球の質量
            \param theta1_0 θ1の初期値
            \param theta2_0 θ2の初期値
        */
        SolveEOM(float l, float m, float theta1_0, float theta2_0);

        //! A destructor.
        /*!
            デフォルトデストラクタ
        */
        ~SolveEOM() = default;

        // #endregion コンストラクタ・デストラクタ

        // #region publicメンバ関数

        //! A public member function.
        /*!
            運動エネルギーを求める
            \return 運動エネルギー
        */
        float kinetic_energy() const;

        //! A public member function.
        /*!
            運動方程式を、指定された時間まで積分する
            \param dt 指定時間
            \return theta1 θ1の値
            \return theta2 θ2の値
        */
        void operator()(float dt, float * theta1, float * theta2);

        //! A public member function.
        /*!
            運動方程式を、指定された時間まで積分し、その結果を時間間隔dtごとにファイルに保存する
            \param dt 時間刻み
            \param filename 保存ファイル名
            \param t 指定時間
        */
        void operator()(double dt, std::string const & filename, double t);

        //! A public member function.
        /*!
            ポテンシャルエネルギーを求める
            \return ポテンシャルエネルギー
        */
        float potential_energy() const;
        
        // #endregion publicメンバ関数

    private:
        // #region privateメンバ関数

        //! A private member function.
        /*!
            運動方程式を返す
            \return 運動方程式のstd::function
        */
        std::function<void(state_type const &, state_type &, double const)> getEOM() const;

		//! A public member function.
		/*!
			ポテンシャルエネルギーを求める
			\return ポテンシャルエネルギー
		*/
		double total_energy() const;
		
        // #endregion privateメンバ関数

        // #region プロパティ

    public:
        //! A property.
        /*!
            θ1へのプロパティ
        */
        Property<float> Theta1;

        //! A property.
        /*!
            θ2へのプロパティ
        */
        Property<float> Theta2;
        
        //! A property.
        /*!
            速度v1へのプロパティ
        */
        Property<float> V1;

        //! A property.
        /*!
            速度v2へのプロパティ
        */
        Property<float> V2;
        
        // #endregion プロパティ

        // #region メンバ変数

    private:
        //! A private static member variable (constant expression).
        /*!
            Bulirsch-Stoer法の初期刻み値
        */
        static auto constexpr DX = 0.01;

        //! A private static member variable (constant expression).
        /*!
            許容誤差
        */
        static auto constexpr EPS = 1.0E-14;

        //! A private static member variable (constant expression).
        /*!
            重力加速度
        */
        static auto constexpr g = 9.80665;

        //! A private member variable.
        /*!
            棒の端から球までの長さ
        */
        double l_;

        //! A private member variable.
        /*!
            球の質量
        */
        double m_;

        //! A private member variable.
        /*!
            Bulirsch-Stoer法のBoost.ODEIntオブジェクト
        */
        boost::numeric::odeint::bulirsch_stoer<state_type> stepper_;

        //! A private member variable.
        /*!
            微分方程式の現在の状態
        */
        state_type x_;

        // #endregion メンバ変数

        // #region 禁止されたコンストラクタ・メンバ関数

        //! A private constructor (deleted).
        /*!
            デフォルトコンストラクタ（禁止）
        */
        SolveEOM() = delete;

        //! A private copy constructor (deleted).
        /*!
            コピーコンストラクタ（禁止）
        */
        SolveEOM(SolveEOM const &) = delete;

        //! A private member function (deleted).
        /*!
            operator=()の宣言（禁止）
            \param コピー元のオブジェクト（未使用）
            \return コピー元のオブジェクト
        */
        SolveEOM & operator=(SolveEOM const &) = delete;

        // #endregion 禁止されたコンストラクタ・メンバ関数
    };
    
    // #region template関数の実装

    template <typename T>
    inline T sqr(T x)
    {
        return x * x;
    }

    // #endregion template関数の実装
}

#endif  // _SOLVEEOM_H_
