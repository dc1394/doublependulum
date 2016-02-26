/*! \file solveeommain.h
    \brief 単振り子に対して運動方程式を解く関数群の宣言

    Copyright ©  2016 @dc1394 All Rights Reserved.
    This software is released under the BSD 2-Clause License.
*/
#ifndef _SOLVEEOMMAIN_H_
#define _SOLVEEOMMAIN_H_

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

#include "solveeom.h"
#include <boost/optional.hpp>   // for boost::optional

extern "C" {
    //! A global variable.
    /*!
        SolveEOMクラスのオブジェクトへのポインタ
    */
    static boost::optional<solveeom::SolveEOM> pse;
    
    //! A global function.
    /*!
        角度θ1の値に対するgetter
        \return 角度θ1
    */
    DLLEXPORT float __stdcall gettheta1();
    
    //! A global function.
    /*!
        角度θ2の値に対するgetter
        \return 角度θ1
    */
    DLLEXPORT float __stdcall gettheta2();
    
    //! A global function.
    /*!
        速度v1の値に対するgetter
        \return 速度v1
    */
    DLLEXPORT float __stdcall getv1();

    //! A global function.
    /*!
        速度v2の値に対するgetter
        \return 速度v2
    */
    DLLEXPORT float __stdcall getv2();
    
    //! A global function.
    /*!
        seオブジェクトを初期化する
        \param l ロープの長さ
        \param m 球の質量
        \param theta1_0 θ1の初期値
        \param theta2_0 θ2の初期値
    */
    DLLEXPORT void __stdcall init(float l, float m, float theta1_0, float theta2_0);

    //! A global function.
    /*!
        次のステップを計算する
        \param dt 経過した時間
        \return theta1 θ1の値
        \return theta2 θ2の値
    */
    DLLEXPORT void __stdcall nextstep(float dt, float * theta1, float * theta2);

    //! A global function.
    /*!
        運動方程式を、指定された時間まで積分し、その結果を時間間隔Δtごとにファイルに保存する
        \param dt 時間刻み
        \param filename 保存ファイル名
        \param t 指定時間
    */
    DLLEXPORT void __stdcall saveresult(double dt, std::string const & filename, double t);

    //! A global function.
    /*!
        角度θ1の値に対するsetter
        \param theta 設定する角度θ1
    */
    DLLEXPORT void __stdcall settheta1(float theta);

    //! A global function.
    /*!
        角度θ2の値に対するsetter
        \param theta 設定する角度θ2
    */
    DLLEXPORT void __stdcall settheta2(float theta);
    
    //! A global function.
    /*!
        速度v1の値に対するsetter
        \return 設定する速度v1
    */
    DLLEXPORT void __stdcall setv1(float v);
    
    //! A global function.
    /*!
        速度v2の値に対するsetter
        \return 設定する速度v2
    */
    DLLEXPORT void __stdcall setv2(float v);
}

#endif  // _SOLVEEOMMAIN_H_

