using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace In2D
{

    public class PopulationManager : MonoBehaviour
    {

        public GameObject PersonPrefab;
        public int PopulationSize = 10;

        List<DNA> mPopulation;

        public static float timeEllapsed = 0;

        int Generation = 1;
        public float TimePerGeneration = 10.0f;


        GUIStyle mGUIStyle = new GUIStyle();

        private void OnGUI()
        {
            mGUIStyle.fontSize = 10;
            mGUIStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 20), "Generation:" + Generation, mGUIStyle);
            GUI.Label(new Rect(10, 65, 100, 20), string.Format("Time:{0:f2}", timeEllapsed), mGUIStyle);
        }

        private Vector2 RandomPosition()
        {
            float tHeight = Camera.main.orthographicSize;
            float tWidth = Camera.main.aspect * tHeight;
            return new Vector2(Random.Range(-tWidth, tWidth), Random.Range(-tHeight, tHeight));
        }

        // Use this for initialization
        void Start()
        {
            mPopulation = new List<DNA>();
            for (int i = 0; i < PopulationSize; i++)
            {
                GameObject tGO = Instantiate(PersonPrefab, RandomPosition(), Quaternion.identity);
                DNA tDNA = tGO.GetComponent<DNA>();
                tDNA.r = Random.Range(0.0f, 1.0f);
                tDNA.g = Random.Range(0.0f, 1.0f);
                tDNA.b = Random.Range(0.0f, 1.0f);
                tDNA.MyParent = DNA.Parent.Mutation;
                mPopulation.Add(tDNA);
            }
        }

        // Update is called once per frame
        void Update()
        {
            timeEllapsed += Time.deltaTime;
            if (timeEllapsed > TimePerGeneration)
            {
                BreedNewGeneration();
                timeEllapsed = 0;
            }
        }

        void BreedNewGeneration()
        {
            List<DNA> tSortedList = mPopulation.OrderBy(o => o.timeToDie).ToList();    //Sorted list of current population
            mPopulation.Clear();
            for (int i = 0; i < tSortedList.Count / 2; i++)
            {
                mPopulation.Add(Breed(tSortedList[i], tSortedList[i + 1]));
                mPopulation.Add(Breed(tSortedList[i + 1], tSortedList[i]));
            }

            foreach (DNA tDNA in tSortedList)
            {
                Destroy(tDNA.gameObject);
            }
            Generation++;
        }

        DNA Breed(DNA vParent1, DNA vParent2)
        {      //Pick a random parent
            GameObject tGO = Instantiate(PersonPrefab, RandomPosition(), Quaternion.identity);
            DNA tDNA = tGO.GetComponent<DNA>();
            int tRandom = Random.Range(0, 1001);    //Number between 0-1000
            if (tRandom < 10)
            {
                tDNA.r = Random.Range(0.0f, 1.0f);
                tDNA.g = Random.Range(0.0f, 1.0f);
                tDNA.b = Random.Range(0.0f, 1.0f);
                tDNA.MyParent = DNA.Parent.Mutation;
            }
            else if (tRandom < 495)
            {
                tDNA.r = vParent1.r;
                tDNA.g = vParent1.g;
                tDNA.b = vParent1.b;
                tDNA.MyParent = DNA.Parent.Parent1;
            }
            else
            {
                tDNA.r = vParent2.r;
                tDNA.g = vParent2.g;
                tDNA.b = vParent2.b;
                tDNA.MyParent = DNA.Parent.Parent2;
            }
            return tDNA;
        }
    }
}