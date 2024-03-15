using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableState
{
    public Vector3 position;
    public bool isActive;

    public SaveableState(Vector3 position, bool isActive)
    {
        this.position = position;
        this.isActive = isActive;
    }
}
