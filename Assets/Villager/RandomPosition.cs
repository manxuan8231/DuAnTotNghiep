using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    public Vector2 xRange = new Vector2(-10f, 10f); 
    public Vector2 yRange = new Vector2(-10f, 10f); 
    public Vector2 zRange = new Vector2(-10f, 10f);

    public float updateInterval = 2f; 


    void Start()
    {

        StartCoroutine(UpdatePositionCoroutine());
    }


    IEnumerator UpdatePositionCoroutine()
    {
        while (true) 
        {

            float randomX = Random.Range(xRange.x, xRange.y);
            float randomY = Random.Range(yRange.x, yRange.y);
            float randomZ = Random.Range(zRange.x, zRange.y);

    
            transform.position = new Vector3(randomX, randomY, randomZ);


            yield return new WaitForSeconds(updateInterval);
        }
    }
}
