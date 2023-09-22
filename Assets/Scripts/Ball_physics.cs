using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball_physics : MonoBehaviour
{

 public GameObject ball;

 public MeshGenerator mesh;
    private Vector2 _currentPos;

    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private Vector3 previousVelocity;

    private void Awake()
    {
    }

    void Move()
    {
            // Iterate through each triangle 
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                
                var currentIndex = i / 3;
                var previousIndex = i;
                // Find the vertices of the triangle
                Vector3 p0 = mesh.vertices[mesh.triangles[i]];
                Vector3 p1 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 p2 = mesh.vertices[mesh.triangles[i + 2]];
    
                // Find the balls position in the xz-plane
                Vector2 pos = new Vector2( _currentPos.x, _currentPos.y);
    
                // Find which triangle the ball is currently on with barycentric coordinates
                Vector3 baryCoords = BarycentricCoordinates(
                    new Vector2(p0.x, p0.z),
                    new Vector2(p1.x, p1.z),
                    new Vector2(p2.x, p2.z),
                    pos
                );
    
                if (baryCoords is { x: >= 0.0f, y: >= 0.0f, z: >= 0.0f })
                {
                    //beregne normal
                    Vector3 normalVector = Vector3.Cross(p1 - p0, p2 - p0).normalized;
    
                    //bergen akselerasjonesvektor - ligning (8.12)
                   // Vector3 acceleration = (1 / ballMass) * (normalVector + Physics.gravity);
                    //Oppdaterer hastigheten og posisjon
                    var Acceleration = Physics.gravity.y * new Vector3((normalVector.x * normalVector.y),
                        (normalVector.y * normalVector.y - 1),
                        (normalVector.z * normalVector.y));
                    //Oppdater hastighet og posisjon
                    //ligning (8.14) og (8.15)
                    
                    currentVelocity = previousVelocity + Acceleration * Time.fixedTime;
                    currentPosition = previousPosition + previousVelocity * Time.fixedTime;
    
                    if (currentIndex != previousIndex) 
                    {
                        //ballen har Rullet over til en ny trekant
                        //beregn normaler  til kollisjonsplanet
                        // se ligningen(8.17)
                        
                        //Korrigere posisjon oppover i normalens retning
                        //oppdater hastighetsverkoren (8.16)
                        //oppdater posisjon i retning den nye hastighestvektoren
                    }
                    //Oppdater gammel  normal og indeks
                }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }
}
