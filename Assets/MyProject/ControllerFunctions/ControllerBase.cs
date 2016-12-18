using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.MyProject
{
    public class ControllerBase : MonoBehaviour
    {
        private SteamVR_TrackedObject trackedObj;
        private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

        public delegate void OnButtonPress();
        public event OnButtonPress GripPressed;
        public event OnButtonPress MenuPressed;
        public event OnButtonPress TriggerPressed;
        public event OnButtonPress TriggerReleased;

        public delegate void OnCollided(Collider collider);
        public event OnCollided TriggerEnter;

        void Start()
        {
            trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
        }
        
        void Update()
        {
            if (controller == null)
            {
                Debug.Log("Controller not initialized");
                return;
            }
            
            if (controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)) 
                GripPressed();
            if(controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu)) 
                MenuPressed();
            if (controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) 
                TriggerPressed();
            else if (controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) 
                TriggerReleased();
        }

        //unity function
        private void OnTriggerEnter(Collider collider)
        {
            try {
                TriggerEnter(collider);
            }catch(Exception e)
            {

            }
        }
        
    }
}
