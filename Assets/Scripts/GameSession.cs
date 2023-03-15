using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI LivesText;
    [SerializeField] TextMeshProUGUI ScoreText;
    void Awake() 
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

void Start() 
{
    LivesText.text = playerLives.ToString();
    ScoreText.text = playerScore.ToString();
}
   public void ProcessPlayerDeath()
   {
    if(playerLives > 1)
        {
            TakeLife();
        }
    else
        {
            ResetGameSession();
        }
   }

   public void AddToScore(int PointsToAdd)
   {
        playerScore += PointsToAdd;
        ScoreText.text = playerScore.ToString();
   }

   void ResetGameSession()
   {
        FindObjectOfType<ScenePersist>().resetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
   }

   void TakeLife()
   {
    playerLives --;
    int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(CurrentSceneIndex);
    LivesText.text = playerLives.ToString();
   }
}
