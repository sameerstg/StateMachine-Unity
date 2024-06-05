using Oculus.Interaction;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StudentStates
{
    SittingIdle, Talking, Student_Feedback,
    GeneralFeedback, Student_Conflict, Toilet, EmptyIdle,
    BrokenAeroplane, Crafting, LearningIdle, LearningConflict,
    LearningIdleLost, TeacherCrafting
}
public class Student : MonoBehaviour
{
    public StudentType studentType;
    public StateManager stateManager;
    internal Animator animator;
    public bool isPlayingStudent;
    public Image greenImage, unfocusImage, toiletImage;
    public Image brokenAeroplaneUi;
    public Sprite unfocusSpr, angrySpr;
    public Sprite happy, mid, sad;
    //public Button toiletYes, toiletNo;
    public TextMeshProUGUI text;
    public int timesUnfocussed;
    public bool isToiletStudent, isConflictStudent;
    public bool keepInactive;
    public float walkSpeed;
    public float learningYPos;
    public Transform handPos;
    public int timesLearned;
    public void Awake()
    {
        stateManager = new(new() { new Student_SittingIdle(this), new Student_Talking(this), new Student_Feedback(this),
            new Student_GeneralFeedback(this), new Student_Conflict(this), new Student_Toilet(this) ,new Student_EmptyIdle(this)
            ,new Student_BrokenAeroplane(this),new Student_Crafting(this), new Student_LearningIdle(this),new Student_LearningConflict(this),
        new Student_LearningIdleLost(this),new Student_TeacherCrafting(this)});
        animator = GetComponent<Animator>();
        //toiletYes?.onClick.AddListener(() => { keepInactive = true; gameObject.SetActive(false); SampleManager._instance.ChangeGameMode(); });
        //toiletNo?.onClick.AddListener(() => { stateManager.ChangeState(1); SampleManager._instance.ChangeGameMode(); });
        if (!isPlayingStudent) GetComponent<InteractableUnityEventWrapper>().WhenSelect.AddListener(() => { Debug.LogError("Selected"); });
    }
    private void OnEnable()
    {
        studentType = UnityEngine.Random.Range(0, 2) == 0 ? StudentType.Bad : StudentType.Good;
    }
    private void Start()
    {
        happy = Resources.Load<Sprite>("Sprites\\happy");
        mid = Resources.Load<Sprite>("Sprites\\mid");
        sad = Resources.Load<Sprite>("Sprites\\sad");
        SampleManager._instance.onStateChange += _ => AssignStateAcc(_);
        if (text != null) text.enableWordWrapping = false;
        unfocusSpr = unfocusImage?.sprite;
        greenImage?.gameObject.SetActive(false);
        unfocusImage?.gameObject.SetActive(false);
        toiletImage?.gameObject.SetActive(false);
        brokenAeroplaneUi?.gameObject.SetActive(false);
        AssignStateAcc(SampleManager._instance.gameState);
    }
    public void AssignStateAcc(GameState state)
    {
        bool notPlayingStudentActiveCondition = (state == GameState.Story || state == GameState.GeneralFeedbackBad || state == GameState.GeneralFeedbackGood || state == GameState.Conflict || state == GameState.Toilet ||
                    state == GameState.StudentFeedback || state == GameState.Re || state == GameState.End || state == GameState.Crafting || state == GameState.BrokenAeroplane || state == GameState.StudentCrafting
                    || state == GameState.Learning);
        gameObject.SetActive((state == GameState.None || state == GameState.Intro || state == GameState.IntroEnvironment || state == GameState.IntroTask || state == GameState.Gather) && isPlayingStudent ||
            !isPlayingStudent && notPlayingStudentActiveCondition && !keepInactive);
        if (isPlayingStudent) return;
        if (state == GameState.Story)
        {
            stateManager.ChangeState(0);
        }
        else if (state == GameState.StudentFeedback)
        {
            stateManager.ChangeState(2);
        }
        else if (state == GameState.GeneralFeedbackGood || state == GameState.GeneralFeedbackBad)
        {
            stateManager.ChangeState(3);
        }
        else if (state == GameState.Conflict)
        {

            if (isConflictStudent)
                stateManager.ChangeState(4);
            else
                stateManager.ChangeState(6);
        }
        else if (state == GameState.Toilet)
        {

            if (!isToiletStudent)
                stateManager.ChangeState(6);
            else
                stateManager.ChangeState(5);
        }
        else if (state == GameState.BrokenAeroplane)
        {
            if (!isToiletStudent)
                stateManager.ChangeState(6);
            else
            {
                stateManager.ChangeState(7);
            }
        }
        else if (state == GameState.StudentCrafting || state == GameState.Crafting)
        {
            if (state == GameState.Crafting)
                stateManager.ChangeState(12);
            else
                stateManager.ChangeState(8);
        }
        else if (state == GameState.Learning)
        {
        }
    }
    private void Update()
    {
        stateManager?.currentState?.Update();
    }
    public void GoToObject(GameObject go, Student student, Vector3 whereToPut, Vector3 position)
    {
        student.transform.LookAt(go.transform, Vector3.up);
        student.transform.LeanMove(go.transform.position, 3).setEase(LeanTweenType.linear).setOnComplete(() =>
        {
            go.transform.position = student.handPos.position;
            go.transform.SetParent(student.handPos);
            student.transform.LookAt(whereToPut, Vector3.up);
            student.transform.LeanMove(whereToPut, 3).setEase(LeanTweenType.linear).setOnComplete(() =>
            {
                go.transform.parent = null;
                go.transform.position = whereToPut;
                go.transform.eulerAngles = Vector3.up * 180f;
                student.transform.LookAt(position, Vector3.up);
                student.transform.LeanMove(position, 3).setEase(LeanTweenType.linear).setOnComplete(() =>
                {
                    student.stateManager.ChangeState(9);
                });
            });


        });
    }
    //public IEnumerator GoTo(GameObject go, Student student, Vector3 whereToPut, Vector3 position)
    //{

