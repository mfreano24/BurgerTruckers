using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckDriver : MonoBehaviour
{
    /// <summary>
    /// Unity Wheel Collider controller
    /// </summary>
    public List<Axle> axles;
    public float maxTorque;
    public float maxSteer;
    public float brakeTorque;

    public bool truckMode = false;

    public GameManager gm;
    bool initialBoost = false;

    AudioSource aud;
    Rigidbody rb;
    
    void Start()
    {
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        if (!gm.paused)
        {
            if (truckMode)
            {
                float motor = maxTorque;
                if (Input.GetButton("Submit"))
                {
                    motor = brakeTorque;
                }
                float steer = maxSteer * Input.GetAxis("Horizontal");
                foreach (Axle a in axles)
                {
                    if (a.steering)
                    {
                        a.leftWheel.steerAngle = steer;
                        a.rightWheel.steerAngle = steer;
                    }

                    if (a.motor)
                    {
                        a.leftWheel.motorTorque = -motor;
                        a.rightWheel.motorTorque = -motor;
                    }
                }

            }
            else if (!truckMode && gm.requestsDone > 0)
            {
                float motor = maxTorque;
                foreach (Axle a in axles)
                {
                    if (a.motor)
                    {
                        a.leftWheel.motorTorque = -motor;
                        a.rightWheel.motorTorque = -motor;
                    }
                }
            }

            if(rb.velocity.magnitude > 0 && gm.requestsDone > 0)
            {
                aud.pitch = rb.velocity.magnitude * 0.025f + 1.0f;
                if(aud.pitch >= 2)
                {
                    aud.pitch = 2;
                }
            }

        }
        
        
    }

    int InAccel()
    {
        if (Input.GetButton("Submit"))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

[System.Serializable]
public class Axle
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
