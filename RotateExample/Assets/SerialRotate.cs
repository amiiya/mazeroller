using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialRotate : MonoBehaviour {
    public SerialController serialController;
    public bool UseGyro;
    public bool UseAccelerometer;
    public float alpha;
    Vector3 lastGyroRate, lastAccAngles;

    public Text[] valuesWristAngle,valuesWristAcc, valuesRingAngle,valuesRingAcc;

    private string[] oldtxyz;
    private float[] calAgWrist,calAccWrist,calAgRing,calAccRing, angleAccRing, angleAccWrist;
    private float[] agWrist, accWrist, agRing, accRing;
    private bool isCalibrated;
    private string[] txyz = new string[12];
    

    // Use this for initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        oldtxyz = new string[12];
        calAgRing = new float[3];
        calAccRing = new float[3]; 
        calAgWrist = new float[3];
        calAccWrist = new float[3];
        agWrist = new float[3];
        accWrist = new float[3];
        angleAccRing = new float[3];
        angleAccWrist = new float[3];

        agRing = new float[3];
        accRing = new float[3];
        for (int i=0;i<oldtxyz.Length;i++)
        {
            oldtxyz[i] = "0";
        }
    }

    public void processMessage(string message)
    {

 		//if (message.StartsWith("D.ypr:"))
        //{
        //    message = message.Substring(6);
        //    string[] ypr = message.Split(',');
        //    float yaw = float.Parse(ypr[0]);
        //    float pitch = float.Parse(ypr[1]);
        //    float roll = float.Parse(ypr[2]);
        //    Vector3 yprvec = new Vector3(-pitch, yaw, -roll);

        //    transform.eulerAngles = yprvec;
        //}

        if (message.StartsWith(""))
        {
            ///message = message.Substring(10);
            Debug.Log("length:"+ message.Length);
            txyz = message.Split('\t');

                 //angle and acc of ring
                agRing[0] = float.Parse(txyz[0])-calAgRing[0];
                agRing[1] = float.Parse(txyz[1]) - calAgRing[1];
                agRing[2] = float.Parse(txyz[2]) - calAgRing[2];

                accRing[0] = float.Parse(txyz[3]) - calAccRing[0];
                accRing[1] = float.Parse(txyz[4]) - calAccRing[1];
                accRing[2] = float.Parse(txyz[5]) - calAccRing[2];

                angleAccRing[0] = -Mathf.Atan2(accRing[2], accRing[1]) * 180 / Mathf.PI;
                angleAccRing[1] = 0;
                angleAccRing[2] = -90 + Mathf.Atan2(accRing[1], accRing[0]);


                //angle and acc of wrist 
                agWrist[0] = float.Parse(txyz[6])-calAgWrist[0];
                agWrist[1] = float.Parse(txyz[7]) - calAgWrist[1];
                agWrist[2] = float.Parse(txyz[8]) - calAgWrist[2];
                accWrist[0] = float.Parse(txyz[9])-calAccWrist[0];
                accWrist[1] = float.Parse(txyz[10]) - calAccWrist[1];
                accWrist[2] = float.Parse(txyz[11]) - calAccWrist[2];

                angleAccWrist[0] = -Mathf.Atan2(accRing[2], accRing[1]) * 180 / Mathf.PI;
                angleAccWrist[1] = 0;
                angleAccWrist[2] = -90 + Mathf.Atan2(accRing[1], accRing[0]);

                //lastGyroRate.Set(grx, gry, grz);

            //values of wrist / Handgelenk
                valuesWristAngle[0].text = "" + Mathf.Round(agWrist[0]);
                valuesWristAngle[1].text = "" + Mathf.Round(agWrist[1]);
                valuesWristAngle[2].text = "" + Mathf.Round(agWrist[2]);
                valuesWristAcc[0].text = "" + accWrist[0].ToString("F2");
                valuesWristAcc[1].text = "" + accWrist[1].ToString("F2");
                valuesWristAcc[2].text = "" + accWrist[2].ToString("F2");

                //values of ring 
                valuesRingAngle[0].text = "" + Mathf.Round(agRing[0]);
                valuesRingAngle[1].text = "" + Mathf.Round(agRing[1]);
                valuesRingAngle[2].text = "" + Mathf.Round(agRing[2]);
                valuesRingAcc[0].text = "" + accRing[0].ToString("F2");
                valuesRingAcc[1].text = "" + accRing[1].ToString("F2");
                valuesRingAcc[2].text = "" + accRing[2].ToString("F2");

                
            /*Debug.Log(" 0 Wert" + timestamp);
			Debug.Log("erster Wert" + grz );
			Debug.Log("zweiter Wert" + grx);
			Debug.Log("dritte  Wert" + gry);*/

        }

        if (message.StartsWith("D.a.txyz:"))
        {
            message = message.Substring(9);
            string[] txyz = message.Split(',');
            long timestamp = long.Parse(txyz[0]);
            float ax = float.Parse(txyz[2]);
            float ay = float.Parse(txyz[3]);
            float az = float.Parse(txyz[1]);

            lastAccAngles.x = -Mathf.Atan2(az, ay) * 180 / Mathf.PI;
            lastAccAngles.y = 0;
            lastAccAngles.z = -90 + Mathf.Atan2(ay, ax) * 180 / Mathf.PI;

        }
        if (UseAccelerometer && UseGyro)
        {
            Vector3 rotxyz = transform.eulerAngles;
            Quaternion a = Quaternion.identity;
            Quaternion b = Quaternion.identity;

            a.eulerAngles = (rotxyz + lastGyroRate * Time.deltaTime);
            b.eulerAngles = lastAccAngles;
            transform.rotation = Quaternion.Lerp(a,b,alpha);
        }
        else if (UseAccelerometer)
        {
            Quaternion q = Quaternion.identity;
            q.eulerAngles = lastAccAngles;
            transform.rotation = q;
        }
        else if (UseGyro)
        {
            Vector3 rotxyz = transform.eulerAngles;
            rotxyz = rotxyz + lastGyroRate * Time.deltaTime;
            transform.eulerAngles = rotxyz;
        }
    }
     
    

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

    public float[] getRotationRing()
    {
        return angleAccRing;
    }

    public float[] getRotationWrist()
    {
        return angleAccWrist;
    }

    public float[] getGyroRing() {
        return agRing;
    }

    public float[] getGyrosWrist() {
        return agWrist;
    }

    void Update () {
        //string message;

        //while (true)
        //{
        //    message = serialcontroller.readserialmessage();
        //    if (message == null)
        //        break;
        //    if (referenceequals(message, serialcontroller.serial_device_connected))
        //        continue;
        //    else if (referenceequals(message, serialcontroller.serial_device_disconnected))
        //        continue;
        //    processmessage(message);
        //}
    }


    // when the calibrate button is pressed, set the values to
    public void Calibrate()
    {
        Debug.Log(txyz);
        Debug.Log(float.Parse(txyz[0]));
        calAgRing[0] = float.Parse(txyz[0]);
        Debug.Log("2");
        calAgRing[1] = float.Parse(txyz[1]);
        Debug.Log("3");
        calAgRing[2] = float.Parse(txyz[2]);
        Debug.Log("4");
        calAccRing[0] = float.Parse(txyz[3]);
        Debug.Log("5");
        calAccRing[1] = float.Parse(txyz[4]);
        Debug.Log("6");
        calAccRing[2] = float.Parse(txyz[5]);

        //angle and acc of wrist
        calAgWrist[0] = float.Parse(txyz[6]);
        calAgWrist[1] = float.Parse(txyz[7]);
        calAgWrist[2] = float.Parse(txyz[8]);
        calAccWrist[0] = float.Parse(txyz[9]);
        calAccWrist[1] = float.Parse(txyz[10]);
        calAccWrist[2] = float.Parse(txyz[11]);
    }
}
