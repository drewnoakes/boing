[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)

A simple library for 2D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

## Usage

Build a `Simulation` comprising:

- `PointMass` objects
- Local forces (such as `Spring`s) 
- Global forces (such as gravity or Coloumb forces)

Periodically update the simulation with a time step.

## Example

```csharp
// create a new simulation
var simulation = new Simulation();

// create some point masses
var pointMass1 = new PointMass(mass: 1.0f);
var pointMass2 = new PointMass(mass: 2.0f);

simulation.Add(pointMass1);
simulation.Add(pointMass2);

// create a spring between these point masses
simulation.Add(new Spring(pointMass1, pointMass2));

// add some global forces to the simulation
simulation.Add(new ColoumbForce());                       // point masses are attracted to one another
simulation.Add(new OriginAttractionForce(stiffness: 10)); // point masses move towards the origin
simulation.Add(new FlowDownwardForce(magnitude: 100));    // gravity

// set up a loop
while (true)
{
    // compute a time step
    simulation.Update(dt: 0.01f);

    // use the resulting positions somehow
    Console.WriteLine($"PointMass1 at {pointMass1.Position}, PointMass2 at {pointMass2.Position}");
}
```

## License

Copyright 2015-2016 Drew Noakes, Krzysztof Dul

> Licensed under the Apache License, Version 2.0 (the "License");
> you may not use this file except in compliance with the License.
> You may obtain a copy of the License at
>
>     http://www.apache.org/licenses/LICENSE-2.0
>
> Unless required by applicable law or agreed to in writing, software
> distributed under the License is distributed on an "AS IS" BASIS,
> WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
> See the License for the specific language governing permissions and
> limitations under the License.
