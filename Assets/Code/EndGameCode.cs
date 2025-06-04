using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class EndGameCode : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button PlayAgainButton;
    [SerializeField] private TextMeshProUGUI ScoreEndGame;
    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            ReturnToMainMenu();
        });
        PlayAgainButton.onClick.AddListener(() =>
        {
            RetryGame();
        });
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ScoreEndgame()
    {
        //ScoreEndGame.text = Console.WriteLine
    }

 
}