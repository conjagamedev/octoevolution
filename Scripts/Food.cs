using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject octoFood;

    public GameObject[] spawnPoints;

    private float timer; 
    private float foodTimer;

    public int foodCounter; 

    private void Start() 
    {
        foodTimer = Random.Range(5f,15f);
    }
    private void FixedUpdate() 
    {
        timer += Time.deltaTime; 

        if(timer >= foodTimer)
        {
            if(foodCounter <= 15)
            {
                Transform spawner = spawnPoints[Random.Range(0,spawnPoints.Length)].transform;
                if(spawner.childCount == 0)
                {
                    GameObject newInst = Instantiate(octoFood, spawner.position, Quaternion.identity);
                    newInst.transform.SetParent(spawner);
                    timer = 0;
                    foodCounter++; 
                }
                else
                {
                    spawner = spawnPoints[Random.Range(0,spawnPoints.Length)].transform;
                    timer = 0; 
                    return;
                }
            }
        }
    }
}
