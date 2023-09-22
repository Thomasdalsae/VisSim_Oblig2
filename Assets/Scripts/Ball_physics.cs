using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball_physics : MonoBehaviour
{

   public MeshGenerator mesh;

    private Vector2 _currentPos;
    private float _radius = 0.020f;
    [SerializeField] private Vector3 _currentfPosition;
    [SerializeField] private Vector3 _previousPosition;
    [SerializeField] private Vector3 _currentVelocity;
    [SerializeField] private Vector3 _previousVelocity;
    
    //Start locations
    
    [SerializeField] private Vector2 _startLocation = new Vector2(0.06f,0.03f);
    private float _startHeight;
    private void Start()
    {
        var _startHeight =mesh.GetSurfaceHeight(new Vector2(_startLocation.x, _startLocation.y));
        _currentfPosition = new Vector3(_startLocation.x, _startHeight + _radius, _startLocation.y);
        _previousPosition = _currentfPosition;
    }

    private void FixedUpdate()
    {
        if (mesh)
        {
            Move();
        } 
    }

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
            Vector2 pos = new Vector2(_currentPos.x, _currentPos.y);

            // Find which triangle the ball is currently on with barycentric coordinates
            Vector3 baryCoords = mesh.BarycentricCoordinates(
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

                _currentVelocity = _previousVelocity + Acceleration * Time.fixedTime;
                _currentfPosition = _previousPosition + _previousVelocity * Time.fixedTime;

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


        }
    }
}
