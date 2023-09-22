using System;
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
    private Mesh _mesh;

    Vector3[] vertices;
    int[] triangles;

    private float ballMass = 0.006f;

    public GameObject ball;

    private Vector2 _currentPos;

     private void Awake()
     {
         _mesh = new Mesh();
         GetComponent<MeshFilter>().mesh = _mesh;
         
         CreateShape();
         UpdateMesh();
         
     }
    
        void Start()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
    
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
            _mesh.Clear();
    
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }
    private Vector3 BarycentricCoordinates(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point)
    {
        Vector2 p12 = point2 - point1;
        Vector2 p13 = point3 - point1;
        Vector3 n = (Vector3.Cross(new Vector3(p12.x, 0.0f, p12.y), new Vector3(p13.x, 0.0f, p13.y)));
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

    private float GetSurfaceHeight(Vector2 p)
    {
        for (int i = 0; i < triangles.Length; i += 3)
        {
            var v0 = vertices[triangles[i]];
            var v1 = vertices[triangles[i + 1]];
            var v2 = vertices[triangles[i + 2]];

            Vector3 barcoords = BarycentricCoordinates(
                new Vector2(v0.x, v0.z),
                new Vector2(v1.x, v1.z),
                new Vector2(v2.x, v2.z),
                p);

            if (barcoords.x >= 0.0f && barcoords.y >= 0.0f && barcoords.z >= 0.0f)
            {
                float height = barcoords.x * v0.y + barcoords.y * v1.y + barcoords.z * v2.y;

                return height;
            }
        }

        return 0.0f;
    }


}

