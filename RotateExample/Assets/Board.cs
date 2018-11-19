using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public float smooth = 5.0f;
    float tiltAngle = -90.0f;
    private SerialRotate serialrotate;
    private bool isCalibrated;


    // Use this for initialization
    void Start()
    {
        serialrotate = GetComponent<SerialRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCalibrated)
        {
            // Smoothly tilts a transform towards a target rotation.
            //float tiltAroundZ = serialrotate.getRotationRing()[1] * tiltAngle;
            //float tiltAroundX = serialrotate.getRotationRing()[0] * tiltAngle;

            //// Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = serialrotate.getRotationWrist()[1] * tiltAngle;
            float tiltAroundX = serialrotate.getRotationWrist()[0] * tiltAngle;

            // Hier soll die Berechnung mithilfedes Kalman Fehlers stattfinden.

            Rotation(tiltAroundX, tiltAroundZ);
        }
    }

    public void Rotation( float tiltAroundX, float tiltAroundZ) {
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }

    public void Calibrate() {

        isCalibrated = true;
        serialrotate.Calibrate();
    }

}
