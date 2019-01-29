using Kalman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public float smooth = 5.0f;
    //float tiltAngle = -90.0f;
    private SerialRotate serialrotate;
    private bool isCalibrated;
    private MatrixKalmanWrapper matrixkalman;
    public Vector3 filteredRotation;
    public Vector4 current_phi;
    public Vector4 current_gyro;
    public float total_x;
    public float total_z;

    // Use this for initialization
    void Start()
    {
        serialrotate = GetComponent<SerialRotate>();
        matrixkalman = new MatrixKalmanWrapper();
        filteredRotation = new Vector3(0, 0, 0);
        current_phi = new Vector4(0, 0, 0);
        current_gyro = new Vector4(0, 0, 0); ;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCalibrated)
        {
            // Rotation for the Ring
            
            float tiltAroundRingZ = serialrotate.getRotationRing()[2] /** tiltAngle*/;
            float tiltAroundRingX = serialrotate.getRotationRing()[0] /** tiltAngle*/;
            float tiltAroundRingY = serialrotate.getRotationRing()[1] /** tiltAngle*/;

            //// Rotation of the Wrist
            float tiltAroundZ = serialrotate.getRotationWrist()[2];
            float tiltAroundX = serialrotate.getRotationWrist()[1];
            float tiltAroundY = serialrotate.getRotationWrist()[0];


            float gyroAroundRingZ = serialrotate.getGyroRing()[2] /** tiltAngle*/;
            float gyroAroundRingX = serialrotate.getGyroRing()[0] /** tiltAngle*/;
            float gyroAroundRingY = serialrotate.getGyroRing()[1] /** tiltAngle*/;

            //// Rotation of the Wrist
            float gyrotiltAroundZ = serialrotate.getGyrosWrist()[2];
            float gyrotiltAroundX = serialrotate.getGyrosWrist()[1];
            float gyrotiltAroundY = serialrotate.getGyrosWrist()[0];


            // Hier soll die Berechnung mithilfe des Kalman Filters stattfinden.
            //filteredRotation = matrixkalman.Update(new Vector3 (tiltAroundRingX, 0, tiltAroundRingZ));
            //filteredRotation = matrixkalman.Update(new Vector3 (tiltAroundX, tiltAroundY, tiltAroundZ));

           

            //current_phi.ringX, 
            current_phi[0] = tiltAroundRingX;
            current_phi[1] = tiltAroundX;
            current_phi[2] = tiltAroundRingY;
            current_phi[3] = tiltAroundY;

            //current_gyro.ringX, 
            current_phi[0] = gyroAroundRingX;
            current_phi[1] = gyrotiltAroundX;
            current_phi[2] = gyroAroundRingY;
            current_phi[3] = gyrotiltAroundY;

            filteredRotation = matrixkalman.Update(current_phi, current_gyro);


            // Rotation(tiltAroundX, tiltAroundZ);
            Rotation(filteredRotation.x, filteredRotation.y);
        }
    }

    public void Rotation( float tiltAroundX, float tiltAroundZ)
    {
        Debug.Log("Incoming: " + tiltAroundX + " & " + tiltAroundZ);
        Debug.Log("Computed: " + (tiltAroundX > 2 || tiltAroundX < -2 ? tiltAroundX : 0) + " & " + (tiltAroundZ > 1 || tiltAroundZ < -1 ? tiltAroundZ : 0));
        total_x = total_x + (tiltAroundX > 2 || tiltAroundX < -2 ? tiltAroundX : 0);
        total_z = total_z + (tiltAroundZ > 1 || tiltAroundZ < -1 ? tiltAroundZ : 0);
        Quaternion target = Quaternion.Euler(total_x * smooth, 0, total_z * smooth);
        // Quaternion target = Quaternion.Euler(tiltAroundX * smooth, 0, tiltAroundZ * smooth * 2);

        // Dampen towards the target rotation

        //transform.rotation = Quaternion.Slerp(transform.rotation, target, smooth);
        transform.rotation = target;
    }

    public void Calibrate() {

        isCalibrated = true;
        
        serialrotate.Calibrate();
        
    }

    // Invoked when a line of data is received from the serial device.
    public void OnMessageArrived(string msg)
    {
        // Debug.Log("message arrived");

        // Debug.Log( msg);
        serialrotate.processMessage(msg);
    }

}
