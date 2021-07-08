using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    public int currentPopulation; 
    public int currentDied; 
    public Transform[] spawnPoints;  

    public GameObject maleOcto;
    public GameObject femaleOcto; 

    void Start() 
    {
        if(currentPopulation == 0)
        {
            Instantiate(maleOcto, spawnPoints[0].position, Quaternion.identity);
            Instantiate(femaleOcto, spawnPoints[1].position, Quaternion.identity);
            Instantiate(maleOcto, spawnPoints[0].position, Quaternion.identity);
            Instantiate(femaleOcto, spawnPoints[1].position, Quaternion.identity);

        }
    }
}
