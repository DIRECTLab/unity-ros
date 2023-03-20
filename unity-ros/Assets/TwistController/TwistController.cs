using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

[RequireComponent(typeof(Rigidbody))]
public class TwistController : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<TwistMsg>("cmd_vel", SetVelocity);
        rb = GetComponent<Rigidbody>();
    }

    void SetVelocity(TwistMsg msg)
    {
        rb.velocity = new Vector3((float)msg.linear.x, (float)msg.linear.y, (float)msg.linear.z);
        rb.angularVelocity = new Vector3((float)msg.angular.x, (float)msg.angular.y, (float)msg.angular.z);
    }
}
