using UnityEngine;

public class ScoreController : MonoBehaviour
{

    public static ScoreController Instance;

    public float score;

    private float deliveryScore = 0;

    private void Awake()
    {
        
        Instance = this; 
        ResetScore();
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



}
