#region License

//  Copyright 2015-2017 Drew Noakes, Krzysztof Dul
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

namespace Boing
{
    public sealed class KeepWithinBoundsForce : IForce
    {
        public Rectangle2f Bounds { get; set; }
        public float Magnitude { get; set; }
        public float MaximumForce { get; set; }

        public KeepWithinBoundsForce(Rectangle2f bounds, float magnitude = 3.0f, float maximumForce = 1000.0f)
        {
            Bounds = bounds;
            Magnitude = magnitude;
            MaximumForce = maximumForce;
        }

        /// <inheritdoc />
        void IForce.ApplyTo(Simulation simulation)
        {
            foreach (var pointMass in simulation.PointMasses)
            {
                if (pointMass.IsPinned)
                    continue;

                if (pointMass.Position.X > Bounds.Right)
                {
                    var force = (float)Math.Pow(pointMass.Position.X - Bounds.Right, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(-force, 0));
                }
                else if (pointMass.Position.X < Bounds.Left)
                {
                    var force = (float)Math.Pow(Bounds.Left - pointMass.Position.X, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(force, 0));
                }

                if (pointMass.Position.Y > Bounds.Bottom)
                {
                    var force = (float)Math.Pow(pointMass.Position.Y - Bounds.Bottom, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(0, -force));
                }
                else if (pointMass.Position.Y < Bounds.Top)
                {
                    var force = (float)Math.Pow(Bounds.Top - pointMass.Position.Y, Magnitude);
                    if (force > MaximumForce)
                        force = MaximumForce;
                    pointMass.ApplyForce(new Vector2f(0, force));
                }
            }
        }
    }
}