using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag: MonoBehaviour
{
    public List<int> Discs = new List<int>();
    public List<GameObject> DiscMapping = new List<GameObject>();   
    

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        foreach (int id in Discs) 
        { 
            Debug.Log(id); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}