using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[]triangles;
   

   private Vector3 BarycentricCoordinates(Vector2 point1,Vector2 point2,Vector2 point3, Vector2 point)
    {
        Vector2 p12 = point2 - point1;
        Vector2 p13 = point3 - point1;
        Vector3 n = (Vector3.Cross(new Vector3(p12.x,0.0f ,p12.y) ,new Vector3(p13.x,0.0f ,p13.y)));
        float areal_123 = n.magnitude;
        Vector3 baryc;
        //u
        Vector2 p = point2 - point;
        Vector2 q = point3 - point;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.x = n.z / areal_123;
        //v
        p = point3 - point;
        q = point1 - point;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.y = n.z / areal_123;
        //w
        p = point1 - point;
        q = point2 - point;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.z = n.z / areal_123;
        return baryc;
    }
float Triangulation::GetSurfaceHeight(glm::vec3 p)
{
    // Loop through each triangle in the mesh.
    for (int i = 0; i < (numTriangles); i++)
    {
        // Get the vertices of the triangle.
        unsigned int v0 = getIndex(i, 0);
        unsigned int v1 = getIndex(i, 1);
        unsigned int v2 = getIndex(i, 2);
        glm::vec3 p0 = getVertex(v0);
        glm::vec3 p1 = getVertex(v1);
        glm::vec3 p2 = getVertex(v2);

        glm::vec3 baryCoords = barycentricCoordinates(p0, p1, p2, p);

        // Check if the player's position is inside the triangle.
        if (baryCoords.x >= 0.0f && baryCoords.y >= 0.0f && baryCoords.z >= 0.0f)
        {
            // The player's position is inside the triangle.
            // Calculate the height of the surface at the player's position.
            float height = baryCoords.x * p0.z + baryCoords.y * p1.z + baryCoords.z * p2.z;

            // Return the height as the height of the surface at the player's position.
            return height;
        }
    }

    return 0.0f;
}
   Vector3 GetSurfaceHeight()
   {
       
       
       return Height;
   }
   private Vector3 GetCollision( Vector3 barycentric)
   {

       Vector3 S = (barycentric.x* vertices)
       
       
       return ;
   }

    void Move()
    {
        // Iterate through each triangle 
        for (int i = 0; i <mesh.triangles.Length; i += 3)
        {
            // Find the vertices of the triangle
            Vector3 p0 = mesh.vertices[mesh.triangles[i]];
            Vector3 p1 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 p2 = mesh.vertices[mesh.triangles[i + 2]];

            // Find the balls position in the xz-plane
            Vector2 pos = new Vector2(_currentPos.x, _currentPos.z);

            // Find which triangle the ball is currently on with barycentric coordinates
            Vector3 baryCoords = BarycentricCoordinates(
                new Vector2(p0.x, p0.z),
                new Vector2(p1.x, p1.z),
                new Vector2(p2.x, p2.z),
                pos
            );

            if (baryCoords is { x: >= 0.0f, y: >= 0.0f, z: >= 0.0f })
            {
               // .......
            }
        }
    }



    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0.0f, 0.097f, 0.0f),
            new Vector3(0.4f, 0.005f, 0.0f),
            new Vector3(0.0f, 0.005f, 0.4f),
            new Vector3(0.8f, 0.007f, 0.4f),
            new Vector3(0.4f, 0.075f, 0.4f),
            new Vector3(0.8f, 0.039f, 0.0f)
        };

        triangles = new int[]
        {
            2, 1, 0,
            2, 4, 1,
            1, 4, 3,
            1, 3, 5
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

