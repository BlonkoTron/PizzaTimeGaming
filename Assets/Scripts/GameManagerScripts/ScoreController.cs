using System.Runtime.CompilerServices;
using UnityEngine;

public class ScoreController : MonoBehaviour
{

    public static ScoreController Instance;

    public float highScore;

    public float score;

    private float deliveryScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ResetScore();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ResetScore()
    {
        score = 0f;
    }

    public void AddPoints(float points)
    {
        deliveryScore += points;
    }

    public void AddtoTotalScore()
    {   
        int addedScore = Mathf.RoundToInt(deliveryScore);

        score += addedScore;
        deliveryScore = 0;
    }

    public void CheckScore()
    {
        if (score > highScore)
        {
            highScore = score;
        }
    }


}
