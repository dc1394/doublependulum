/*! \file solveeomexemain.cpp
    \brief メイン関数

    Copyright ©  2016 @dc1394 All Rights Reserved.
    This software is released under the BSD 2-Clause License.
*/
#include "../solveeom/solveeommain.h"

int main()
{
    init(1.0f, 0.05f, 0.1745329f, 0.1745329f);
    saveresult(0.001, "double_pendulum_10.csv", 30.0);

    init(1.0f, 0.05f, 0.5235988f, 0.5235988f);
    saveresult(0.001, "double_pendulum_30.csv", 30.0);

    init(1.0f, 0.05f, 1.5533430f, 1.5533430f);
    saveresult(0.001, "double_pendulum_89.csv", 30.0);

    init(1.0f, 0.05f, 1.5550884f, 1.5550884f);
    saveresult(0.001, "double_pendulum_89_1.csv", 30.0);

    return 0;
}
