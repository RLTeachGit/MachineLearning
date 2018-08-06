using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace In3D
{
    public class DNA  {

        public  enum Gene {
            Forward
            ,TurnLeft
            ,TurnRight
            ,LastGene
        }

        List<Gene> mGenes = new List<Gene>();

        public  DNA(int vLenght) {
            SetAllRandom(vLenght);
        }


        public void SetAllRandom(int vCount) {
            mGenes.Clear();
            while(vCount--!=0) {
                mGenes.Add(RandomGene());
            }
        }

        public void    SetGene(int vPos, Gene vValue) {
            Debug.Assert(vPos < mGenes.Count,"Gene pos out of range");
            mGenes[vPos] = vValue;
        }

        public void    Combine(DNA vDNA1, DNA vDNA2) {
            for (int i = 0; i < mGenes.Count;i++) {
                if((i&1) ==0) {
                    SetGene(i, vDNA1.mGenes[i]);
                } else {
                    SetGene(i, vDNA2.mGenes[i]);
                }
            }
        }
        public void Mutate() {
            SetGene(RandomPosition(), RandomGene());
        }

        int     RandomPosition() {
            return  Random.Range(0, mGenes.Count);
        }

        Gene    RandomGene() {
            return (Gene)Random.Range(0, (int)Gene.LastGene);
        }

        public  Gene GetGene(int vPos) {
            Debug.Assert(vPos < mGenes.Count, "Gene pos out of range");
            return  mGenes[vPos];

        }

    }
}
