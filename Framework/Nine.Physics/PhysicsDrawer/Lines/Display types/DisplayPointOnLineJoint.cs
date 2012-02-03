﻿using BEPUphysics.Constraints.TwoEntity.Joints;
using Microsoft.Xna.Framework;

namespace BEPUphysicsDrawer.Lines
{
    /// <summary>
    /// Graphical representation of a PointOnPlaneConstraint
    /// </summary>
    class DisplayPointOnLineJoint : SolverDisplayObject<PointOnLineJoint>
    {
        private readonly Line aToConnection;
        private readonly Line bToConnection;
        private readonly Line error;

        public DisplayPointOnLineJoint(PointOnLineJoint constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            aToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            bToConnection = new Line(Color.DarkBlue, Color.DarkBlue, drawer);
            error = new Line(Color.Red, Color.Red, drawer);
            myLines.Add(aToConnection);
            myLines.Add(bToConnection);
            myLines.Add(error);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            aToConnection.PositionA = LineObject.ConnectionA.Position;
            aToConnection.PositionB = LineObject.LineAnchor;

            bToConnection.PositionA = LineObject.ConnectionB.Position;
            bToConnection.PositionB = LineObject.ConnectionB.Position + LineObject.OffsetB;

            error.PositionA = aToConnection.PositionB;
            error.PositionB = bToConnection.PositionB;
        }
    }
}