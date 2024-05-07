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
        Vector3 targetDir = target.transform.position - transform.position;
        Vector3 currentDir = pointer.transform.position - pointerWrapper.transform.position;
        float angle = Vector3.Angle(currentDir, targetDir);
        Debug.Log(angle);
        //if (angle > 5)
            //pointerWrapper.transform.rotation = Quaternion.Slerp(pointerWrapper.transform.rotation, to.rotation, timeCount);
    }
}
