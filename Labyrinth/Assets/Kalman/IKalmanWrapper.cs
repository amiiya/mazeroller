using UnityEngine;
using System.Collections;


namespace Kalman {
	public interface IKalmanWrapper : System.IDisposable
	{
        Vector3 Update(Vector4 current_phi, Vector4 current_gyro);

    }
}
