using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class Rotation : Gene
    {
        protected   float mSpeed;

        public override string TypeName
        {
            get
            {
                return "Rotation";
            }
        }


        public override Gene Clone() {
            Rotation tClone = new Rotation(((Rotation)this).mSpeed);
            return tClone;
        }


        public Rotation(float vSpeed) {
            mSpeed = vSpeed;
        }

        public Rotation() {
            mSpeed = RandomSpeed();
        }

        float   RandomSpeed() {
            return Random.Range(-360.0f, 360.0f);
        }

        public override void Process(Brain vBrain) {
            vBrain.SetRotation(mSpeed);
        }

        public override Gene Combine(Gene vOther) {
            Debug.Assert(vOther.GetType() == GetType(),"Illegal EyeGene Combine");
            if (Chance(MutationRate)) return new Rotation();  //Random Mutation
            return Chance(50.0f) ? Clone() : vOther.Clone();
        }
    }
}
