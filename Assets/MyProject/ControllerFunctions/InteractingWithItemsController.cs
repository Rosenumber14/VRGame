using System.Collections.Generic;
using UnityEngine;

namespace Assets.MyProject
{
    //test class that isnt really used
    public class InteractingWithItemsController : MonoBehaviour
    {
        private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        private SteamVR_TrackedObject trackedObj;
        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

        HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();
        private InteractableItem interactingItem;

        void Start()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        void Update()
        {
            if(controller == null)
            {
                Debug.Log("Controller not initialized");
                return;
            }

            if (controller.GetPressDown(gripButton))
            {
                interactingItem = GetClosestItem();

                if(interactingItem)
                {
                    //if object is allready interacting with something else then drop it 
                    if(interactingItem.IsInteracting())
                        interactingItem.EndInteraction(this);

                    interactingItem.BeginInteraction(this);
                }
            }
            else if (controller.GetPressUp(gripButton) && interactingItem != null)
            {
                interactingItem.EndInteraction(this);
            }
        }

        //unity function
        private void OnTriggerEnter(Collider collider)
        {
            InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
            if (collidedItem)
                objectsHoveringOver.Add(collidedItem);
        }
        //unity function
        private void OnTriggerExit(Collider collider)
        {
            InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
            if (collidedItem)
                objectsHoveringOver.Remove(collidedItem);
        }

        private InteractableItem GetClosestItem()
        {
            float minDistance = float.MaxValue;
            float distance;
            InteractableItem closestItem = null;
            foreach (InteractableItem item in objectsHoveringOver)
            {
                distance = (item.transform.position - transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            return closestItem;
        }
    }
}
