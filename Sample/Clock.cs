using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject arrow;
    public int seconds;
    private void OnEnable()
    {
        arrow.transform.eulerAngles = Vector3.down * 90;
    }
    private void Start()
    {
        GameManager._instance.onStateChange += state => OnStateChange(state);
    }

    private void OnStateChange(GameState state)
    {
        if (state == GameState.Story || state == GameState.Crafting) StartClock();
        else
        {
            LeanTween.cancel(arrow);
        }
    }

    [ContextMenu("Start")]
    void StartClock()
    {
        arrow.transform.eulerAngles = Vector3.down * 90;
        LeanTween.rotateLocal(arrow, Vector3.down * 90 + Vector3.forward * -90, seconds / 4).setOnComplete(() =>
        {



            LeanTween.rotateLocal(arrow, Vector3.down * 90 + Vector3.forward * -180, seconds / 4).setOnComplete(() =>
            {


                LeanTween.rotateLocal(arrow, Vector3.down * 90 + Vector3.forward * -270, seconds / 4).setOnComplete(() =>
                {

                    LeanTween.rotateLocal(arrow, Vector3.down * 90, seconds / 4).setOnComplete(() =>
                    {
                        if (GameManager._instance.gameState == GameState.Story)
                            GameManager._instance.ChangeGameMode();


                    });


                });

            });


        });
    }
}
