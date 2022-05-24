using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayRecorder
{
    public Dictionary<int, InputStruct> Inputs = new Dictionary<int, InputStruct>();
    
    private static ReplayRecorder m_Instance;
    
    public static ReplayRecorder Instance => m_Instance ??= new ReplayRecorder();

    public void RecordInput(int tickNumber, InputStruct inputs)
    {
        Inputs.Add(tickNumber, inputs);
    }

    public InputStruct GetInput(int tickNumber)
    {
        return Inputs[tickNumber];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class TestObject
{
    public int num1;
    public int num2;
    
}
