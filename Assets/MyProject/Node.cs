using System.IO;
using UnityEngine;

namespace Assets.MyProject
{
    public class Node : MonoBehaviour
    {
        public Node PrevNode;
        private Node nextNode;

        public delegate void Create(Vector3 position, bool isLoading = false);

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
            nextNode = CreateNode(localPosition, transform.parent, this, isLoading);
            return nextNode;
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
                var line = sr.ReadLine();
                while (line != null)
                {
                    create(StringToVector3(line), true);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            else {
                Debug.Log("Could not Open the file: " + fileName + " for reading.");
                return;
            }
        }
        public void SaveNodes(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var sr = File.CreateText(fileName);
            Vector3? prevPosition = transform.localPosition;
            var prevNode = PrevNode;
            while (prevPosition.HasValue)
            {
                sr.WriteLine(prevPosition.Value.ToString("F3"));
                prevPosition = prevNode != null ? prevNode.transform.localPosition : (Vector3?)null;
                prevNode = prevNode != null ? prevNode.PrevNode : null;
            }
            sr.Close();
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
