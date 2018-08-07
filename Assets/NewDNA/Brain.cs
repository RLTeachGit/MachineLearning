using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyPhysics;

namespace NewDNA
{
    [RequireComponent(typeof(CharacterPhysics))]
    [RequireComponent(typeof(Collider))]

    public class Brain : MonoBehaviour
    {
        public GameObject Eyes;  
        public  MeshRenderer[] EyeMesh;
        DNA mDNA;

        public  DNA GetDNA {
            get {
                return mDNA;
            }
        }

        private CharacterPhysics mCP;
        private Collider mCollider;
        private MeshRenderer mMR;
        private CharacterController mCC;

        private LayerMask mWalkLayer;

        private float mSpeed;

        private bool mIsAlive;
        private bool mSuspend;

        private bool mSeeGround=true;

        float   mTimeAlive = 0.0f;
        float   mDistanceWalked = 0.0f;
        float   mDistanceFromStart = 0.0f;
        Vector3 mStartPosition;

        private float mViewDistance = 10.0f;

        public  bool    IsAlive {
            get {
                return mIsAlive;
            }
        }

        public  bool    Suspend {
            get {
                return mSuspend;
            }
            set {
                mSuspend = value;
            } 
        }


        // Use this for initialization
        void Awake() {
            mCP = GetComponent<CharacterPhysics>();
            mCollider = GetComponent<Collider>();
            mMR = GetComponent<MeshRenderer>();
            mCC = GetComponent<CharacterController>();
            mWalkLayer = 1 << LayerMask.NameToLayer("WalkOn");
            transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        }

        public void Init() {
            mDNA = new DNA();

            mDNA.AddGene(new EyeColour());
            mDNA.AddGene(new Movement());
            mDNA.AddGene(new Sight());
            mDNA.AddGene(new EyeAngle());

            mDNA.Process(this);

            mIsAlive = true;
            mTimeAlive = 0.0f;
            mDistanceWalked = mDistanceFromStart = 0.0f;
            mStartPosition = transform.position;
        }

        public void Init(DNA vDNA) {
            mDNA = vDNA;

            mDNA.Process(this);

            mIsAlive = true;
            mSuspend = false;

            mTimeAlive = 0.0f;
            mDistanceWalked = mDistanceFromStart = 0.0f;
            mStartPosition = transform.position;
        }



        private void OnTriggerExit(Collider vOther) {
            if (vOther.gameObject.tag == "Death") {
                Die();
            }
        }

        public void Die() {
            mIsAlive = false;
            mCollider.enabled = mMR.enabled = false;    //Stop rendering or colliding
            mCP.enabled = mCC.enabled = false; //Turn off all the scripts other than Brain
            EyeMesh[0].enabled = EyeMesh[1].enabled = false;
        }

        public  void    SetEyeColour(Color vColour) {
            EyeMesh[0].material.color = vColour;
            EyeMesh[1].material.color = vColour;
        }

        public void     SetMovement(float vSpeed) {
            mSpeed = vSpeed;
        }

        public  void    SetViewDistance(float vDistance) {
            mViewDistance = vDistance;
        }

        public  void    SetEyeAngle(float vAngle) {
            Eyes.transform.localRotation = Quaternion.Euler(vAngle, 0, 0);
        }

        // Update is called once per frame
        void Update() {
            if (!IsAlive || mSuspend) return;
            Vector3 tLastPosition = transform.position;
            CheckGround();
            DoMove();
            mTimeAlive = BotManager.TimeAlive;
            mDistanceWalked += (transform.position - tLastPosition).magnitude;
            mDistanceFromStart = (mStartPosition-transform.position).magnitude;
        }

        public  float   Fitness {
            get {
                if (!IsAlive) return 0.0f;
                return mDistanceFromStart;
            }
        }

        void CheckGround()
        {
            RaycastHit tRayHit;
            mSeeGround = false;
            if (Physics.SphereCast(Eyes.transform.position,0.5f,Eyes.transform.forward, out tRayHit, mViewDistance, mWalkLayer))
            {
                Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * mViewDistance, Color.green);
                mSeeGround = true;
            } else {
                Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * mViewDistance, Color.red);
            }
        }

        void    DoMove() {
            if(mSeeGround) mCP.Move(mSpeed);
        }
    }
}
