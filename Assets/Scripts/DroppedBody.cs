using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedBody : MonoBehaviour
{
    public GameObject[] bodyParts;

    public void BreakBody()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;

        for (int i = 0; i < bodyParts.Length; i++)
        {
            HingeJoint2D hinge = bodyParts[i].GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                bodyParts[i].GetComponent<HingeJoint2D>().enabled = false;
            }
            CapsuleCollider2D capsule = bodyParts[i].GetComponent<CapsuleCollider2D>();

            if (capsule != null)
            {
                capsule.enabled = true;
            }
            StartCoroutine(DestroyBrokenBody(bodyParts[i].gameObject));
        }
    }

    IEnumerator DestroyBrokenBody(GameObject gameObject)
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
