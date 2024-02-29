using UnityEngine;
using UnityEngine.UI;

public class ConflictUi : MonoBehaviour
{
    public Button quiet, sittingDistant, ask1ToLeave, ask2ToLeave;
    private void Start()
    {
        GameManager._instance.onStateChange += _ => OnStateChanged(_);
        OnStateChanged(GameManager._instance.gameState);
        quiet.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore = 30;
            GameManager._instance.sittingManager.instantiatedStudents.FindAll(x => x.isConflictStudent).ForEach(x => { x.isConflictStudent = false;x.unfocusImage.gameObject.SetActive(false); });
            GameManager._instance.ChangeGameMode();
        });
        ask2ToLeave.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore = 0;

            var stu = GameManager._instance.sittingManager.instantiatedStudents.FindAll(x => x.isConflictStudent);
            stu.ForEach(x => { x.gameObject.SetActive(false); x.keepInactive = true; x.unfocusImage.gameObject.SetActive(false); });
            GameManager._instance.ChangeGameMode();
        });
        ask1ToLeave.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore = 0;

            var stu = GameManager._instance.sittingManager.instantiatedStudents.FindAll(x => x.isConflictStudent);
            stu[0].keepInactive = true;


            stu.ForEach(x => { x.isConflictStudent = false; x.unfocusImage.gameObject.SetActive(false); });
            GameManager._instance.ChangeGameMode();
        });
        sittingDistant.onClick.AddListener(() =>
        {
            GameManager._instance.classManagementScore = 100;

            var stu = GameManager._instance.sittingManager.instantiatedStudents.FindAll(x => x.isConflictStudent);

            stu.ForEach(x => { x.isConflictStudent = false; x.unfocusImage.gameObject.SetActive(false); });
            stu[0].transform.SetPositionAndRotation(GameManager._instance.sittingManager.distantPositions[0].position, GameManager._instance.sittingManager.distantPositions[0].rotation);
            stu[1].transform.SetPositionAndRotation(GameManager._instance.sittingManager.distantPositions[1].position, GameManager._instance.sittingManager.distantPositions[1].rotation);
            GameManager._instance.ChangeGameMode();
        });
    }
    private void OnStateChanged(GameState mode)
    {
        gameObject.SetActive(mode == GameState.Conflict);
    }
}
