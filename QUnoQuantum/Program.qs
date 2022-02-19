// <copyright file="Program.qs" company="Mooville">
//   Copyright (c) 2022 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Quantum {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Measurement;
    open Microsoft.Quantum.Intrinsic;

    //@EntryPoint()
    operation HelloQ() : Int {
        Message("Hello quantum world!");

        let max = 50;
        Message($"Creating random quantum numbers between 0 and {max}: ");

        return GenerateRandomNumberInRange(max);
    }

    operation GenerateRandomNumber() : Result {
        use q = Qubit();
        H(q);

        return MResetZ(q);
    }

    operation GenerateRandomNumberInRange(max : Int) : Int {
        mutable bits = new Result[0];

        for indexBit in 1..BitSizeI(max) {
            set bits += [GenerateRandomNumber()];
        }

        let sample = ResultArrayAsInt(bits);

        return (sample >  max) ? GenerateRandomNumberInRange(max) | sample;
    }

    operation SetQubitState(desired : Result, q1 : Qubit) : Unit {
        if desired != M(q1) {
            X(q1);
        }
    }

    @EntryPoint()
    operation TestBellState(count : Int, initial : Result) : (Int, Int, Int) {
        mutable numberOfOnes = 0;
        mutable agreements = 0;
        use (q0, q1) = (Qubit(), Qubit());

        for test in 1..count {
            SetQubitState(initial, q0);
            SetQubitState(Zero, q1);
            H(q0);
            CNOT(q0, q1);

            let res = M(q0);

            if M(q1) == res {
                set agreements += 1;
            }

            if res == One {
                set numberOfOnes += 1;
            }
        }

        SetQubitState(Zero, q0);
        SetQubitState(Zero, q1);

        Message("Test results are (# of 0s, # of 1s): ");

        return (count - numberOfOnes, numberOfOnes, agreements);
    }
}
