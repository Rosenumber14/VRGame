using UnityEngine;
using System.Collections;
using Assets.MyProject;

public class InteractableItem : MonoBehaviour {
    public Rigidbody rigidBody;
    private bool currentlyInteracting;
    private InteractingWithItemsController attachedWand;
    private Transform interactionPoint;
    private Vector3 posDelta;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;
    private float rotationFactor = 400f;
    private float velocityFactor = 20000f;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigidBody.mass; //show down velocity if object is larger
        rotationFactor /= rigidBody.mass;
	}
	
	// Update is called once per frame
	void Update () {
	    if(attachedWand && currentlyInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigidBody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if(angle > 180)
            {
                angle -= 360;
            }

            this.rigidBody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void BeginInteraction(InteractingWithItemsController wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);
        currentlyInteracting = true;
    }
    public void EndInteraction(InteractingWithItemsController wand)
    {
        if(wand == attachedWand)
        {
            attachedWand = null;
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting()
    {
        return currentlyInteracting;
    }
}
