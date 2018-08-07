using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


namespace NewDNA
{
    public class BotManager : MonoBehaviour
    {

        public GameObject PeepPrefab;

        [Range(1, 1000)]
        public int PopulationSize = 30;


        List<Brain> mBots = new List<Brain>();

        int Generation = 0;

        float mTimeAlive = 0.0f;

        public int TimeLimit = 10;
        GUIStyle mGUIStyle = new GUIStyle();

        private void OnGUI()
        {
            mGUIStyle.fontSize = 10;
            mGUIStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 20), "Generation:" + Generation, mGUIStyle);
            GUI.Label(new Rect(10, 35, 100, 20), string.Format("Alive:{0}", mBots.Where(o => o.IsAlive).Count()), mGUIStyle);
            GUI.Label(new Rect(10, 65, 100, 20), string.Format("Time:{0:f2}", TimeAlive), mGUIStyle);
            int tAliveCount = mBots.Where(o => o.IsAlive).Count();

        }

        public static float TimeAlive {
            get {
                return Singleton.mTimeAlive;
            }
        }

        public static BotManager Singleton;

        // Use this for initialization
        void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(Singleton.gameObject);
                FirstTimeInit();
            }
            else if (Singleton != this)
            {
                Destroy(gameObject);
            }
        }

        Vector3 RandomPosition()
        {
            Vector3 tPosition = transform.position;
            tPosition.x += Random.Range(-3.0f, 3.0f);
            tPosition.z += Random.Range(-3.0f, 3.0f);
            return tPosition;
        }

        private void FirstTimeInit() {
            mTimeAlive = 0.0f;
            mBots.Clear();
            for (int i = 0; i < PopulationSize; i++) {
                mBots.Add(NewBrain());
            }
        }

        Brain NewBrain() {
            GameObject tGO = Instantiate(PeepPrefab, RandomPosition(), Quaternion.identity);
            Brain tBrain = tGO.GetComponent<Brain>();
            tBrain.Init();
            return tBrain;
        }

        Brain NewBrain(DNA vDNA) {
            GameObject tGO = Instantiate(PeepPrefab, RandomPosition(), Quaternion.identity);
            Brain tBrain = tGO.GetComponent<Brain>();
            tBrain.Init(vDNA);
            return tBrain;
        }

        Brain Breed(Brain vParent1, Brain vParent2) {
            DNA tNewDNA = DNA.Combine(vParent1.GetDNA, vParent2.GetDNA);
            Brain tBrain = NewBrain(tNewDNA);
            return tBrain;
        }

        List<Brain> SortFittest() {
            return mBots.OrderByDescending(o => o.Fitness).ToList();
        }

        IEnumerator BreedNewGeneration() {
            List<Brain> tSortedList = SortFittest();
            mBots.Clear();
            for (int i = 0; i < tSortedList.Count; i++)
            {
                if(i < Mathf.Min(tSortedList.Count, PopulationSize) / 2) {
                    Brain tBot1 = Breed(tSortedList[i], tSortedList[i + 1]);
                    Brain tBot2 = Breed(tSortedList[i + 1], tSortedList[i]);
                    tBot1.Suspend = true;
                    tBot2.Suspend = true;
                    mBots.Add(tBot1);
                    mBots.Add(tBot2);
                    tSortedList[i].Suspend = true;
                    tSortedList[i+1].Suspend = true;
                } else {
                    tSortedList[i].Die();
                }
            }
            yield return new WaitForSeconds(2);
            foreach (Brain tBrain in tSortedList) {
                Destroy(tBrain.gameObject);
            }
            foreach(Brain tBrain in mBots) {
                tBrain.Suspend = false;
            }
            yield return new WaitForSeconds(1);
            if (tSortedList.Count < PopulationSize) {      //If population count has changed add more random ones
                int tAddMore = PopulationSize - tSortedList.Count;
                while (tAddMore-- > 0) {
                    mBots.Add(NewBrain());
                }
            }
            Generation++;
            mNoUpdate = false;
        }


        bool mNoUpdate = false;

        // Update is called once per frame
        void Update()
        {
            if (mNoUpdate) return;
            mTimeAlive += Time.deltaTime;
            if (mTimeAlive >= TimeLimit) {
                mNoUpdate = true;
                mTimeAlive = 0.0f;
                StartCoroutine(BreedNewGeneration());
            }
        }
    }
}

