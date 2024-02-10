using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyBreakScript : MonoBehaviour
{

    List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Rigidbody2D rigidbody = transform.GetChild(i).GetComponent<Rigidbody2D>();

            if (rigidbody != null)
            {
                rigidbodies.Add(rigidbody);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }
        }
    }

}
