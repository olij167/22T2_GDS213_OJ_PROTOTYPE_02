using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbelt_OJ;

public class BoostPad : MonoBehaviour
{

    public float boostForce, boostTimer;
    float boostTimerReset;

    public bool boost;

    MeshRenderer meshRenderer;

    //public List<AudioClip> trampolineAudio;
    //private AudioSource audioSource;

    private void Start()
    {
        boostTimerReset = boostTimer;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        boostTimer -= Time.deltaTime;

        if (boostTimer <= 0f)
        {
            boost = !boost;

            boostTimer = boostTimerReset;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //other.GetComponent<CharacterController>().

        if (boost)
        {
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().AddForce(other.GetComponent<Rigidbody>().velocity * boostForce);
            }

            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().moveSpeed *= boostForce;
            }

            meshRenderer.material.color = Color.green;
        }
        else
        {
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().AddForce(other.GetComponent<Rigidbody>().velocity / boostForce);
            }

            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().moveSpeed /= 2;
            }

            meshRenderer.material.color = Color.red;
        }

        //audioSource.PlayOneShot(trampolineAudio[Random.Range(0, trampolineAudio.Count)]);
    }
}
