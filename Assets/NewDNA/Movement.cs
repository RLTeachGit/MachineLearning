using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class Movement : Gene
    {
        protected   float mSpeed;

        public override string TypeName
        {
            get
            {
                return "Movement";
            }
        }


        public override Gene Clone() {
            Movement tClone = new Movement(((Movement)this).mSpeed);
            return tClone;
        }


        public Movement(float vSpeed) {
            mSpeed = vSpeed;
        }

        public Movement() {
            mSpeed = RandomSpeed();
        }

        float   RandomSpeed() {
            return Random.Range(1.0f, 10.0f);
        }

        public override void Process(Brain vBrain) {
            vBrain.SetMovement(mSpeed);
        }

        public override Gene Combine(Gene vOther) {
            Debug.Assert(vOther.GetType() == GetType(),"Illegal EyeGene Combine");
            if (Chance(10.0f)) return new Movement();  //Random Mutation
            return Chance(50.0f) ? Clone() : vOther.Clone();
        }
    }
}
