using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointManager : MonoBehaviour
{

    public static CheckpointManager instance { get; private set; }

    public Vector2 LastCheckpointPosition;

    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
    }
    
    public void SetCheckpoint(Vector2 position)
    {
        LastCheckpointPosition = position;
    }
}
