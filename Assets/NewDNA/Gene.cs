using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewDNA {


    public abstract class Gene 
    {
        protected   DNA mDNA;

        public virtual string  TypeName {
            get {
                return "Gene";
            }
        }


        public abstract void Process(Brain vBrain);
        public abstract Gene Combine(Gene vOther);
        public abstract Gene Clone();

        public  bool    Chance(float tPercentage) {
            return  Mathf.Min(Random.Range(0.0f, 101.0f),100.0f) <= tPercentage;
        }
    }
}
