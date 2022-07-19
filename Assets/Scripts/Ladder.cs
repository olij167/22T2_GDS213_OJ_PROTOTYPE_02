using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbelt_OJ;

public class Ladder : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().moveDirection.y = Input.GetAxisRaw("Vertical") * (other.GetComponent<PlayerController>().moveSpeed / 2);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.GetComponent<PlayerController>().moveDirection.y = Input.GetAxisRaw("Vertical");
    //    }
    //}
}
