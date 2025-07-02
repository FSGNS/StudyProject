using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    public GameObject PausePanel;

    public void PauseButtonPressed()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueButtonPresed()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

}