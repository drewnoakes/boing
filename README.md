[![Boing NuGet version](https://img.shields.io/nuget/v/Boing.svg)](https://www.nuget.org/packages/Boing/)

A simple library for 2D physics simulations in .NET.

## Installation

The easiest way to use this library is via its [NuGet package](https://www.nuget.org/packages/Boing/):

    PM> Install-Package Boing

## Usage

Build a `Graph` comprising `Node`s (point masses) connected via `Edge`s.

Create a `Physics` object for that graph and add various forces.

Then periodically update the physics object with a time step.

## Example

```csharp
// construct a graph (this is a very simple one)
var node1 = new Node("Node1", mass: 1.0f);
var node2 = new Node("Node2", mass: 2.0f);

var graph = new Graph();
graph.AddNode(node1);
graph.AddNode(node2);
graph.AddEdge(new Edge("Edge1", node1, node2));

var physics = new Physics(graph);

// add various forces to the universe
physics.Add(new ColoumbForce());                       // nodes are attracted
physics.Add(new HookeForce());                         // edges are springs
physics.Add(new OriginAttractionForce(stiffness: 10)); // nodes move towards the origin
physics.Add(new FlowDownwardForce(magnitude: 100));    // gravity

// set up a loop
while (true)
{
  // compute a time step
  physics.Update(dt: 0.01f);
  
  // use the resulting positions somehow
  Console.WriteLine($"Node1 at {node1.Position}, Node2 at {node2.Position}");
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