using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointManager : MonoBehaviour
{

    public static CheckpointManager instance { get; private set; }

    public Vector2 LastCheckpointPosition;

    private PlayerMovement player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);

        player = FindObjectOfType<PlayerMovement>();
    }
    
    public void SetCheckpoint(Transform position)
    {
        Debug.Log("Checkpoint Set");

        LastCheckpointPosition = position.position;
        
    }
}
