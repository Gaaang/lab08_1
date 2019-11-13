using System.Drawing;
using System;
using System.Linq;

namespace AffineTransformationsIn3D.Geometry
{
    public class MeshWithNormals : Mesh
    {
        public Vector[] Normals { get; set; }
        public bool VirisbleNormals { get; set; } = false;

        public MeshWithNormals(Vector[] vertices, Vector[] normals, int[][] indices) : base(vertices, indices)
        {
            Normals = normals;
        }

        public override void Apply(Matrix transformation)
        {
            var normalTransformation = transformation.Inverse().Transpose();
            for (int i = 0; i < Coordinates.Length; ++i)
            {
                Coordinates[i] *= transformation;
                Normals[i] = (Normals[i] * normalTransformation);
            }
        }

        public override void Draw(Graphics3D graphics)
        {
            graphics.DrawPoint(new Vertex(new Vector(0.5, 0.5, 0.5), Color.Yellow));
            graphics.DrawLine(new Vertex(new Vector(0.5, 0.5, 0.5),Color.Yellow), new Vertex(new Vector(0, 0, 0),Color.Yellow));
            foreach (var facet in Indices)
            {
                
                var verts = facet.Distinct().ToArray();
                Vector norm = Normals[verts[0]];
                norm = Vector.CrossProduct((Coordinates[facet[0]]-Coordinates[facet[1]]), (Coordinates[facet[1]] - Coordinates[facet[2]]));




                if (Vector.AngleBet((new Vector(0.5, 0.5, 0.5)), norm) > Math.PI/2)  
                    for (int i = 0; i < facet.Length; ++i)
                    {
                        var a = new Vertex(Coordinates[facet[i]], Color.White, Normals[facet[i]]);
                        var b = new Vertex(Coordinates[facet[(i + 1) % facet.Length]] , Color.White, Normals[facet[(i + 1) % facet.Length]]);
                        graphics.DrawLine(a,b);
                    }
               
            }
        }
    }
}
