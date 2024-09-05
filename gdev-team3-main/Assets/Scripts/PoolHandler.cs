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

    public AudioSource audiosource;

    public float WaiTimeBeforeStart;

    public Color FallToolColor;

    [Range(0f, 10f)]
    public float gravityModifier=9.8f;

    private void Awake()
    {
        
        pool = new List<GameObject>();

        FallToolColor.a=1;
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
            obj.transform.Find("Graphics").GetComponent<SpriteRenderer>().color=FallToolColor;
            obj.transform.Find("CollitionTrigger").GetComponent<FallingToolHandler>().accelerationGravity=gravityModifier;
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
        yield return new WaitForSeconds(WaiTimeBeforeStart);
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            foreach (Transform child in poolParent)
            {
                if (!child.gameObject.activeInHierarchy)
                {                    
                    child.gameObject.transform.SetParent(null);
                    child.gameObject.SetActive(true);
                    audiosource.Play();
                    break;
                }
            }
        }
    }
}
