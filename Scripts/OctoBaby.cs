using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class OctoBaby : MonoBehaviour
{
    private Population population; 
    
    private int randomGender;
     

    private void Start() 
    {
        population = GameObject.Find("GAME MANAGER").GetComponent<Population>(); 

    }
    
    public void SpawnBaby(Transform mothersTransform, NavMeshAgent mothersAgent, NavMeshAgent fathersAgent,
        OctoGenetics mothersGenet, OctoGenetics fatherGenet, OctoBehavior motherBehav, OctoBehavior fatherBehav) 
    {
        mothersAgent.isStopped = false; 
        fathersAgent.isStopped = false; 
        mothersGenet.isBreeding = false;
        fatherGenet.isBreeding = false; 
        motherBehav.FinishedBreeding = true;
        fatherBehav.FinishedBreeding = true;
         
        randomGender = Random.Range(0,2); //0 = male 1 = female 

        GameObject baby; 
        
        if(randomGender == 0)
            baby = Instantiate(population.maleOcto, mothersTransform.position, Quaternion.identity);
        else
            baby = Instantiate(population.femaleOcto, mothersTransform.position, Quaternion.identity);
        
        print("baby spawned!");
        OctoGenetics babyGenet = baby.GetComponent<OctoGenetics>(); 
        babyGenet.father = fathersAgent.gameObject;
        babyGenet.mother = mothersAgent.gameObject;





        



    }
}
