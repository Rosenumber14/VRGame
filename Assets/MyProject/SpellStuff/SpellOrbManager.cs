using UnityEngine;

namespace Assets.MyProject.SpellStuff
{
    class SpellOrbManager : MonoBehaviour
    {
        public int Level = 1; //player level

        void Start()
        {
            var orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            orb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            orb.transform.parent = transform;
            orb.transform.localPosition = new Vector3(-1, -0.5f, -1);
        }
    }
}
