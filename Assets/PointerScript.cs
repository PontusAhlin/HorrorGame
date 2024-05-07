using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PointerScript : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform pointerTransform;
    public GameObject pointer;
    public GameObject pointerWrapper;
    public GameObject target;
    void Awake()
    {

    }    
    private void Update() {
        //Vector3 toPosition = targetPosition;
        //Vector3 fromPosition = gameObject.transform.position;
        //fromPosition.y = 0f;
        //Vector3 direction = (toPosition - fromPosition).normalized;
        Vector3 targetDir = target.transform.position - pointerWrapper.transform.position;
        Vector3 currentDir = pointer.transform.position - pointerWrapper.transform.position;
        float angle = Vector3.Angle(currentDir, targetDir);
        Debug.Log(angle);
        Rotate(angle,currentDir,targetDir);
    }

        private void Rotate(float rotationSpeed, Vector3 forward, Vector3 destdir){
        // rotation is the vector about which we rotate
        Vector3 rotationVector = new Vector3(0,rotationSpeed,0);
        // Quaternion is a measure of how much rotation there is between two vectors
        Quaternion rot = Quaternion.FromToRotation(forward, destdir);
        // here we pick the closest rotation (whether to turn left or right)
        if (rot.y > 0)
        {
            pointerWrapper.transform.Rotate(rotationVector * Time.deltaTime);
        }
        if (rot.y <= 0)
        {
            pointerWrapper.transform.Rotate(-rotationVector * Time.deltaTime);
        }
    }
}
