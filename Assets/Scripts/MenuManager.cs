using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [Header("SceneIndex")]
    [SerializeField] private int GameIndex = 1;

    [SerializeField] private TMP_Text HighScoreText;

    private void Update()
    {
        HighScoreText.text = "HighScore: " + ScoreController.Instance.highScore;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(GameIndex);
    }

    public void Exit() 
    {
        Application.Quit();
    }





}
