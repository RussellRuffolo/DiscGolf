using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayJawn : MonoBehaviour
{
    private bool isInReplayMode;
    private int currentReplayIndex;
    private Rigidbody rigidbody;
    private List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //pick button to enter replay mode
        /*
         if(Input.GetKeyDown(KeyCode.?))
        {
        isInReplayMode = !isInReplayMode;
            
            if (isInReplayMode)
            {
                SetTransform(0);
                rigidbody.isKinematic = true;
            }
            else
            {
                SetTransform(actionReplayRecords.Count - 1);
                rigidbody.isKinematic = false;
            }
        }
         */
    }

    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            actionReplayRecords.Add(new ActionReplayRecord { position = transform.position, rotation = transform.rotation });
        }
        else
        {
            int nextIndex = currentReplayIndex + 1;

            if(nextIndex < actionReplayRecords.Count)
            {
                SetTransform(nextIndex);
            }
        }
    }

    private void SetTransform(int index)
    {
        currentReplayIndex = index;

        ActionReplayRecord actionReplayRecord = actionReplayRecords[index];

        transform.position = actionReplayRecord.position;
        transform.rotation = actionReplayRecord.rotation;

    }
}
