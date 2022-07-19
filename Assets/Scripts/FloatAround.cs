using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FloatAround : MonoBehaviour
{
    public float minX, maxX, minY, maxY, minZ, maxZ, moveSpeed, floatDistance;

    public float newPosTimer = 10f, minFloatTime, maxFloatTime;
    float posTimerReset, floatTimer;

    [SerializeField] Vector3 randomPosition;

    void Start()
    {
        posTimerReset = newPosTimer;
        floatTimer = Random.Range(minFloatTime, maxFloatTime);
        SetNewPosition();
    }

    void Update()
    {
        newPosTimer -= Time.deltaTime;
        floatTimer -= Time.deltaTime;

        MoveToPosition();

        if (floatTimer <= 0f)
        {
            floatDistance = -floatDistance;
            floatTimer = Random.Range(minFloatTime, maxFloatTime);
        }

        if (Vector3.Distance(transform.position, randomPosition) <= 1f || newPosTimer <= 0f)
        {
            SetNewPosition();
            newPosTimer = posTimerReset;
        }

        //if (transform.position.x < minX || transform.position.x > maxX)
        //{
        //    Debug.Log("Out of bounds on x axis");
        //    randomPosition = Vector3.zero;
        //    newPosTimer = timerReset;
        //} 
        
        //if (transform.position.y < minY || transform.position.y > maxY)
        //{
        //    Debug.Log("Out of bounds on y axis");
        //    randomPosition = ;
        //    newPosTimer = timerReset;
        //}
        
        //if (transform.position.z < minZ || transform.position.z > maxZ)
        //{
        //    Debug.Log("Out of bounds on z axis");
        //    randomPosition = Vector3.zero;

        //}
    }

    private void SetNewPosition()
    {
        randomPosition = new Vector3(Random.Range(minX, maxX), transform.position.y + floatDistance, Random.Range(minZ, maxZ));

    }

    private void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(randomPosition.x, transform.position.y + floatDistance, randomPosition.z), moveSpeed * Time.deltaTime);

        
    }
}
