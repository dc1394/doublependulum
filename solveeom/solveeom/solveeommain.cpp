/*! \file solveeommain.cpp
    \brief 単振り子に対して運動方程式を解く関数群の実装

    Copyright ©  2016 @dc1394 All Rights Reserved.
    This software is released under the BSD 2-Clause License.
*/
#include "solveeommain.h"
#include <boost/utility/in_place_factory.hpp>   // boost::in_place

extern "C" {
    float __stdcall gettheta1()
    {
        return pse->Theta1();
    }

    float __stdcall gettheta2()
    {
        return pse->Theta2();
    }

    float __stdcall getv1()
    {
        return pse->V1();
    }

    float __stdcall getv2()
    {
        return pse->V2();
    }

    void __stdcall init(float l, float m, float theta1_0, float theta2_0)
    {
        pse = boost::in_place(l, m, theta1_0, theta2_0);
    }
    
    float __stdcall kinetic_energy()
    {
        return pse->kinetic_energy();
    }

    void __stdcall nextstep(float dt, float * theta1, float * theta2)
    {
        (*pse)(dt, theta1, theta2);
    }

    float __stdcall potential_energy()
    {
        return pse->potential_energy();
    }

    void __stdcall saveresult(double dt, std::string const & filename, double t)
    {
        (*pse)(dt, filename, t);
    }

    void __stdcall settheta1(float theta)
    {
        pse->Theta1(theta);
    }

    void __stdcall settheta2(float theta)
    {
        pse->Theta2(theta);
    }

    void __stdcall setv1(float v)
    {
        pse->V1(v);
    }

    void __stdcall setv2(float v)
    {
        pse->V2(v);
    }
}
