using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyBreakScript : MonoBehaviour
{

    List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();
    List<Transform> childTransforms = new List<Transform>();
    [SerializeField] private AudioSource sfxSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies.Clear();
        childTransforms.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            
            Rigidbody2D rigidbody = transform.GetChild(i).GetComponent<Rigidbody2D>();

            if (rigidbody != null)
            {
                rigidbodies.Add(rigidbody);
            }

            Transform childTransform = transform.GetChild(i).transform;

            if (childTransform != null)
            {
                childTransforms.Add(childTransform);
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
            if (sfxSource != null)
            {
                sfxSource.Play();
                sfxSource = null;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DestroyBrokenRBs());
        }
        
    }

    IEnumerator DestroyBrokenRBs()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
