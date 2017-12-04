using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour {

    private List<Pool> pools = new List<Pool>();
    private List<int> next = new List<int>();

    private class Pool
    {
        private List<GameObject> pool = new List<GameObject>();
        private int next = 0;
        private int max = 0;

        public Pool(GameObject prefab, int number, Transform parent)
        {
            for (int i = 0; i < number; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.parent = parent;
                pool.Add(obj);

                //if (obj.GetComponent<EnemyController>())
                //{
                //    obj.GetComponent<EnemyController>().MakeTarget();
                //}
            }
            next = 0;
            max = number;
        }

        public void Destroy()
        {
            foreach (GameObject g in pool)
            {
                g.SetActive(false);
                Object.Destroy(g);
            }
        }

        public GameObject CreateObject()
        {
            int start = next;
            GameObject obj;

            // loop from next until an inactive object is found
            do {
                obj = pool[next];
                next = (next + 1) % max;
                if (!obj.activeInHierarchy) {
                    obj.SetActive(true);
                    return obj;
                }
            } while (next != start);


            // pool is full
            return null;
        }
    }

    [SerializeField]
    private List<GameObject> prefabs;
    [SerializeField]
    private List<int> numbers;
    [SerializeField]
    private List<Transform> parents;

	// Use this for initialization
    //void Start () {
    //    CreatePool();
    //}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(pools[0].CreateObject());
        }
	}

    public void CreatePool()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            pools.Add(new Pool(prefabs[i], numbers[i], parents[i]));
        }
    }

    public GameObject CreateObject(int poolIndex)
    {
        return pools[poolIndex].CreateObject();
    }

    public void DeletePool()
    {

    }
}
