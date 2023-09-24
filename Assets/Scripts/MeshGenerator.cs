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
         for (int i = 0; i < vertices.Length; i++)
         {
             Debug.Log("vertices" + vertices[0]);
         }
         
         textHandlingTriangles();
         //CreateShape();
         UpdateMesh();
         
     }
    
       /*
        void Start()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
    
            CreateShape();
            UpdateMesh();
        }
*/
     
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

    void txtHandlingVertices()
    {
        Debug.Log("Entering vertices func" );
        List<Vector3> temp = new List<Vector3>();
        if (txtFileVertices != null)
        {
            
            CultureInfo cultureInfo = new CultureInfo("en-US");
            string[] textInfo = txtFileVertices.text.Split('\n'); //this is the content as string
            
            foreach (var line in textInfo)
            {
                Debug.Log("line" + line);
                var data = line.Split(' ');
                if (data.Length == 3)
                {
                    float x = float.Parse(data[0],cultureInfo);
                    float y = float.Parse(data[1],cultureInfo);
                    float z = float.Parse(data[2],cultureInfo);
                    /*
                    Debug.Log("X" + x);
                    Debug.Log("Y" + y);
                    Debug.Log("Z" + z);
                    */
                    temp.Add(new Vector3(x,y,z));
                }

                
                foreach (var VARIABLE in temp)
                {
                    Debug.Log("ssssssssssssssssssssssssssssssssss" + VARIABLE.ToString("F4")  );
                    
                }
                
                    vertices = temp.ToArray();
            }
            
        }
               //Debug.Log("TextFile has not been found");
    }

    void textHandlingTriangles()
    {
        Debug.Log("Entering Triangle func" );
        List<int> temp = new List<int>();
               if (txtFileTriangle != null)
               {
                   
                   CultureInfo cultureInfo = new CultureInfo("en-US");
                   string[] textInfo = txtFileTriangle.text.Split('\n'); //this is the content as string
                   
                   foreach (var line in textInfo)
                   {
                       Debug.Log("line" + line);
                       var data = line.Split(' ');
                       if (data.Length == 6)
                       {
                           temp.Add(int.Parse(data[0],cultureInfo));
                           temp.Add(int.Parse(data[1],cultureInfo));
                           temp.Add(int.Parse(data[2],cultureInfo));
                           
                           
                       }
       
                       foreach (var VARIABLE in temp)
                       {
                           Debug.Log("Triangle Temp check" + VARIABLE.ToString()  );
                       }
                       triangles = temp.ToArray();
                   }
                   
               } 
              // Debug.Log("TextFile has not been found");
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
                /*
                Debug.Log("first for if statement" + barcoords.x);
                Debug.Log("first for if statement" + barcoords.y);
                Debug.Log("first for if statement" + barcoords.z);
                Debug.Log("first for if statement" + v0.x);
                Debug.Log("first for if statement" + v0.y);
                Debug.Log("first for if statement" + v0.z);
                Debug.Log("first for if statement" + height);
                */
                return height;
            }
        }

        return 0.0f;
    }


}

