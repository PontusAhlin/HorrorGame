using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ButtonScript : MonoBehaviour
{
    [SerializeField] AudioSource myAudio; //serialize field allows us to click n drag assets
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current; //we are using INPUTSYSTEM, and i've configured it to imitate gamepad buttons for our touchscreen
        if (gamepad == null)
            return; // No gamepad connected.

        if (gamepad.buttonSouth.wasPressedThisFrame) //check if we just pressed buttonSouth, which is our sound button
        {
            myAudio.Play();
        }
    }
}
