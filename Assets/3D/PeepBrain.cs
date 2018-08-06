using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace In3D
{
    [RequireComponent(typeof(CharacterPhysics))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterPhysics))]

    public class PeepBrain : MonoBehaviour
    {
        public  int DNALenght = 2;

        private CharacterPhysics mCP;
        private Collider mCollider;
        private MeshRenderer mMR;
        private CharacterController mCC;

        public GameObject Eyes;

        [Range(1.0f,5.0f)]
        public float Speed = 5.0f;

        [Range(1.0f, 90.0f)]
        public float Angle = 45.0f;

        bool mIsAlive = false;

        public bool IsAlive {
            get {
                return mIsAlive;
            }
        }

        float mTimeAlive = 0.0f;

        public  float   TimeAlive {
            get {
                return mTimeAlive;
            }
        }

        public float SeeDistance = 3.0f;

        bool mSeeGround = false;

        LayerMask mWalkLayer;

        public  DNA mDNA;

        void Start()
        {
            mCP = GetComponent<CharacterPhysics>();
            mCollider = GetComponent<Collider>();
            mMR = GetComponent<MeshRenderer>();
            mCC = GetComponent<CharacterController>();
            mWalkLayer = 1 << LayerMask.NameToLayer("WalkOn");
        }

        public void Init() {
            mTimeAlive = 0.0f;
            mIsAlive = true;
            mDNA = new DNA(DNALenght);
        }

        private void OnTriggerEnter(Collider vOther)
        {
            if (vOther.gameObject.tag == "Death")
            {
                Die();
            }
        }

        private void Die()
        {
            mIsAlive = false;
            mCollider.enabled = mMR.enabled = false;    //Stop rendering or colliding
            mCP.enabled = mCC.enabled = false; //Turn off all the scripts other than Brain
        }

        // Update is called once per frame
        void Update()
        {
            if (!mIsAlive) return;        //Dont process moves if dead
            CheckGround();
            mTimeAlive = PopulationManager.TimeAlive;
            if(mSeeGround) {
                MoveGene(0);
            } else {
                MoveGene(1);
            }
        }

        void    MoveGene(int vGene) {
            switch(mDNA.GetGene(vGene)) {
                case    DNA.Gene.Forward:
                    mCP.Move(Speed);
                    break;
                case    DNA.Gene.TurnLeft:
                    mCP.Turn(Angle);
                    break;
                case DNA.Gene.TurnRight:
                    mCP.Turn(-Angle);
                    break;
                default:
                    break;
            }
        }

        void CheckGround()
        {
            RaycastHit tRayHit;
            mSeeGround = false;
            if (Physics.Raycast(Eyes.transform.position, Eyes.transform.forward, out tRayHit, SeeDistance))
            {
                Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * SeeDistance, Color.green);
                mSeeGround = true;
            }
            else
            {
                Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * SeeDistance, Color.red);
            }
        }

        void MoveManual()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                mCP.Turn(360.0f);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                mCP.Turn(-360.0f);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mCP.Move(5.0f);
            }
            if (Input.GetKey(KeyCode.Return))
            {
                mCP.Jump();
            }
        }
    }
}
