using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour
{
    public List<Vector3> BasketPositions = new List<Vector3>();

    public int currentBasketIndex;

    public GameObject Basket;

    public delegate void BasketScoreEventHandler(object sender, BasketScoreEventArgs e);

    public delegate void DiscThrowEventHandler(object sender, DiscThrowEventArgs e);

    public event DiscThrowEventHandler OnThrowEvent;

    public event BasketScoreEventHandler OnBasketScore;

    // Start is called before the first frame update
    void Start()
    {
        Basket.transform.position = BasketPositions[currentBasketIndex];
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnScore()
    {
        OnBasketScore?.Invoke(this, new BasketScoreEventArgs(0));

        currentBasketIndex++;
        if (currentBasketIndex < BasketPositions.Count)
        {
            Basket.transform.position = BasketPositions[currentBasketIndex];
        }
        else
        {
            //stubbed logical flow
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void OnThrow(DiscController discController)
    {
        OnThrowEvent?.Invoke(this, new DiscThrowEventArgs(discController));
    }

    public void OnMiss()
    {
    }
}