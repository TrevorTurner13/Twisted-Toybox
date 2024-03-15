using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CheckpointController : MonoBehaviour
{

    private PlayerMovement player;
    private Transform respawnPoint;

    public List<GameObject> SaveableObjects = new List<GameObject>();
    public Dictionary<GameObject, SaveableState> savedStates = new Dictionary<GameObject, SaveableState>();
    public Dictionary<GameObject, SaveableState> GetSavedStates()
    {
        return savedStates;
    }


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        respawnPoint = transform.Find("Respawn Point");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Triggered");
           
            CheckpointManager.instance.SetCheckpoint(respawnPoint);

            foreach (GameObject obj in SaveableObjects)
            {
                SaveState(obj);
            }
        }  
    }

    void SaveState (GameObject obj)
    {
        SaveableState state = new SaveableState(obj.transform.position, obj.activeSelf);
        if (savedStates.ContainsKey(obj))
        {
            savedStates[obj] = state;
        }
        else
        {
            savedStates.Add(obj, state);
        }
        foreach(var entry in savedStates)
{
            Debug.Log($"Object: {entry.Key.name}, Position: {entry.Value.position}, IsActive: {entry.Value.isActive}");
        }
    }
}
