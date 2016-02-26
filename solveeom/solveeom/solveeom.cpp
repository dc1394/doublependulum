/*! \file solveeom.cpp
    \brief 単振り子に対して運動方程式を解くクラスの実装

    Copyright ©  2016 @dc1394 All Rights Reserved.
    (but this is originally adapted by Freddie Witherden for doublependulum.cpp from https://freddie.witherden.org/tools/doublependulum/ )
    This software is released under the BSD 2-Clause License.
*/
#include "solveeom.h"
#include <cmath>                                // for std::sin, std::cos
#include <fstream>                              // for std::ofstream
#include <boost/assert.hpp>                     // for BOOST_ASSERT
#include <boost/format.hpp>                     // for boost::format
#include <boost/math/constants/constants.hpp>   // for boost::math::constants::pi

namespace solveeom {
    // #region コンストラクタ

    SolveEOM::SolveEOM(float l, float m, float theta1_0, float theta2_0) :
        Theta1([this] { return static_cast<float>(x_[0]); }, [this](auto theta) { return x_[0] = theta; }),
        Theta2([this] { return static_cast<float>(x_[2]); }, [this](auto theta) { return x_[2] = theta; }),
        V1([this] { return static_cast<float>(x_[1]); }, [this](auto v) { return x_[1] = v; }),
        V2([this] { return static_cast<float>(x_[3]); }, [this](auto v) { return x_[3] = v; }),
        l_(l),
        m_(m),
        stepper_(SolveEOM::EPS, SolveEOM::EPS)
    {
        
        x_ = { theta1_0, 0.0, theta2_0, 0.0 };
    }

    // #endregion コンストラクタ

    // #region publicメンバ関数
        
    void SolveEOM::operator()(float dt, float * theta1, float * theta2)
    {
        boost::numeric::odeint::integrate_adaptive(
            stepper_,
            getEOM(),
            x_,
            0.0,
            static_cast<double>(dt),
            SolveEOM::DX);

        *theta1 = static_cast<float>(x_[0]);
        *theta2 = static_cast<float>(x_[2]);
    }

    void SolveEOM::operator()(double dt, std::string const & filename, double t)
    {
        std::ofstream result(filename);

        boost::numeric::odeint::integrate_const(
            stepper_,
            getEOM(),
            x_,
            0.0,
            t,
            dt,
            [&result](auto const & x, auto const t)
        {
            result << boost::format("%.3f, %15f, %.15f\n") % t % x[0] % x[2];
        });
    }

    // #endregion publicメンバ関数

    // #region privateメンバ関数

    std::function<void(SolveEOM::state_type const &, SolveEOM::state_type &, double const)> SolveEOM::getEOM() const
    {
        auto const eom = [this](state_type const & x, state_type & dxdt, double const)
        {
            // Delta is θ2 - θ1
            const double delta = x[Num_eqns::THETA_2] - x[Num_eqns::THETA_1];

            // `Big-M' is the total mass of the system, m1 + m2;
            const double M = 2.0 * m_;

            // Denominator expression for ω1
            double den = M * l_ - m_ * l_ * std::cos(delta) * std::cos(delta);

            // dθ/dt = ω, by definition
            dxdt[Num_eqns::THETA_1] = x[Num_eqns::OMEGA_1];

            // Compute ω1
            dxdt[OMEGA_1] = (m_ * l_ * x[Num_eqns::OMEGA_1] * x[Num_eqns::OMEGA_1] * std::sin(delta) * std::cos(delta)
                + m_ * g * std::sin(x[Num_eqns::THETA_2]) * std::cos(delta)
                + m_ * l_ * x[Num_eqns::OMEGA_2] * x[Num_eqns::OMEGA_2] * std::sin(delta)
                - M * g * std::sin(x[THETA_1])) / den;

            // Again, dθ/dt = ω for θ2 as well
            dxdt[THETA_2] = x[OMEGA_2];

            // Multiply den by the length ratio of the two bobs
            den *= l_ / l_;

            // Compute ω2
            dxdt[Num_eqns::OMEGA_2] = (-m_ * l_ * x[Num_eqns::OMEGA_2] * x[Num_eqns::OMEGA_2] * std::sin(delta) * std::cos(delta)
                + M * g * std::sin(x[Num_eqns::THETA_1]) * std::cos(delta)
                - M * l_ * x[OMEGA_1] * x[Num_eqns::OMEGA_1] * std::sin(delta)
                - M * g * std::sin(x[Num_eqns::THETA_2])) / den;
        };

        return eom;
    }

    // #endregion privateメンバ関数
}