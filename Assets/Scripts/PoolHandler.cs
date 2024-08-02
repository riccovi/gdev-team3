using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject objectPrefab; // The prefab to be pooled
    public GameObject objectPrefab; // The prefab to be pooled
    public int poolSize = 4; // Number of objects to pool
    public float spawnInterval = 2.0f; // Time interval between spawns

    private List<GameObject> pool; // The pool of objects
    private Transform poolParent; // The parent transform for pooled objects

    private void Awake()
    {
        
        pool = new List<GameObject>();
        poolParent = transform.Find("pool");

        if (poolParent == null)
        {
            Debug.LogError("No child object named 'pool' found. Please create a child object named 'pool'.");
            return;
        }

        // Initialize the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, poolParent);
            obj.SetActive(false);
            obj.transform.localPosition=Vector3.zero;
            obj.transform.Find("CollitionTrigger").GetComponent<FallingToolHandler>().poolHandler=this;
            pool.Add(obj);
        }
    }

    public void returnPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolParent);
        obj.transform.localPosition=Vector3.zero;
    }

    private void Start()
    {
        StartCoroutine(ActivateObject());
    }

    private IEnumerator ActivateObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            foreach (Transform child in poolParent)
            {
                if (!child.gameObject.activeInHierarchy)
                {                    
                    child.gameObject.transform.SetParent(null);
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
