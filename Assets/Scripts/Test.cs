using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<GameObject> testList;

    public GameObject gO1, gO2;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            testList = new List<GameObject>();
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            testList.Clear();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            FillList();
        }


    }

    void FillList()
    {
        if (!testList.Contains(gO1))
        {
            testList.Add(gO1);
        }
        
        if (!testList.Contains(gO2))
        {
            testList.Add(gO2);
        }
    }
}
