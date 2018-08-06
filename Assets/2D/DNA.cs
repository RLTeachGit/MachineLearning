using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace In2D
{
    public class DNA : MonoBehaviour {
        
        public Text DebugText;
        [HideInInspector]
        public float r;
        [HideInInspector]
        public float g;
        [HideInInspector]
        public float b;

        [HideInInspector]
        public float timeToDie = 0;

        bool mDead = false;

        SpriteRenderer mSR;
        Collider2D mCOL2D;

        public enum Parent {
            Unknown
            ,Parent1
            ,Parent2
            ,Mutation
        };

        public  Parent MyParent=Parent.Unknown;

        private void OnMouseDown()
        {
            mDead = true;
            timeToDie = PopulationManager.timeEllapsed;
            mSR.enabled = false;
            mCOL2D.enabled = false;
            DebugText.enabled = false;
        }

        // Use this for initialization
        void Start () {
            mSR = GetComponent<SpriteRenderer>();
            mCOL2D = GetComponent<Collider2D>();
            mSR.color = new Color(r, g, b, 1);
            SetDebug();
            timeToDie = Mathf.Infinity;
    	}

        void    SetDebug() {
            switch(MyParent) {
                case    Parent.Parent1:
                    DebugText.text = "P1";
                    break;
                case Parent.Parent2:
                    DebugText.text = "P2";
                    break;
                case Parent.Mutation:
                    DebugText.text = "M";
                    break;
                default:
                    DebugText.text = "U";
                    break;
            }        
        }
    }
}
