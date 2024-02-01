using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHandler : MonoBehaviour
{
    [SerializeField]
    List<RopeScript> ropes = new List<RopeScript>();

    private bool grabbed = false;

    public bool Grabbed {  get { return grabbed; } set {  grabbed = value; } }

    private void Start()
    {
        FindRopes();
    }

    private void FindRopes()
    {
        ropes.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RopeScript rope = transform.GetChild(i).GetComponent<RopeScript>();

            if (rope != null)
            {
                ropes.Add(rope);
            }
        }
    }

    public Transform FindNearestRopePoint(Transform playerPosition)
    {
        Transform bestPoint = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (RopeScript rope in ropes)
        {
            Vector3 directionToTarget = rope.transform.position - playerPosition.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestPoint = rope.transform;
            }
        }
        return bestPoint;
    }
}
