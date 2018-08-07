using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class Sight : Gene
    {
        protected   float mDistance;

        public override string TypeName {
            get {
                return "Sight";
            }
        }

        public override Gene Clone() {
            Sight tClone = new Sight(((Sight)this).mDistance);
            return tClone;
        }


        public Sight() {
            mDistance = RandomSight();
        }

        public Sight(float vDistance) {
            mDistance = vDistance;
        }

        float RandomSight() {
            return Random.Range(5.0f, 20.0f);
        }

        public override void Process(Brain vBrain) {
            vBrain.SetViewDistance(mDistance);
        }

        public override Gene Combine(Gene vOther) {
            Debug.Assert(vOther.GetType() == GetType(),"Illegal EyeGene Combine");

            return Chance(50.0f) ? Clone() : vOther.Clone();
        }
    }
}
