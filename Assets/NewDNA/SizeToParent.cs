using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class SizeToParent : MonoBehaviour
    {
        public float Height = 10.0f;
        public float Margin = 1.2f;
        // Use this for initialization
        void Awake() {
            float tMiddle = Height / 2.0f;
            BoxCollider tBox = GetComponent<BoxCollider>();
            transform.position = new Vector3(transform.parent.position.x
                                             ,(tMiddle)-(tMiddle*(Margin-1.0f))
                                             ,transform.parent.position.z);
            Mesh tMesh=transform.parent.gameObject.GetComponent<MeshFilter>().mesh;
            tBox.center = Vector3.zero;
            tBox.size= new Vector3(tMesh.bounds.size.x*Margin,Height,tMesh.bounds.size.z*Margin);
        }
    }
}
