using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum depth
{
    surface,
    medium,
    deep
}


public class DepthController : MonoBehaviour
{
    public depth depthState { get; private set; }


    
}
