using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [Header("Rotation Configs")]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;
    private Vector3 StartRotation;
    private Vector3 Forward;
    private Coroutine AnimationCoroutine;

    private void Awake(){
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.forward;
    }

    public void Open(Vector3 UserPosition){
        Debug.Log("do smth");
        if(!isOpen){
            if(AnimationCoroutine != null){
                StopCoroutine(AnimationCoroutine);
            }
            if(isRotatingDoor){
                float dot = Vector3.Dot(Forward, (transform.position - UserPosition).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount){
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(ForwardAmount >= ForwardDirection){
            endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y - RotationAmount, StartRotation.z));
        }
        else{
            endRotation = Quaternion.Euler(new Vector3(StartRotation.x, StartRotation.y + RotationAmount, StartRotation.z));
        }

        isOpen = true;
        float time = 0;
        while (time < 1){
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close(){
        if(isOpen){
            if (AnimationCoroutine != null){
                StopCoroutine(AnimationCoroutine);
            }

            if (isRotatingDoor){
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose(){
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        isOpen = false;
        float time = 0;
        while (time < 1){
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
