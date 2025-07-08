using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PlayerMove playerMove;

    public static GameManager Instance;

    private bool gameEnded = false;

    private GameObject redWinFlag; // Флажок победы красного
    private GameObject blueWinFlag; // Флажок победы синего
    private GameObject endScreen; // Экран смерти

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Находим объекты с нужными тегами
        redWinFlag = GameObject.FindGameObjectWithTag("RedWin");
        blueWinFlag = GameObject.FindGameObjectWithTag("BlueWin");
        endScreen = GameObject.FindGameObjectWithTag("EndScreen");

        // Делаем флажки и экран смерти неактивными в начале игры
        if (redWinFlag != null) redWinFlag.SetActive(false);
        if (blueWinFlag != null) blueWinFlag.SetActive(false);
        if (endScreen != null) endScreen.SetActive(false);
    }

    public void PlayerDied(string deadPlayerTag)
    {
        if (gameEnded) return;
        gameEnded = true;

        // Определяем победителя
        string winnerTag = deadPlayerTag == "Player1" ? "Player2" : "Player1";
        Debug.Log(winnerTag + " победил!");

        // Активируем экран смерти
        if (endScreen != null)
        {
            endScreen.SetActive(true);

            // Активируем соответствующий флажок победителя
            if (winnerTag == "Player1" && redWinFlag != null)
            {
                redWinFlag.SetActive(true);
                playerMove.SetSpeed(0f);
            }
            else if (winnerTag == "Player2" && blueWinFlag != null)
            {
                blueWinFlag.SetActive(true);
                playerMove.SetSpeed(0f);
            }
        }
    }
}