using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace In3D
{
    public class PopulationManager : MonoBehaviour {

        public  GameObject PeepPrefab;

        [Range(1,200)]
        public int PopulationSize = 30;

        List<PeepBrain> mPeeps=new List<PeepBrain>();

        int Generation = 0;

        float mTimeAlive = 0.0f;

        public int TimeLimit = 10;   
        GUIStyle mGUIStyle = new GUIStyle();

        private void OnGUI() {
            mGUIStyle.fontSize = 10;
            mGUIStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 20), "Generation:" + Generation, mGUIStyle);
            GUI.Label(new Rect(10, 35, 100, 20), string.Format("Alive:{0}", mPeeps.Where(o => o.IsAlive).Count()), mGUIStyle);
            GUI.Label(new Rect(10, 65, 100, 20), string.Format("Time:{0:f2}", TimeAlive), mGUIStyle);
            int tAliveCount = mPeeps.Where(o => o.IsAlive).Count();

        }

        public   static float   TimeAlive {
            get {
                return Singleton.mTimeAlive;
            }
        }

        public static PopulationManager Singleton;

    	// Use this for initialization
    	void Awake () {
            if(Singleton==null) {
                Singleton = this;
                DontDestroyOnLoad(Singleton.gameObject);
                Init();
            } else if( Singleton != this) {
                Destroy(gameObject);
            }
    	}

        Vector3 RandomPosition() {
            Vector3 tPosition = transform.position;
            tPosition.x += Random.Range(-3.0f, 3.0f);
            tPosition.z += Random.Range(-3.0f, 3.0f);
            return tPosition;
        }

        private void Init() {
            mTimeAlive = TimeLimit;
            mPeeps.Clear();
            for (int i = 0; i < PopulationSize;i++) {
                PeepBrain tBrain = NewBrain();
                mPeeps.Add(tBrain);
            }
        }

        PeepBrain NewBrain() {
            GameObject tGO = Instantiate(PeepPrefab, RandomPosition(), Quaternion.identity);
            PeepBrain tBrain = tGO.GetComponent<PeepBrain>();
            tBrain.Init();
            return tBrain;
        }

        PeepBrain    Breed(PeepBrain vParent1, PeepBrain vParent2) {
            PeepBrain tBrain = NewBrain();
            if(Random.Range(0,100)==1) {
                tBrain.mDNA.Mutate();
            } else {
                tBrain.mDNA.Combine(vParent1.mDNA, vParent2.mDNA);
            }
            return tBrain;
        }

        void    BreedNewGeneration() {
            List<PeepBrain> tSortedList = mPeeps.OrderByDescending(o => o.TimeAlive).ToList();
            mPeeps.Clear();
            for (int i = 0; i < tSortedList.Count / 2;i++) {
                mPeeps.Add(Breed(tSortedList[i], tSortedList[i + 1]));
                mPeeps.Add(Breed(tSortedList[i+1], tSortedList[i]));
            }
            foreach(PeepBrain tBrain in tSortedList) {
                Destroy(tBrain.gameObject);
            }
            Generation++;
        }

        // Update is called once per frame
        void Update () {
            mTimeAlive += Time.deltaTime;
            if(mTimeAlive>=TimeLimit) {
                mTimeAlive = 0.0f;
                BreedNewGeneration();
            }
    	}
    }
}

