using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewDNA
{
    public class EyeColour : Gene
    {
        protected   Color mColour;

        public override string TypeName {
            get {
                return "EyeColour";
            }
        }

        public override Gene Clone() {
            EyeColour tClone = new EyeColour(((EyeColour)this).mColour);
            return tClone;
        }

        public EyeColour(Color vColour) {
            mColour = vColour;
        }

        public EyeColour() {
            mColour = RandomEyeColour();
        }

        Color   RandomEyeColour() {
            int tBaseColour = Random.Range(0, 3);
            float tHueBrown = 30.0f / 360.0f;
            float tHueGreen = 125.0f / 360.0f;
            float tHueBlue = 239.0f / 360.0f;

            Color tColour;
            switch(tBaseColour) {
                case 0:
                    tColour = Random.ColorHSV(tHueBrown * 0.9f, tHueBrown * 1.1f, 0.8f, 1.0f);
                    break;
                case 1:
                    tColour = Random.ColorHSV(tHueGreen * 0.9f, tHueGreen * 1.1f, 0.8f, 1.0f);
                    break;
                case 2:
                    tColour = Random.ColorHSV(tHueBlue * 0.9f, tHueBlue * 1.1f, 0.8f, 1.0f);
                    break;
                default:
                    tColour = Color.red;
                    break;

            }
            return tColour;
        }

        public override void Process(Brain vBrain) {
            vBrain.SetEyeColour(mColour);
        }

        public override Gene Combine(Gene vOther) {
            Debug.Assert(vOther.GetType() == typeof(EyeColour),"Illegal EyeGene Combine");
            if (Chance(10.0f)) return new EyeColour();  //Random Mutation
            return new EyeColour(Color.Lerp(mColour, ((EyeColour)vOther).mColour, Random.Range(0.0f, 1.0f)));
        }
    }
}
