using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{

    public GameObject loch1;

    void OnTriggerEnter(Collider other)
    {
        Destroy(loch1.gameObject);
    }
}
