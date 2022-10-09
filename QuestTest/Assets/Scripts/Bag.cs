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
    private GameObject Player;

    private OVRPlayerController PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);


        SceneManager.sceneLoaded += (arg0, mode) =>
        {
            Player = GameObject.FindWithTag("Player");
            PlayerController = Player.GetComponentInChildren<OVRPlayerController>();

            Vector3 playerPosition = Player.transform.position;
            transform.position = playerPosition + new Vector3(.7f, -.2f, -.25f);

            for(int i = 0; i < Discs.Count; i ++)
            {
               GameObject discObj = Instantiate(Resources.Load<GameObject>(Discs[i]), transform);
               discObj.transform.localPosition = startPosition + Vector3.forward * seperation * (i - 1);
               discObj.transform.rotation = Quaternion.Euler(startOrientation);
            }

            foreach (DiscController disc in transform.GetComponentsInChildren<DiscController>())
            {
                disc.startPosition = disc.transform.localPosition;
                disc.startRotation = disc.transform.localRotation;
                disc.currentBag = this;
            }
        };

        SceneManager.sceneUnloaded += (arg0) =>
        {
            if (arg0.name != "MenuScene")
            {
                Discs.Clear();
            }

            EmptyBag();
        };
    }

    public void DiscLanded(Vector3 discPosition)
    {
        PlayerController.enabled = false;
        Player.transform.position = discPosition + Vector3.up;
        PlayerController.enabled = true;
        //   Player.GetComponentInChildren<OVRPlayerController>().UpdateMovement();
        Vector3 playerPosition = Player.transform.position;
        transform.position = playerPosition + new Vector3(.7f, -.2f, -.25f);
    }

    public void EmptyBag()
    {
        transform.DetachChildren();
    }

    public void AddMenuDisc(GameObject disc)
    {
        if (!Discs.Contains(disc.gameObject.name))
        {
            Discs.Add(disc.gameObject.name);
          //  disc.transform.parent = transform;
            disc.transform.position = startPosition + Vector3.forward * seperation * (Discs.Count - 1);
            disc.transform.rotation = Quaternion.Euler(startOrientation);
            disc.GetComponent<DiscController>().currentBag = this;

            disc.GetComponent<DiscController>().startPosition = disc.transform.position;
        }
    }
}