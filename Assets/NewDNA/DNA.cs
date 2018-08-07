using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace NewDNA {
    public class DNA {
        
        List<Gene> mGenes;

        public  DNA() {
            mGenes = new List<Gene>();
        }

        public  void AddGene(Gene vGene) {
            mGenes.Add(vGene);
        }

        public void Process(Brain vBrain) {
            foreach(Gene tGene in mGenes) {
                tGene.Process(vBrain);
            }
        }

        static  public DNA Combine(DNA vDNA1, DNA vDNA2) {
            DNA tNewDNA = new DNA();
            foreach(Gene tFirstGene in vDNA1.mGenes) {
                Gene tMatchingGene = FindMatchingGene(tFirstGene, vDNA2);
                if(tMatchingGene!=null) {
                    tNewDNA.AddGene(tFirstGene.Combine(tMatchingGene));
                } else {
                    Debug.LogError("No matching Gene");                    
                }
            }
            return tNewDNA;
        }

        static  public Gene FindMatchingGene(Gene vMyGene,DNA vOtherDNA) {
            Gene    tMatch = vOtherDNA.mGenes.Find(o => o.GetType() == vMyGene.GetType());
            return  tMatch;
        }
    }
}
