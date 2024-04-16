using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class PhysicsControl : MonoBehaviour
{
    void Awake()
    {
        Physics.defaultSolverIterations = 16;
        Physics.defaultSolverVelocityIterations = 16;
    }
}
