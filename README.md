![boing logo](https://cdn.rawgit.com/drewnoakes/boing/master/Resources/logo.svg)

[![Build status](https://github.com/drewnoakes/boing/actions/workflows/ci.yml/badge.svg)](https://github.com/drewnoakes/boing/actions/workflows/ci.yml)
[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A simple library for 2D and 3D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

Boing supports `net47` (.NET Framework 4.7 and above) and `netstandard2.0` (.NET Standard 2.0 and above) for .NET Core and other platforms.

## Usage

Create either a 2D `Simulation<Vector2>` or a 3D `Simulation<Vector3>` comprising:

- `PointMass2` or `PointMass3` objects
- Dimension-specific forces such as `Spring2`, `Spring3`, `KeepWithinBounds2Force`, and `KeepWithinBounds3Force`
- Shared forces such as `ColoumbForce`, `FlowDownwardForce`, `OriginAttractionForce`, and `ViscousForce`

Periodically update the simulation with a time step.

## Example

This example creates a 2D simulation. To build a 3D simulation, use `Vector3`, `PointMass3`, and `Spring3` instead.

```csharp
// create some point masses
var pointMass1 = new PointMass2(mass: 1.0f);
var pointMass2 = new PointMass2(mass: 2.0f);

// create a new simulation
var simulation = new Simulation<Vector2>
{
    // add the point masses
    pointMass1,
    pointMass2,

    // create a spring between these point masses
    new Spring2(pointMass1, pointMass2, length: 20),

    // point masses are attracted to one another
    new ColoumbForce(),

    // point masses move towards the origin
    new OriginAttractionForce(stiffness: 10),

    // gravity
    new FlowDownwardForce(magnitude: 100)
};
```

With the simulation configured, we must run it step by step in a loop. You could do this in
several ways, but here are two common approaches.

#### Max speed

If you call `Update` as fast as possible, the simulation will most likely run faster than real time.

```csharp
void RunAtMaxSpeed()
{
    while (true)
    {
        // update the simulation
        simulation.Update(dt: 0.01f);

        // use the resulting positions somehow
        Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
    }
}
```

#### At a fixed rate

A more controlled approach is to use `FixedTimeStepUpdater`. This allows you to update the simulation
at one rate, and integrate the simulation's output at another rate. For example, you might run the
simulation at 200Hz, and render the output at 60Hz, as shown here:

```csharp
async Task RunAtFixedRateAsync(CancellationToken token)
{
    var updater = new FixedTimeStepUpdater<Vector2>(simulation, timeStepSeconds: 1f/200);

    while (!token.IsCancellationRequested)
    {
        updater.Update();

        // TODO render frame

        await Task.Delay(millisecondsDelay: 1/60, cancellationToken: token);
    }
}
```

For each `Render` call, the simulation will have multiple updates with smaller time deltas. 

This example code is `async`, but only to support a non-blocking delay. You could make it synchronous and use `Thread.Sleep` if you prefer.
