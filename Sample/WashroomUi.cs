using UnityEngine;
using UnityEngine.UI;

public class WashroomUi : MonoBehaviour
{
    public Button allow, quiet, distract, decline;


    private void Start()
    {
        SampleManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(SampleManager._instance.gameState);
        allow.onClick.AddListener(() =>
        {
            SampleManager._instance.classManagementScore += 100;

            SampleManager._instance.ChangeGameMode();
        });
        quiet.onClick.AddListener(() =>
        {

            SampleManager._instance.ChangeGameMode();
        });
        distract.onClick.AddListener(() =>
        {
            SampleManager._instance.classManagementScore += 30;

            SampleManager._instance.ChangeGameMode();
        });
        decline.onClick.AddListener(() =>
        {

            SampleManager._instance.ChangeGameMode();
        });
    }
    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.Toilet);
    }
}
