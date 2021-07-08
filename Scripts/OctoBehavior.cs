using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class OctoBehavior : MonoBehaviour
{
    private OctoLocomotion octoLocomotion; 
    private OctoGenetics octoGenetics;
    private Population population; 
    private Food food;  

    public bool isEating;
    public GameObject currentScran; 

    public Transform bestFoodTarget; 
    public Transform bestBreedTarget; 

    private OctoBaby octoBaby; 

    private float breedTimer = 100f; 
    private float breedTimerReset;
    public bool FinishedBreeding;  


    private void Start() 
    {
        octoLocomotion = GetComponent<OctoLocomotion>();
        octoGenetics = GetComponent<OctoGenetics>(); 
        population = GameObject.Find("GAME MANAGER").GetComponent<Population>();
        food = GameObject.Find("GAME MANAGER").GetComponent<Food>(); 
        octoBaby = GameObject.Find("GAME MANAGER").GetComponent<OctoBaby>(); 
    }

    private void Update() 
    {
        if(octoGenetics.hunger <= 0)
        {
            Destroy(gameObject);
            population.currentDied++; 
            population.currentPopulation--;

        }

        if(FinishedBreeding)
        {
            breedTimerReset += Time.deltaTime; 
            if(breedTimerReset >= breedTimer)
            {
                breedTimerReset = 0; 
                FinishedBreeding = false; 
                
            }
        }
    }

    public void Eat() 
    {
        currentScran = octoLocomotion.foodTravellingTo; 
        isEating = false;
        food.foodCounter--;
        octoGenetics.foodAte++; 
        Destroy(currentScran); 
        octoGenetics.hunger = octoGenetics.maxHunger;
        octoLocomotion.agent.isStopped = false;
    }

    public void Breeding() 
    {
        NavMeshAgent breedAgent = bestBreedTarget.GetComponent<NavMeshAgent>(); 
        OctoGenetics targetOG = bestBreedTarget.GetComponent<OctoGenetics>(); 

        if(!targetOG.isBreeding)
        {
            targetOG.isBreeding = true;
            octoGenetics.breedSearch = false; 
            octoLocomotion.agent.isStopped = true;  
            breedAgent.isStopped = true; 
            transform.LookAt(breedAgent.transform);
            breedAgent.transform.LookAt(transform);
            octoGenetics.isBreeding = true; 
            octoBaby.SpawnBaby(breedAgent.transform, breedAgent, octoLocomotion.agent, targetOG, octoGenetics, breedAgent.GetComponent<OctoBehavior>(), this);
        }
        else
            CaclulateClosestBreedMatch(); 
    }

    public void CalculateClosestFood() 
    {
        float closestDistSqr = Mathf.Infinity; 
        Vector3 currentPos = transform.position; 
        foreach(GameObject potentialFood in octoLocomotion.nearbyFood)
        {
            Vector3 directionToTarget = potentialFood.transform.position - currentPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistSqr)
            {
                closestDistSqr = dSqrToTarget;
                bestFoodTarget = potentialFood.transform; 
                octoLocomotion.foodTravellingTo = potentialFood;
            }
        } 
    }

    public void CaclulateClosestBreedMatch()
    {
        float closestDistSqr = Mathf.Infinity;
        Vector3 currentPos = transform.position; 
        foreach(GameObject potentialMatch in octoLocomotion.nearbyBreedMatches)
        {
            if(potentialMatch.GetComponent<OctoGenetics>().isBreeding == false)
            {
                Vector3 directionToTarget = potentialMatch.transform.position - currentPos; 
                float dSqrToTarget = directionToTarget.sqrMagnitude; 
                
                if(dSqrToTarget < closestDistSqr)
                {
                    closestDistSqr = dSqrToTarget; 
                    bestBreedTarget = potentialMatch.transform; 
                    octoLocomotion.breedMatchTravellingTo = bestBreedTarget.gameObject; 
                }
            }
            else
                return;
        }
    }

}
