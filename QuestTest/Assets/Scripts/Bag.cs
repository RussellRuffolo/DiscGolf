using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        //
        // for (int i = 0; i < Discs.Count; i++)
        // {
        //     GameObject disc = Resources.Load<GameObject>(Discs[i]);
        //     disc.transform.parent = transform;
        //     disc.transform.localPosition = startPosition + Vector3.forward * seperation * i;
        //     disc.transform.rotation = Quaternion.Euler(startOrientation);
        // }

        SceneManager.sceneLoaded += (arg0, mode) =>
        {
            Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
            Debug.Log(playerPosition);
            transform.position = playerPosition + new Vector3(.7f, -.2f, -.25f);

            foreach (DiscController controller in transform.GetComponentsInChildren<DiscController>())
            {
                controller.startPosition = controller.transform.position;
            }
        };
    }

    public void AddMenuDisc(GameObject disc)
    {
        if (!Discs.Contains(disc.gameObject.name))
        {
            Discs.Add(disc.gameObject.name);
            disc.transform.parent = transform;
            disc.transform.localPosition = startPosition + Vector3.forward * seperation * (Discs.Count - 1);
            disc.transform.rotation = Quaternion.Euler(startOrientation);
            disc.GetComponent<DiscController>().currentBag = this;

            disc.GetComponent<DiscController>().startPosition = disc.transform.position;
        }
    }
}