using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 destdir;
    [SerializeField] private Transform transform;
    private Vector3 rotation;
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void update()
    {
        rotation = new Vector3(0,rotationSpeed,0);
        Quaternion rot = Quaternion.FromToRotation(transform.forward, destdir);
        
        Debug.Log(rot.x + " " + rot.y + " " + rot.z);
        if (rot.y > 0)
        {
            transform.Rotate(rotation * Time.deltaTime);
        }

        if (rot.y < 0)
        {
            transform.Rotate(-rotation * Time.deltaTime);
        }
    }
}
