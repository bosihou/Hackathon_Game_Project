using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject targetObject;
    public float spawnInterval = 5f;
    public bool startActive = true;

    void Start()
    {
        targetObject.SetActive(startActive);
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
