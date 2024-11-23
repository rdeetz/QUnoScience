# QUno

Scientific tools for an Uno-like card game.

## Requirements

* [.NET SDK](https://dotnet.microsoft.com/download)
* .NET 9 SDK for C# projects
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (I use the Community Edition, v17)
* [ML.NET Model Builder 2022](https://marketplace.visualstudio.com/items?itemName=MLNET.ModelBuilder2022)
* [Quantum Development Kit](https://marketplace.visualstudio.com/items?itemName=quantum.DevKit)
* .NET 9 SDK for Q# projects
* Your favorite editor (my favorite editor is [Visual Studio Code](https://code.visualstudio.com/))
* [Quantum Development Kit Extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=quantum.quantum-devkit-vscode)

## How To Play

`QUnoQuantum` is a console application for experiment with Q# and quantum concepts. Use `--help` to 
see the required options. For example:

```
QUnoQuantum --max 32 --count 8 --initial Zero
```

This will generate a random quantum numbers up to 32, and then will test the state 
of 8 qubits using an initial value of `Zero`.

`QUnoDoctor` is a console application _to be continued_.

## Developer Notes

_Coming soon._
