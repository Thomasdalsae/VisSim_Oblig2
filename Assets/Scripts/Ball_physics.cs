using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball_physics : MonoBehaviour
{

   public MeshGenerator mesh;

    private float _radius = 0.020f;
    [SerializeField] private Vector3 _currentfPosition;
    [SerializeField] private Vector3 _previousPosition;
    [SerializeField] private Vector3 _currentVelocity;
    [SerializeField] private Vector3 _previousVelocity;
    
    //
    [SerializeField] private int _currentIndex;//(current triangle)
    [SerializeField] private int _previousIndex;//(Previous triangle)
    [SerializeField] private Vector3 _previousNormal;
    [SerializeField] private Vector3 _currentNormal;
    
    
    //Start locations
    
    [SerializeField] public Vector2 _startLocation = new Vector2(0.05f,0.05f);
    private float _startHeight;

    private void Start()
    {
        var _startHeight = mesh.GetSurfaceHeight(new Vector2(_startLocation.x,_startLocation.y));
        Debug.Log("herro" + _startHeight);
        _currentfPosition = new Vector3(_startLocation.x, _startHeight + _radius, _startLocation.y);
        _previousPosition = _currentfPosition;
        
        transform.position = _currentfPosition;
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

            // Find the vertices of the triangle
            Vector3 p0 = mesh.vertices[mesh.triangles[i]];
            Vector3 p1 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 p2 = mesh.vertices[mesh.triangles[i + 2]];

            // Find the balls position in the xz-plane
            Vector2 pos = new Vector2(_currentfPosition.x, _currentfPosition.z);

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
                _currentIndex = i / 3;
                _currentNormal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
                
                //bergen akselerasjonesvektor - ligning (8.12)
                // Vector3 acceleration = (1 / ballMass) * (normalVector + Physics.gravity);
                //Oppdaterer hastigheten og posisjon
                var Acceleration = -Physics.gravity.y * new Vector3((_currentNormal.x * _currentNormal.y),
                    (_currentNormal.y * _currentNormal.y - 1),
                    (_currentNormal.z * _currentNormal.y));
                //Oppdater hastighet og posisjon
                //ligning (8.14) og (8.15)

                _currentVelocity = _previousVelocity + Acceleration * Time.fixedDeltaTime;
                _previousVelocity = _currentVelocity;
                
                _currentfPosition = _previousPosition + _previousVelocity * Time.fixedDeltaTime;
                _previousPosition = _currentfPosition;
                transform.position = _currentfPosition;
                
                Debug.Log("hmm" + _currentIndex);
                Debug.Log("hmm" + _previousIndex);
                
                if (_currentIndex != _previousIndex)
                {
                    Debug.Log("triange" + i/3);
                    //ballen har Rullet over til en ny trekant
                    //beregn normaler  til kollisjonsplanet
                    // se ligningen(8.17)

                    var n = (_currentNormal + _previousNormal).normalized;


                    //Korrigere posisjon oppover i normalens retning
                    //oppdater hastighetsverkoren (8.16)
                    var afterCollisionVelocity =  _currentVelocity - 2 * Vector3.Dot(_currentVelocity , n) * n;
                    //oppdater posisjon i retning den nye hastighestvektoren
                    _currentVelocity = afterCollisionVelocity + Acceleration * Time.fixedDeltaTime;
                    _previousVelocity = _currentVelocity;
                
                    _currentfPosition = _previousPosition + _previousVelocity * Time.fixedDeltaTime;
                    _previousPosition = _currentfPosition;
                    transform.position = _currentfPosition;
                
                }
                //Oppdater gammel  normal og indeks
                _previousNormal = _currentNormal;
                _previousIndex = _currentIndex;
            }

            
        }
    }
}
