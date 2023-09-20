using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_physics : MonoBehaviour
{
    
    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private Vector3 previousVelocity;
    [SerializeField] private int acceleration = 0;
    [SerializeField] private int mass = 1;
    
   // Vector3 normalForce = -Vector3.Dot(normalUnitVector,9.81f*mass)*normalUnitVector
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        //Vector3 newPosition = previousPosition + newVelocity * Time.fixedDeltaTime;
        
        //Sprettballmetoden

        //Vector3 Vnormal = Vector3.Dot(currentVelocity, normalUnitVector) * normalUnitVector;
        //Vector3 newVelocity = currentVelocity - 2 * Vnormal;

    }
}
