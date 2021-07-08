using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class OctoLocomotion : MonoBehaviour
{
    public float wanderRadius; 
    public float wanderTimer; 

    private Transform target; 
    public NavMeshAgent agent; 
    private float timer; 

    private OctoGenetics octoGenetics; 
    private OctoBehavior octoBehavior;

    [HideInInspector]
    public GameObject[] nearbyFood;
    public GameObject foodTravellingTo; 
    public GameObject[] nearbyBreedMatches; 
    public GameObject breedMatchTravellingTo; 



    private void OnEnable() 
    {
        agent = GetComponent<NavMeshAgent>(); 
        timer = wanderTimer; 
        agent.updateRotation = false;
        wanderRadius = 50f; 
        wanderTimer = 5f;
        octoGenetics = GetComponent<OctoGenetics>();  
        octoBehavior = GetComponent<OctoBehavior>();

    }

    private void Update() 
    {
        Locomotion(); 
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist; 
        randDirection += origin; 
        NavMeshHit navHit; 
        NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask); 
        return navHit.position;
    }

    private void Locomotion() 
    {    
        if(!octoBehavior.isEating && !octoGenetics.isBreeding)
        {
            timer += Time.deltaTime; 
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);

            if(timer >= wanderTimer)
            {
                if((nearbyFood == null || !octoGenetics.hungry) && !octoGenetics.breedSearch)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                    wanderTimer = Random.Range(0, 11f);
                    wanderRadius = Random.Range(25f,50f);
                }
                if(octoGenetics.hungry && !octoBehavior.isEating)
                {
                    nearbyFood = GameObject.FindGameObjectsWithTag("food");
                    if(nearbyFood != null)
                    {
                        octoBehavior.CalculateClosestFood();
                        
                        if(foodTravellingTo != null)
                            agent.SetDestination(foodTravellingTo.transform.position);
                        else
                            return; 
                        
                        timer = 0; 
                        wanderTimer = 0; 
                        wanderRadius = 0;

                        if(agent.remainingDistance <= 0.3f && foodTravellingTo != null)
                        {
                            octoBehavior.isEating = true;
                            agent.isStopped = true; 
                            octoBehavior.Eat();  
                        }
                    }
                    else
                        return; 
                }

                if(octoGenetics.breedSearch)
                {
                    nearbyBreedMatches = GameObject.FindGameObjectsWithTag("female");
                    if(nearbyBreedMatches != null)
                    {
                        octoBehavior.CaclulateClosestBreedMatch(); 

                        if(breedMatchTravellingTo != null)
                            agent.SetDestination(breedMatchTravellingTo.transform.position); 
                        
           
                        timer = 0; 
                        wanderTimer = 0;
                        wanderRadius = 0; 

                        if(agent.remainingDistance <= 1.3f && !octoGenetics.isBreeding && breedMatchTravellingTo.GetComponent<OctoGenetics>().isBreeding == false)
                        {
                            octoBehavior.Breeding(); 
                        }
                    }
                    
                }
            }
        }
    }
    
}
