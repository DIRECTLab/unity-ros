using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

/// <summary>
/// Planar Lidar that publishes to a ROS core
/// Adapted from https://github.com/fsstudio-team/ZeroSimROSUnity/blob/master/Runtime/Scripts/Sensors/LIDAR/ZOLIDAR2D.cs
/// </summary>

public class PlanarLidar : MonoBehaviour
{
    ROSConnection ros;

    [Header("ROS Topic Name")]
    public string topicName = "scan";

    private float timeElapsed;

    [Header("Timing Settings")]
    public float scanTimeSeconds = 0.2f; // number of seconds between scans

    [Header("FOV Settings")]
    public float minAngle = 0.0f;
    public float maxAngle = 360.0f;

    [Header("Resolution")]
    public float angleIncrement = 4.0f;

    [Header("Range Settings")]
    public float minRangeDistanceMeters = 0;
    public float maxRangeDistanceMeters = 100;

    [Header("Debug")]
    public bool isDebug = false;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<LaserScanMsg>(topicName);
    }

    private float[] gatherHits()
    {
        float[] ranges = new float[(int)((maxAngle - minAngle) / angleIncrement)];
        for (int i = 0; i < ranges.Length; i++)
        {
            ranges[i] = 0;
            Vector3 axis = new Vector3(0, minAngle - angleIncrement * i, 0);
            Vector3 direction = Quaternion.Euler(axis) * transform.forward;
            Ray ray = new Ray(transform.position, direction);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, maxRangeDistanceMeters))
            {
                if (hit.distance >= minRangeDistanceMeters && hit.distance <= maxRangeDistanceMeters)
                {
                    ranges[i] = hit.distance;
                }

                if (isDebug)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green, scanTimeSeconds);
                }
            }
        }

        return ranges;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > scanTimeSeconds)
        {
            HeaderMsg headerMsg = new HeaderMsg();

            float[] ranges = gatherHits();

            LaserScanMsg msg = new LaserScanMsg(
                headerMsg,
                minAngle,
                maxAngle,
                angleIncrement,
                scanTimeSeconds / ((maxAngle - minAngle) / angleIncrement),
                scanTimeSeconds,
                minRangeDistanceMeters,
                maxRangeDistanceMeters,
                ranges,
                new float[ranges.Length]
                );

            ros.Publish(topicName, msg);

            timeElapsed = 0;
        }
    }
}
