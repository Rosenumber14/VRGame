using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.MyProject
{
    public class Node : MonoBehaviour
    {
        public Node PrevNode;
        private Node nextNode;
        private List<Node> Branches;
        public bool IsBranching = false; //means next node that is created will be a branch node off this node

        public delegate Node Create(Vector3 position, bool isLoading = false);

        public static Node CreateNode(Vector3 localPosition, Transform parent, Node prevNode = null, bool isLoading = false)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            if (!isLoading)
            {
                sphere.transform.localPosition = localPosition;
            }
            sphere.transform.parent = parent;
            if(isLoading)
            {
                sphere.transform.localPosition = localPosition;
            }
            var node = sphere.AddComponent<Node>();
            node.PrevNode = prevNode;
           
            return node;
        }

        //returns the new node
        public Node AddNextNode(Vector3 localPosition, bool isLoading)
        {
            var node = CreateNode(localPosition, transform.parent, this, isLoading);

            if (IsBranching)
            {
                Branches.Add(node);
                IsBranching = false;
            }
            else
                nextNode = node;

            return node;
        }

        public Node GetPrevNodeIfColliderMatchesThisNode(Collider collider)
        {
            return this.gameObject == collider.gameObject? PrevNode : this;
        }

        public void DeleteNode()
        {
            if(PrevNode != null)
                PrevNode.nextNode = nextNode;
            if(nextNode != null)
                nextNode.PrevNode = PrevNode;
            Destroy(gameObject);
        }

        public static void LoadNodes(string fileName, Create create)
        {
            if (File.Exists(fileName))
            {
                var sr = File.OpenText(fileName);
                LoadNodes(sr, create);
                sr.Close();
            }
            else {
                Debug.Log("Could not Open the file: " + fileName + " for reading.");
                return;
            }
        }

        public static void LoadNodes(StreamReader reader, Create create, Node nodeBranchingFrom = null)
        {
            Node lastSpawn = null;
            var line = reader.ReadLine();
            while (line != null && (lastSpawn == null || line != "}")) 
            {
                if (line.StartsWith("(") && line.EndsWith(")"))
                {
                    var pos = StringToVector3(line);
                    lastSpawn = lastSpawn != null ? lastSpawn.AddNextNode(pos, true) : create(pos, true);
                }

                if (line.StartsWith("Branch:"))
                {
                    lastSpawn.IsBranching = true;
                    LoadNodes(reader, create, lastSpawn);
                }
                ///create(StringToVector3(line), true);
                line = reader.ReadLine();
            }
        }
        public void SaveNodes(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var sr = File.CreateText(fileName);
            SaveNodes(sr, true); //by default when we save assume we called 'saveNodes' on the last node that was created means we will be using (prevNode) when saving
            sr.Close();
        }

        public void SaveNodes(StreamWriter writer, bool saveBackwards)
        {
            if (saveBackwards)
            {
                var prevNode = this;
                while (prevNode != null)
                {
                    prevNode.SaveNode(writer);
                    prevNode = prevNode.PrevNode;
                }
                return;
            }

            var nextNode = this;
            while (nextNode != null)
            {
                nextNode.SaveNode(writer);
                nextNode = nextNode.nextNode;
            }
        }

        public void SaveNode(StreamWriter writer)
        {
            writer.WriteLine(transform.localPosition.ToString("F3"));

            if (Branches.Count > 0)
            {
                writer.WriteLine("Branches: {");
                foreach (var branch in Branches)
                {
                    writer.WriteLine("Branch: {");
                    branch.SaveNodes(writer, false); //because a branch is the 'start' of a list, we need to save going forwards (nextnode)
                    writer.WriteLine("}");
                }
                writer.WriteLine("}");
            }
        }

        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }
    }
}
