using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerProps
{
    public string name;
    public int score;
    public float[] color;
    public int JsonId;
    public PlayerProps()
    {
        color = new float[3];
    }
}
