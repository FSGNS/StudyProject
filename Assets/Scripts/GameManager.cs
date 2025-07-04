using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayerDied(string deadPlayerTag)
    {
        if (gameEnded) return;
        gameEnded = true;

        string winnerTag = deadPlayerTag == "Player1" ? "Player2" : "Player1";
        Debug.Log(winnerTag + " победил!");

        Invoke("ReloadScene", 2f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}