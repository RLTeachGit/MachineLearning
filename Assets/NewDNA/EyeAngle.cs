using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class EyeAngle : Gene
    {
        protected   float mAngle;

        public override string TypeName {
            get {
                return "EyeAngle";
            }
        }

        public override Gene Clone() {
            EyeAngle tClone = new EyeAngle(((EyeAngle)this).mAngle);
            return tClone;
        }


        public  EyeAngle(float vEyeAngle) {
            mAngle = vEyeAngle;
        }

        public EyeAngle() {
            mAngle = RandomAngle();
        }

        float   RandomAngle() {
            return Random.Range(15f, 60.0f);
        }

        public override void Process(Brain vBrain) {
            vBrain.SetEyeAngle(mAngle);
        }

        public override Gene Combine(Gene vOther) {
            Debug.Assert(vOther.GetType() == GetType(),"Illegal EyeGene Combine");
            if (Chance(MutationRate)) return new EyeAngle();
            return Chance(50.0f) ? Clone() : vOther.Clone();
        }
    }
}
