using UnityEngine;

public class ScoreController : MonoBehaviour
{

    public ScoreController Instance;

    private float score;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; 
        ResetScore();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void AddPoints(int points)
    {
        score += points;
    }
}
