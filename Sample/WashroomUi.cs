using UnityEngine;
using UnityEngine.UI;

public class WashroomUi : MonoBehaviour
{
    public Button allow, quiet, distract, decline;


    private void Start()
    {
        GameManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(GameManager._instance.gameState);
        allow.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore += 100;
            var stud = GameManager._instance.sittingManager.instantiatedStudents.Find(x => x.isToiletStudent);
            stud.toiletImage.gameObject.SetActive(false);
            stud.gameObject.SetActive(false);
            stud.isToiletStudent = false;
            stud.keepInactive = true;

            GameManager._instance.ChangeGameMode();
        });
        quiet.onClick.AddListener(() =>
        {

            var stud = GameManager._instance.sittingManager.instantiatedStudents.Find(x => x.isToiletStudent);
            stud.toiletImage.gameObject.SetActive(false);
            stud.isToiletStudent = false;
            GameManager._instance.ChangeGameMode();
        });
        distract.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore += 30;

            var stud = GameManager._instance.sittingManager.instantiatedStudents.Find(x => x.isToiletStudent);
            stud.toiletImage.gameObject.SetActive(false);
            stud.isToiletStudent = false;
            GameManager._instance.ChangeGameMode();
        });
        decline.onClick.AddListener(() =>
        {

            var stud = GameManager._instance.sittingManager.instantiatedStudents.Find(x => x.isToiletStudent);
            stud.toiletImage.gameObject.SetActive(false);
            stud.isToiletStudent = false;
            GameManager._instance.ChangeGameMode();
        });
    }
    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.Toilet);
    }
}
