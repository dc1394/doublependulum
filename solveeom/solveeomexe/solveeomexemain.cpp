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

    init(1.0f, 0.05f, 1.5707963f, 1.0471976f);
    saveresult(0.001, "double_pendulum_60.csv", 30.0);

    init(1.0f, 0.05f, 1.5707963f, 1.5707963f);
    saveresult(0.001, "double_pendulum_90.csv", 30.0);

    init(1.0f, 0.05f, 3.1241394f, 3.1241394f);
    saveresult(0.001, "double_pendulum_179.csv", 30.0);

    init(1.0f, 0.05f, 3.1258847f, 3.1258847f);
    saveresult(0.001, "double_pendulum_179_1.csv", 30.0);

    return 0;
}
