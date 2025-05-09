using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //script for storing objects like projectiles, enemies etc

    [System.Serializable]
    public class Pool
    {
        public string poolTag;
        public GameObject prefab;
        public int poolSize;
    }


    public List<Pool> listOfPools;
    public static ObjectPooler instance;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public Queue<GameObject> bulletPool;
    public GameObject objectToPool;
    public int poolSize;


    private void Awake()
    {
        instance = this;
    }
    
    //initialise and populate pools
    private void Start()
    {
       
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
          
            bulletPool.Enqueue(obj);
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in listOfPools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = (GameObject)Instantiate(pool.prefab);
              
                objectQueue.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolTag, objectQueue);

        }


    }

    //return object from pool
    public GameObject ReturnObject(string tag)
    {
        return poolDictionary[tag].Dequeue();
    }

    //add object to pool
    public void AddObject(string tag, GameObject obj)
    {
        poolDictionary[tag].Enqueue(obj); 
    }



}
