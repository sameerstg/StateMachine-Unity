using UnityEngine;
public class Student : MonoBehaviour
{
    public StateManager stateManager;
    public Animator animator;
    public void Awake()
    {

        stateManager = new(new() { new Student_SittingIdle(this) });
    }
    private void Start()
    {
        stateManager.ChangeState(0);
    }
    private void Update()
    {
        stateManager.currentState.Update();
    }
}
public class Student_SittingIdle : State
{
    Student student;
    public Student_SittingIdle(object obj) : base(obj)
    {
        student = obj as Student;
    }

    public override void Enter()
    {
        student.animator.SetTrigger("SitIdle");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        
    }
}
