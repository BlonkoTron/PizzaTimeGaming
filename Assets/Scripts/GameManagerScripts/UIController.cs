using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private GameObject scoreBack;
    [SerializeField] private GameObject timer;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (timerText != null)
        {
            timerText.text = GameManager.instance.timeLeft.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + ScoreController.Instance.score.ToString() ;
        }
        
    }

    public void EnableUI()
    {
        
        scoreBack.SetActive(true);
        timer.SetActive(true);

        countDownText.enabled = false;
    }

    public void CountDownText(string count)
    {
        if (countDownText != null)
        {
            countDownText.text = count.ToString();
        }
        
    }


}
