/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool SpaceKeyPressed;
    private float horizontalInput;
    private float verticalInput;
    float rotationX;
    float rotationY;
    float sensitivity = 15f;
    private float speed = 8f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)){
            SpaceKeyPressed = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //Sai's mouse is flipped???? maybe????
        rotationX += Input.GetAxis("Mouse Y") * sensitivity * -1;
        rotationY += Input.GetAxis("Mouse X") * sensitivity;

    }
    private void FixedUpdate() {
        if(SpaceKeyPressed){
            GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            SpaceKeyPressed = false;
        }
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput * 8, GetComponent<Rigidbody>().velocity.y, verticalInput * 8);
        transform.localEulerAngles = new Vector3(rotationX ,rotationY ,0);
    }
}*/
