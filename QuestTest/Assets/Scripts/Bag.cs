using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public List<string> Discs = new List<string>();

    public Vector3 startPosition;

    public Vector3 startOrientation;

    public float seperation;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        for (int i = 0; i < Discs.Count; i++)
        {
            GameObject disc = Resources.Load<GameObject>(Discs[i]);
            disc.transform.parent = transform;
            disc.transform.localPosition = startPosition + Vector3.forward * seperation * i;
            disc.transform.rotation = Quaternion.Euler(startOrientation);
        }
    }

    public void AddMenuDisc(GameObject disc)
    {
        if (!Discs.Contains(disc.gameObject.name))
        {
            Discs.Add(disc.gameObject.name);
            disc.transform.parent = transform;
            disc.transform.localPosition = startPosition + Vector3.forward * seperation * (Discs.Count - 1);
            disc.transform.rotation = Quaternion.Euler(startOrientation);
        }
    }
}