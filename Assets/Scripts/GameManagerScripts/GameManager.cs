using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;



    [SerializeField] private GameObject[] pickUpPoints;
    [SerializeField] private GameObject[] dropOffPoints;

    [SerializeField] private GameObject Arrow;

    [SerializeField] private float startTime;
    private float timeLeft;

    [SerializeField] private float timeRatio;

    private GameObject currentDeliverySpot;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        StartGame();
    }

    private void StartGame()
    {

        timeLeft = startTime;
        EnablePickups();
        StartCoroutine(GameTimer());
    }

    public void StartDelivery(float distance, GameObject dSpot)
    {
        currentDeliverySpot = dSpot;

        int addTime = Mathf.RoundToInt(distance * timeRatio);

        DisablePickups();
        AddTime(addTime);
        //StartCoroutine(DeliveryTimer(addTime));

        Arrow.SetActive(true);
        Arrow.GetComponent<ArrowController>().SetDestination(currentDeliverySpot.transform);

        ScoreController.Instance.AddPoints(distance);

    }

    public void EndDelivery()
    {
        ScoreController.Instance.AddtoTotalScore();
        currentDeliverySpot = null;

        EnablePickups();

        Arrow.SetActive(false);
    }


    private void EndGame()
    {
        Debug.Log("ENDED!");
    }

    private void EnablePickups()
    {
        foreach (var p in pickUpPoints)
        {
            p.gameObject.SetActive(true);
        }
    }

    private void DisablePickups()
    {
        foreach (var p in pickUpPoints)
        {
            p.gameObject.SetActive(false);
        }
    }

    public GameObject GetDeliveryPoint()
    {
        int deliverySpot = Random.Range(0, dropOffPoints.Length);

        return dropOffPoints[deliverySpot];
       
    }

    public void AddTime(float addedTime)
    {
        Debug.Log("Added Time = " + addedTime);
        timeLeft += addedTime;
    }



    IEnumerator GameTimer()
    {
        while (timeLeft > 0)
        {
            Debug.Log("Timer: " + timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        EndGame();
        
    }
    
    //IEnumerator DeliveryTimer(int addedTime)
    //{

    //}



}
