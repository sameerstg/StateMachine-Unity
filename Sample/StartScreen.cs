using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button startButton;
    private void Start()
    {
        startButton.onClick.AddListener(() => { GameManager._instance.ChangeGameMode(); gameObject.SetActive(false); });
        GameManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(GameManager._instance.gameState);
    }

    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.None);
    }
}
