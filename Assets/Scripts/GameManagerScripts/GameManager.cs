using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [Header("POI")]
    [SerializeField] private GameObject[] pickUpPoints;
    [SerializeField] private GameObject[] dropOffPoints;

    [Header("Refrences")]
    [SerializeField] private GameObject PlayerCar;
    [SerializeField] private GameObject Arrow;

    [SerializeField] private float startTime;
    public float timeLeft;

    [SerializeField] private float timeRatio;

    private GameObject currentDeliverySpot;

    private void Awake()
    {
        instance = this;
        timeLeft = startTime;
        PlayerCar.GetComponent<CarInputHandler>().enabled = false;
        StartCoroutine(CountDown());
    }



    private void StartGame()
    {
        PlayerCar.GetComponent<CarInputHandler>().enabled = true;
        UIController.instance.EnableUI();
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

    IEnumerator CountDown()
    {
        int countdown = 4;
        while (countdown > 0)
        {
            if (countdown > 1)
            {
                int countdownToString = countdown - 1;
                UIController.instance.CountDownText(countdownToString.ToString());
                yield return new WaitForSeconds(1);
                countdown--;
            }
            else
            {
                UIController.instance.CountDownText("GO!");
                yield return new WaitForSeconds(1);
                countdown--;
            }

        }

        StartGame();

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
