using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.MyProject
{
    public class DrawingController : MonoBehaviour
    {
        public bool IsDebugMode = false;
        public bool IsCreating = true;
        public string FileName = "savedNodes";
        public bool IsGripping = false;

        public Spell CurrentCastingSpell;

        private Node lastSpawn;
        private List<GameObject> Parents = new List<GameObject>();

        void Start()
        {
            var controller = GetComponent<ControllerBase>();
            controller.GripPressed += GripPressed;
            controller.MenuPressed += MenuPressed;
            controller.TriggerPressed += TriggerPressed;
            controller.TriggerReleased += TriggerReleased;
            FileName += ".txt";
        }
        
        private void GripPressed()
        {
            IsCreating = IsDebugMode ? !IsCreating : false; //dont let user get into creating mode if we are not debugging
        }

        private void MenuPressed()
        {
            if (IsCreating)
                lastSpawn.SaveNodes(FileName);
            else
            {
                lastSpawn = null;
                Node.LoadNodes(FileName, Create);
            }
        }

        private void TriggerPressed()
        {
            IsGripping = true;
        }

        public void TriggerReleased()
        {
            IsGripping = false;
        }

        //unity function
        void Update()
        {
            if(IsGripping && IsCreating)
                Create(transform.position);
        }

        private void Create(Vector3 position, bool isLoading = false)
        {
            if (lastSpawn == null || Vector3.Distance(position, lastSpawn.transform.position) > 0.1)
            {
                try
                {
                    lastSpawn = lastSpawn != null ? lastSpawn.AddNextNode(position, isLoading) : Node.CreateNode(position, getParentNode(), null, isLoading);
                }
                catch (Exception e)
                {
                    Debug.Log(lastSpawn);
                }
            }
        }

        private Transform getParentNode()
        {
            var parentNode = new GameObject();
            parentNode.transform.position = transform.position;
            parentNode.transform.rotation = transform.rotation;
            Parents.Add(parentNode);
            return parentNode.transform;
        }

        //unity function
        private void OnTriggerEnter(Collider collider)
        {
            if (!IsCreating)
            {
                var nodeToRemove = collider.GetComponent<Node>();
                if(nodeToRemove == null || nodeToRemove.PrevNode != null) //only delete first node in list (they must delete them in order)
                {
                    return;
                }
                if (lastSpawn != null)
                {
                    lastSpawn = lastSpawn.GetPrevNodeIfColliderMatchesThisNode(collider);
                    if(lastSpawn == null)
                    {
                        var spell = (Spell)Instantiate(CurrentCastingSpell, transform.position, transform.rotation, transform);
                        spell.Controller = GetComponent<ControllerBase>();
                    }
                }
                if (nodeToRemove)
                {
                    nodeToRemove.DeleteNode();
                }
            }
            if (collider.tag == "Clear")
            {
                foreach (var parent in Parents)
                {
                    GameObject.Destroy(parent);
                }
            }
        }

    }
}
