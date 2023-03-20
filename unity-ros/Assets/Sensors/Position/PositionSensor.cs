using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;

[RequireComponent(typeof(Rigidbody))]
public class PositionSensor : MonoBehaviour
{
    ROSConnection ros;
    Rigidbody rb;

    [Header("ROS Topic Name")]
    public string topicName = "odom";

    [Header("ROS Timings")]
    public float timeBetweenUpdateSeconds = 0.16f;
    private float elapsedTime = 0;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<OdometryMsg>(topicName);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeBetweenUpdateSeconds)
        {
            HeaderMsg header = new();
            header.frame_id = "odom";
            PointMsg position = new(rb.position.x, rb.position.y, rb.position.z);
            QuaternionMsg orientation = new(rb.rotation.x, rb.rotation.y, rb.rotation.z, rb.rotation.w);
            PoseMsg pose = new(position, orientation);

            Vector3Msg twistLinear = new(rb.velocity.x, rb.velocity.y, rb.velocity.z);
            Vector3Msg twistAngular = new(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z);
            TwistMsg twist = new(twistLinear, twistAngular);

            PoseWithCovarianceMsg poseWithCovarianceMsg = new();
            poseWithCovarianceMsg.pose = pose;
            TwistWithCovarianceMsg twistWithCovarianceMsg = new();
            twistWithCovarianceMsg.twist = twist;

            OdometryMsg odometryMsg = new OdometryMsg(header, "base_link", poseWithCovarianceMsg, twistWithCovarianceMsg);

            ros.Publish(topicName, odometryMsg);

            elapsedTime = 0;
        }
    }
}
