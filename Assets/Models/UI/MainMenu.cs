using App;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject[] topCtrls;
    private GameObject[] botCtrls;

    void Awake()
    {
        topCtrls = GameObject.FindGameObjectsWithTag("TopControls");
        botCtrls = GameObject.FindGameObjectsWithTag("BotControls");
    }

    /// <summary>
    ///     Start Multiplayer game
    /// </summary>
    public void StartVsMultiplayer()
    {
        for (int i = 0; i < topCtrls.Length; i++)
        {
            topCtrls[i].gameObject.SetActive(true);
            botCtrls[i].gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
        FindObjectOfType<GameEngine>().StartGame();
    }

    /// <summary>
    ///     Start game vs AI
    /// </summary>
    public void StartVsAI()
    {
        for (int i = 0; i < botCtrls.Length; i++)
        {
            botCtrls[i].gameObject.SetActive(true);
            botCtrls[i].gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
        FindObjectOfType<GameEngine>().StartGame(true);
    }

    /// <summary>
    ///     Hide visible controls
    /// </summary>
    public void HideControls()
    {
        for (int i = 0; i < topCtrls.Length; i++)
        {
            topCtrls[i].gameObject.SetActive(false);
            botCtrls[i].gameObject.SetActive(false);
        }
    }
    
    void OnEnable()
    {
        HideControls();
    }
}
