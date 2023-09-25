using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;

    public Vector3[] vertices;
    public int[] triangles;

    //Text Handling
    public TextAsset txtFileVertices;
    public TextAsset txtFileTriangle;
    
    public GameObject ball;
    private Vector2 _currentPos;

     private void Awake()
     {
         
         _mesh = new Mesh();
         GetComponent<MeshFilter>().mesh = _mesh;
         
         txtHandlingVertices();
         textHandlingTriangles();
         UpdateMesh();
         
     }

        void UpdateMesh()
        {
            _mesh.Clear();
    
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

    void txtHandlingVertices()
    {
        Debug.Log("Entering vertices func" );
        List<Vector3> temp = new List<Vector3>();
        if (txtFileVertices != null)
        {
            
            CultureInfo cultureInfo = new CultureInfo("en-US");
            string[] textInfo = txtFileVertices.text.Split('\n'); //this is the content as string

            for (int i = 1; i < textInfo.Length; i++)
            {
                var data = textInfo[i].Split(' ');
                if (data.Length == 3)
                {
                    float x = float.Parse(data[0],cultureInfo);
                    float y = float.Parse(data[1],cultureInfo);
                    float z = float.Parse(data[2],cultureInfo);
               
                    temp.Add(new Vector3(x,y,z));
                }
                    vertices = temp.ToArray();
            }
            
        }
    }

    void textHandlingTriangles()
    {
        Debug.Log("Entering Triangle func" );
        List<int> temp = new List<int>();
               if (txtFileTriangle != null)
               {
                   
                   CultureInfo cultureInfo = new CultureInfo("en-US");
                   string[] textInfo = txtFileTriangle.text.Split('\n'); //this is the content as string
                   Debug.Log("text lengt " + textInfo.Length);
                   for (int k = 1; k < textInfo.Length;k++)
                   {
                       var data = textInfo[k].Split(' ');
                       if (data.Length == 6)
                       {
                           
                           Debug.Log("pain" + data[0] + data[1]+data[2]);
                           temp.Add(int.Parse(data[0],cultureInfo));
                           temp.Add(int.Parse(data[1],cultureInfo));
                           temp.Add(int.Parse(data[2],cultureInfo));
                       }

                   }
                       triangles = temp.ToArray();
               } 
    } 
    
      
        public Vector3 BarycentricCoordinates(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point)
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
        baryc.x = n.y / areal_123;
        //v
        p = point3 - point;
        q = point1 - point;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.y = n.y / areal_123;
        //w
        p = point1 - point;
        q = point2 - point;
        n = Vector3.Cross(new Vector3(p.x, 0.0f, p.y), new Vector3(q.x, 0.0f, q.y));
        baryc.z = n.y / areal_123;
        return baryc;
    }

    public float GetSurfaceHeight(Vector2 p)
    {
        for (int i = 0; i < triangles.Length; i += 3)
        {
           // Debug.Log("first for loop");
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

