using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbelt_OJ
{
    public class ObjectSelection : MonoBehaviour
    {
        public KeyCode selectInput;

        public float reachDistance;

        public Transform eyes, carryPos, highlightedTarget, selectedTarget;

        public List<int> keyLayerIndexList;
        int layerMask;

        UnityEvent selectObjectEvent;
        void Start()
        {
            selectObjectEvent.AddListener(SelectObject);
        }


        void Update()
        {
            foreach (int layerIndex in keyLayerIndexList)
            {
                layerMask = 1 << layerIndex;
            }

            RaycastHit hit;

            if (Physics.Raycast(eyes.position, eyes.TransformDirection(Vector3.forward), out hit, reachDistance, layerMask))
            {
                highlightedTarget = hit.transform;

                if (Input.GetKeyDown(selectInput) || Input.GetMouseButtonDown(0))
                {
                    selectObjectEvent.Invoke();
                }
            }
        }

        void SelectObject()
        {
            if (selectedTarget != null)
            {
                //selectedTarget.GetComponent<Rigidbody>().isKinematic = false;
                selectedTarget.GetComponent<Rigidbody>().useGravity = true;

                selectedTarget.parent = null;

                selectedTarget = null;
            }

            highlightedTarget = selectedTarget;

            //selectedTarget.GetComponent<Rigidbody>().isKinematic = true;
            selectedTarget.GetComponent<Rigidbody>().useGravity = false;
            selectedTarget.parent = carryPos;
            selectedTarget.transform.position = carryPos.position;
        }

    }
}
