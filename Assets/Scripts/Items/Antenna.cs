using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    *This code simply rotates the antenna objects
    
    *Author(s): Sai Chintapalli
*/
public class Antenna : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    public GameObject Antenna1;
    public GameObject Antenna2;
    void Update()
    {
        Antenna1.transform.Rotate(0,90,0);
        Antenna2.transform.Rotate(0,-90,0);

    }
}
