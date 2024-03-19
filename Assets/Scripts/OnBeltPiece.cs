using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeltPiece : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
            Destroy(gameObject);
    }
}
