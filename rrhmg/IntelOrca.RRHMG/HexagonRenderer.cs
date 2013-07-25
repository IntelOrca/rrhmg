#region RRHMG, © Copyright Ted John 2013
// Recursive random hexagon map generator
// Intelorca.RRHMG
// 
// Universiy of Manchester, Computer Science
// Third year project
//
// Application to generate random hexagonal maps of terrain where the map can recurse indefinitely
// in the same manner as a fractal.
// 
// © Copyright Ted John 2013
#endregion

using System;
using OpenTK.Graphics.OpenGL;

namespace IntelOrca.RRHMG
{
    /// <summary>
    /// Functionality for drawing hexagons.
    /// </summary>
    class HexagonRenderer
    {
        public void Render(Viewport viewport, Hexagon hexagon)
        {
            DrawHexagon(400, 300, 256, Colour.White);
        }

        private void DrawHexagon(double x, double y, double radius, Colour colour)
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0);

            // Set the colour
            GLSetColour(colour);
            
            GL.Begin(BeginMode.Triangles);

            // Calculate seven radial points where the first and last point are the same
            var corners = new Point[7];
            for (int i = 0; i < 6; i++) {
                double angle = 2.0 * Math.PI / 6.0 * i;
                corners[i] = new Point(
                    radius * Math.Cos(angle),
                    radius * Math.Sin(angle)
                );
            }
            corners[6] = corners[0];

            // Draw six triangles all from the centre of the hexagon
            for (int i = 0; i < 6; i++) {
                GL.Vertex3(0.0, 0.0, 0.0);
                GL.Vertex3(corners[i].X, corners[i].Y, 0.0);
                GL.Vertex3(corners[i + 1].X, corners[i + 1].Y, 0.0);
            }

            GL.End();
            GL.PopMatrix();
        }

        private void GLSetColour(Colour colour)
        {
            GL.Color4(colour.Red, colour.Green, colour.Blue, colour.Alpha);
        }
    }
}
