using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace In3D
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterPhysics : MonoBehaviour
    {

        CharacterController mCC;
        float JumpHeight = 5.0f;

        private float mTurnAngle = 0.0f;
        private bool mJump = false;
        private float mSpeed = 0.0f;

        Vector3 mMoveDirection = Vector3.zero;


        // Use this for initialization
        void Start()
        {
            mCC = GetComponent<CharacterController>();
        }

        public void Move(float vSpeed)
        {
            mSpeed += vSpeed;
        }

        public void Jump()
        {
            mJump |= true;
        }

        public void Turn(float vDirection)
        {
            mTurnAngle += vDirection;
        }

        // Update is called once per frame
        void Update()
        {
            if (mCC.isGrounded)
            {
                mMoveDirection = Vector3.forward * mSpeed;
                mMoveDirection = transform.rotation * mMoveDirection;
                if (mJump)
                {
                    mMoveDirection.y += JumpHeight;
                }
            }
            transform.Rotate(0, mTurnAngle * Time.deltaTime, 0);
            mMoveDirection += Physics.gravity * Time.deltaTime;
            mCC.Move(mMoveDirection * Time.deltaTime);
            mJump = false;
            mTurnAngle = 0.0f;
            mSpeed = 0.0f;
        }
    }

}