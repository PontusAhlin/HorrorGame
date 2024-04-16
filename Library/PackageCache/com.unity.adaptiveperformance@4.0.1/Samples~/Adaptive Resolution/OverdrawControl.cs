using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverdrawControl : MonoBehaviour
{
    public int objectCount = 10;
    public Vector3 origin;
    public Vector3 offset;
    public GameObject prefab;
    public Text overdrawText;

    private int activeCount;
    private GameObject[] instances;

    void Start()
    {
        instances = new GameObject[objectCount];
        for (int i = 0; i < objectCount; ++i)
            instances[i] = Instantiate(prefab, origin + offset * i, Quaternion.identity);
        activeCount = objectCount;
        overdrawText.text = "Overdraw: x" + activeCount;
    }

    public void Increase()
    {
        if (activeCount < objectCount)
        {
            instances[activeCount].SetActive(true);
            activeCount++;
            overdrawText.text = "Overdraw: x" + activeCount;
            Debug.Log("Increase");
        }
    }

    public void Decrease()
    {
        if (activeCount > 0)
        {
            activeCount--;
            instances[activeCount].SetActive(false);
            overdrawText.text = "Overdraw: x" + activeCount;
            Debug.Log("Decrease");
        }
    }
}
