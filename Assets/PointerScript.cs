using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/*
    *This code is to handle the arrow that points
    *towards a sensor.

    *Author(s): Sai Chintapalli
*/
public class PointerScript : MonoBehaviour
{
    public GameObject pointer;
    [SerializeField]
    static public GameObject pointerWrapper;
    static public GameObject target;
    [Tooltip("This is how many seconds the sensor will stay active")]
    public int timer = 5;
    bool active = false;

    private void Start(){
        pointerWrapper = GameObject.Find("Pointer Wrapper");
    }

    public void Activate(){
        StartCoroutine(ActiveTimer());
    }

    private void Update() {
        if(target != null){
            Vector3 targetDir = target.transform.position - pointerWrapper.transform.position;
            Vector3 currentDir = pointer.transform.position - pointerWrapper.transform.position;
            float angle = Vector3.Angle(currentDir, targetDir);
            Rotate(angle,currentDir,targetDir);
        }
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

    IEnumerator ActiveTimer(){
        if(!active){
            active = true;
            pointerWrapper.transform.position = pointerWrapper.transform.position + new Vector3(0,5,0);
            yield return new WaitForSeconds(timer);
        pointerWrapper.transform.position = pointerWrapper.transform.position - new Vector3(0,5,0);
        }
    }
}
