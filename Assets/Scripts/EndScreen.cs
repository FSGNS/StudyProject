using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject PlayerRed; // Ссылка на красного игрока
    public GameObject PlayerBlue; // Ссылка на синего игрока

    private GameObject redWinFlag; // Флажок победы красного
    private GameObject blueWinFlag; // Флажок победы синего
    private GameObject endScreen; // Экран смерти

    void Start()
    {
        // Находим объекты с нужными тегами
        redWinFlag = GameObject.FindGameObjectWithTag("RedWin");
        blueWinFlag = GameObject.FindGameObjectWithTag("BlueWin");
        endScreen = GameObject.FindGameObjectWithTag("EndScreen");
    }

    void Update()
    {
        if (PlayerRed == null) // Если красный игрок мертв
        {
            EndGame("Blue");
        }
        else if (PlayerBlue == null) // Если синий игрок мертв
        {
            EndGame("Red");
        }
    }

    void EndGame(string winner)
    {
        // Активируем соответствующий флажок победителя
        if (winner == "Red" && redWinFlag != null)
        {
            redWinFlag.SetActive(true);
        }
        else if (winner == "Blue" && blueWinFlag != null)
        {
            blueWinFlag.SetActive(true);
        }

        // Активируем экран смерти
        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
    }
}