    //}
}

public enum StudentAnimation
{
    Idle_Standing, Idle_Sitting, Talking_Sitting_Chair, Talking_Standing, Walking, Idle_Sitting_Ground, Crafting
}
public enum StudentType
{
    Good, Bad
}
public enum SightType
{
    None, Student, Host
}
public enum StudentPosition
{
    ground, sitting
}
[System.Serializable]
public class Student_BaseState : State
{
    internal Student student;
    public Student_BaseState(object obj) : base(obj)
    {
        student = obj as Student;
    }
    public float timeToLoseFocus;
    public override void Enter()
    {

        timeToLoseFocus = student.studentType == StudentType.Good ? UnityEngine.Random.Range(15, 20) : UnityEngine.Random.Range(11, 15);
        student.unfocusImage.gameObject.SetActive(false);
        student.greenImage.gameObject.SetActive(false);
        student.toiletImage.gameObject.SetActive(false);
        student.brokenAeroplaneUi.gameObject.SetActive(false);
        student.text.text = " ";
        SetHeight();

    }

    internal void PlayEmptyAnimation()
    {
        if (SampleManager._instance.mode == GameState.Crafting)
        {
            student.animator.SetTrigger(StudentAnimation.Idle_Sitting.ToString());
        }
        else if (SampleManager._instance.mode == GameState.Learning)
        {
            student.animator.SetTrigger(StudentAnimation.Idle_Standing.ToString());
        }
        else if (SampleManager._instance.mode == GameState.Story)
        {
            student.animator.SetTrigger(StudentAnimation.Idle_Sitting_Ground.ToString());
        }

    }
    internal void SetHeight()
    {
        student.transform.position = new Vector3(student.transform.position.x, SampleManager._instance.mode == GameState.Learning ? student.learningYPos : SampleManager._instance.mode == GameState.Story ? 0.15f : .5f, student.transform.position.z);
    }
    public override void Update()
    {
    }
    public bool IsFocusLost()
    {
        return student.didStart;
    }
    public void LookAtPlayer()
    {
        student.transform.LookAt(SampleManager._instance.transform);
        var rot = student.transform.eulerAngles;
        rot.x = rot.z = 0;
        student.transform.eulerAngles = rot;


    }
}
public class Student_SittingIdle : Student_BaseState
{
    public Student_SittingIdle(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        student.greenImage.gameObject.SetActive(true);


        student.animator.SetTrigger(StudentAnimation.Idle_Sitting_Ground.ToString());
    }
    public override void Update()
    {
        base.Update();
        if (IsFocusLost())
        {
            student.stateManager.ChangeState(1);
            return;
        }
    }

}
public class Student_Talking : Student_BaseState
{
    public Student_Talking(object obj) : base(obj)
    {

    }
    public override void Enter()
    {
        base.Enter();
        if (SampleManager._instance.mode == GameState.Crafting)
            student.animator.SetTrigger(StudentAnimation.Talking_Sitting_Chair.ToString());
        student.unfocusImage.sprite = student.unfocusSpr;
        student.unfocusImage.gameObject.SetActive(true);
        student.timesUnfocussed++;
    }
    public override void Update()
    {
        base.Update();
    }
}
public class Student_Feedback : Student_BaseState
{
    public Student_Feedback(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //student.text.text = $"Total Sighted Time : {TimeSpan.FromSeconds(student.sightInteractable.totalSightTime).Seconds} sec \nTimes Unfocussed : {student.timesUnfocussed}";
        //student.animator.SetTrigger(StudentAnimation.Idle_Sitting.ToString());
        PlayEmptyAnimation();
        student.unfocusImage.gameObject.SetActive(true);

    }
}
public class Student_GeneralFeedback : Student_BaseState
{
    public Student_GeneralFeedback(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //student.text.text = $"Total Sighted Time : {TimeSpan.FromSeconds(student.sightInteractable.totalSightTime).Seconds} sec \nTimes Unfocussed : {student.timesUnfocussed}";
        //student.unfocusImage.sprite = student.timesUnfocussed <= 1 ? student.happy : student.timesUnfocussed <= 3 ? student.mid : student.sad;
        //student.unfocusImage.gameObject.SetActive(true);
        //student.animator.SetTrigger(StudentAnimation.Idle_Sitting.ToString());
        PlayEmptyAnimation();
    }
}
public class Student_Toilet : Student_BaseState
{
    public Student_Toilet(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        PlayEmptyAnimation();
        student.toiletImage.gameObject.SetActive(true);
    }
    public override void Exit()
    {
        base.Exit();
        student.toiletImage.gameObject.SetActive(false);
        student.isToiletStudent = false;
    }
}
public class Student_BrokenAeroplane : Student_BaseState
{
    public Student_BrokenAeroplane(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        student.animator.SetTrigger(StudentAnimation.Idle_Sitting.ToString());
        student.brokenAeroplaneUi.gameObject.SetActive(true);
    }
    public override void Exit()
    {
        base.Exit();
        student.brokenAeroplaneUi.gameObject.SetActive(false);
    }
}
public class Student_EmptyIdle : Student_BaseState
{
    public Student_EmptyIdle(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        PlayEmptyAnimation();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
public class Student_Conflict : Student_BaseState
{
    public Student_Conflict(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        PlayEmptyAnimation();
        student.unfocusImage.sprite = student.angrySpr;
        student.unfocusImage.gameObject.SetActive(true);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        student.unfocusImage.gameObject.SetActive(false);
        student.isConflictStudent = false;
    }
}
public class Student_Crafting : Student_BaseState
{
    Quaternion rotation;
    Vector3 pos;
    public Student_Crafting(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();

        pos = student.transform.position;
        var posTemp = pos;
        posTemp.y = .33f;
        student.transform.position = posTemp;

        rotation = student.transform.rotation;
        student.animator.SetTrigger(StudentAnimation.Crafting.ToString());
        var close = SampleManager._instance.studentAircraft[0];
        foreach (var item in SampleManager._instance.studentAircraft)
        {
            close = Vector3.Distance(student.transform.position, item.transform.position) < Vector3.Distance(student.transform.position, close.transform.position) ? item : close;
        }
        student.transform.LookAt(close.transform);

        var rot = student.transform.eulerAngles;
        rot.x = rot.z = 0;
        student.transform.eulerAngles = rot;
    }
    public override void Exit()
    {
        base.Exit();
        student.transform.rotation = rotation;
        student.transform.position = pos;
    }
}
public class Student_LearningIdle : Student_BaseState
{
    public Student_LearningIdle(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        student.greenImage.gameObject.SetActive(true);


        PlayEmptyAnimation();
    }
    public override void Update()
    {
        base.Update();

        if (IsFocusLost())
        {
            student.stateManager.ChangeState(11);
            return;
        }
        LookAtPlayer();
    }
}
public class Student_LearningIdleLost : Student_BaseState
{
    public Student_LearningIdleLost(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        PlayEmptyAnimation();
        student.unfocusImage.sprite = student.unfocusSpr;
        student.unfocusImage.gameObject.SetActive(true);
        student.timesUnfocussed++;
    }
    public override void Update()
    {
        base.Update();
    }
}
public class Student_LearningSetting : Student_BaseState
{
    public Vector3 position;
    public Quaternion rotation;
    public GameObject go;
    Vector3 whereToPut;
    Action action;
    public Student_LearningSetting(object obj, GameObject go, Vector3 whereToPut, Action action) : base(obj)
    {
        this.go = go;
        this.whereToPut = whereToPut;
        this.action = action;
    }
    public override void Enter()
    {
        base.Enter();
        position = student.transform.position;
        rotation = student.transform.rotation;
        //student.animator.SetTrigger(StudentAnimation.Idle_Standing.ToString());
        //student.StartCoroutine(GoToObject());
        student.timesLearned++;
        student.animator.SetTrigger(StudentAnimation.Walking.ToString());
        student.GetComponent<CapsuleCollider>().enabled = false;
        student.GoToObject(go, student, whereToPut, position);
    }
    int i = 0;
    public override void Update()
    {
        base.Update();
        SetHeight();

        //SampleManager._instance.storyObjects[0].timeGrabbed += Time.deltaTime;
    }


    public override void Exit()
    {
        student.transform.position = position;
        student.transform.rotation = rotation;
        student.GetComponent<CapsuleCollider>().enabled = true;
        action?.Invoke();

        base.Exit();
    }
}
public class Student_LearningConflict : Student_BaseState
{
    public Student_LearningConflict(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        student.animator.SetTrigger(StudentAnimation.Idle_Standing.ToString());
    }
    public override void Exit()
    {
        base.Exit();
    }
}
public class Student_TeacherCrafting : Student_BaseState
{
    public Student_TeacherCrafting(object obj) : base(obj)
    {
    }
    public override void Enter()
    {
        base.Enter();
        student.greenImage.gameObject.SetActive(true);


        PlayEmptyAnimation();
    }
    public override void Update()
    {
        base.Update();
        if (IsFocusLost())
        {
            student.stateManager.ChangeState(1);
            return;
        }
    }
}
