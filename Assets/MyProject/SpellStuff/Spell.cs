using UnityEngine;
using Assets.MyProject;
using Assets.MyProject.Enemies;

public class Spell : HealthTeamAndDamage {
    public ControllerBase Controller;
    public int Speed = 5;
    private Rigidbody _rigidBody;
	// Use this for initialization
	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        if (Controller)
        {
            Controller.TriggerPressed += ControllerTriggerPressed;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ControllerTriggerPressed()
    {
        transform.parent = null;
        _rigidBody.velocity += transform.forward * (Speed + _rigidBody.velocity.magnitude);
        Controller.TriggerPressed -= ControllerTriggerPressed;
    }
}
