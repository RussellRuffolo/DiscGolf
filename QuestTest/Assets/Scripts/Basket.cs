using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Basket : MonoBehaviour
{
    private Bag Bag;
    // Start is called before the first frame update
    void Start()
    {
        Bag = GameObject.FindWithTag("bag").GetComponent<Bag>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Bag.EmptyBag();
        SceneManager.LoadScene("MenuScene");
    }
}