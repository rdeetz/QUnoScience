// <copyright file="Program.qs" company="Mooville">
//   Copyright (c) 2022 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Quantum {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Measurement;
    open Microsoft.Quantum.Intrinsic;

    operation GenerateRandomNumber() : Result {
        use q = Qubit();
        H(q);

        return MResetZ(q);
    }

    operation GenerateRandomNumberInRange(max : Int) : Int {
        mutable bits = [];

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

        return (count - numberOfOnes, numberOfOnes, agreements);
    }

    @EntryPoint()
    operation Main(max: Int, count : Int, initial : Result) : (Int, Int, Int) {
        Message("Hello quantum world!");

        let randomNumber = GenerateRandomNumberInRange(max);
        Message($"Creating random quantum number between 0 and {max}: {randomNumber}");

        Message("Testing the result of states (number of Zeros, number of Ones, number of Agreements): ");

        return TestBellState(count, initial);
    }
}
