#region License

//  Copyright 2015-2026 Drew Noakes, Krzysztof Dul
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
using System.Numerics;

namespace Boing;

/// <summary>
/// A force that works to keep point masses within a given rectangular boundary.
/// </summary>
/// <remarks>
/// No force is exerted on point masses within <see cref="Bounds"/>.
/// As a point moves outside the boundary, the force exerted back towards the boundary increases exponentially
/// (with power determined by <see cref="Magnitude"/>). Forces are capped at <see cref="MaximumForce"/>
/// to prevent destabilising the simulation.
/// </remarks>
public sealed class KeepWithinBounds3Force : IForce<Vector3>
{
    /// <summary>
    /// Gets and sets the rectangular bounds to which point masses are constrained.
    /// </summary>
    public Rectangle3f Bounds { get; set; }

    /// <summary>
    /// Gets and sets the exponential power to use when computing the force to apply
    /// to a point mass found outside <see cref="Bounds"/>, based on its distance from
    /// the boundary.
    /// </summary>
    public float Magnitude { get; set; }

    /// <summary>
    /// Gets and sets an upper limit on the force applied to particles, in Newtons,
    /// to prevent destabilising the simulation.
    /// </summary>
    public float MaximumForce { get; set; }

    /// <summary>
    /// Initialises a new instance of <see cref="KeepWithinBounds3Force"/>.
    /// </summary>
    /// <param name="bounds">The bounds to which point masses are constrained.</param>
    /// <param name="magnitude"></param>
    /// <param name="maximumForce">The upper limit on the force applied to particles. The default value is 1000 Newtons.</param>
    public KeepWithinBounds3Force(Rectangle3f bounds, float magnitude = 3.0f, float maximumForce = 1000.0f)
    {
        Bounds = bounds;
        Magnitude = magnitude;
        MaximumForce = maximumForce;
    }

    /// <inheritdoc />
    void IForce<Vector3>.ApplyTo(Simulation<Vector3> simulation)
    {
        foreach (var pointMass in simulation.PointMasses)
        {
            if (pointMass.IsPinned)
                continue;

            if (pointMass.Position.X > Bounds.Max.X)
            {
                var force = (float)Math.Pow(pointMass.Position.X - Bounds.Max.X, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(-force, 0, 0));
            }
            else if (pointMass.Position.X < Bounds.Min.X)
            {
                var force = (float)Math.Pow(Bounds.Min.X - pointMass.Position.X, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(force, 0, 0));
            }

            if (pointMass.Position.Y > Bounds.Max.Y)
            {
                var force = (float)Math.Pow(pointMass.Position.Y - Bounds.Max.Y, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(0, -force, 0));
            }
            else if (pointMass.Position.Y < Bounds.Min.Y)
            {
                var force = (float)Math.Pow(Bounds.Min.Y - pointMass.Position.Y, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(0, force, 0));
            }

            if (pointMass.Position.Z > Bounds.Max.Z)
            {
                var force = (float)Math.Pow(pointMass.Position.Z - Bounds.Max.Z, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(0, 0, -force));
            }
            else if (pointMass.Position.Z < Bounds.Min.Z)
            {
                var force = (float)Math.Pow(Bounds.Min.Z - pointMass.Position.Z, Magnitude);
                if (force > MaximumForce)
                    force = MaximumForce;
                pointMass.ApplyForce(new Vector3(0, 0, force));
            }
        }
    }
}