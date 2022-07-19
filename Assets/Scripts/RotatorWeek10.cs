using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorWeek10: MonoBehaviour
{
    public float speed = 1f, maxRotation, minRotation, maxTimer, minTimer;
    private float x, y, z, changeRotationTimer;

    // Start is called before the first frame update
    void Start()
    {
        RandomiseRotation();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(x * speed * Time.deltaTime, y * speed * Time.deltaTime, z * speed * Time.deltaTime);

        changeRotationTimer -= Time.deltaTime;

        if (changeRotationTimer <= 0f)
        {
            RandomiseRotation();
        }
    }

    public void RandomiseRotation()
    {
        x = Random.Range(minRotation, maxRotation);
        y = Random.Range(minRotation, maxRotation);
        z = Random.Range(minRotation, maxRotation);

        changeRotationTimer = Random.Range(minTimer, maxTimer);
    }
}
