using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class OctoGenetics : MonoBehaviour
{
    public float hunger;
    public float maxHunger; 
    public bool hungry;  
    public float speed; 
    public string gender; 
    public int age; 
    public Vector3 size; 
    public int foodAte;  
    public string lifeStage; //baby, adolsecent, adult 

    public bool breedSearch; 
    public bool isBreeding; 

    private OctoLocomotion octoLocomotion;
    private OctoBehavior octoBehavior;

    private float ageTimerReset;
    private float ageTimer;  
    private Population population; 

    public GameObject father = null; 
    public GameObject mother = null; 

    public int generation; 

    private GameSpeed gameSpeed; 

    private void Start() 
    {
        Application.targetFrameRate = -1;
        octoLocomotion = GetComponent<OctoLocomotion>(); 
        octoBehavior = GetComponent<OctoBehavior>(); 
        population = GameObject.Find("GAME MANAGER").GetComponent<Population>();
        gameSpeed = GameObject.Find("Slider").GetComponent<GameSpeed>(); 
        population.currentPopulation++;

        if(father == null & mother == null)
        {
            maxHunger = Random.Range(300,400); 
            hunger = maxHunger;
            speed = Random.Range(0.5f,0.65f); 
            if(gameObject.tag == "male")
                gender = "male";
            else
                gender = "female";
            age = 1; 
            size = new Vector3(Random.Range(0.5f,1f),Random.Range(0.5f,1f),Random.Range(0.5f,1f));
            transform.localScale = size; 

            lifeStage = "baby";
            octoLocomotion.agent.speed = speed; 
            generation = 1;
            gameSpeed.generationTxt.text = "Generation: " + generation;  
        }
        else
        {
            OctoGenetics fatherGen = father.GetComponent<OctoGenetics>(); 
            OctoGenetics motherGen = mother.GetComponent<OctoGenetics>(); 

            maxHunger = Random.Range(fatherGen.maxHunger, motherGen.maxHunger); 
            hunger = maxHunger;
            speed = Random.Range(fatherGen.speed, motherGen.speed); 
            if(gameObject.tag == "male")
                gender = "male";
            else    
                gender = "female";

            age = 1; 
            size = new Vector3(Random.Range(fatherGen.size.x, motherGen.size.x), Random.Range(fatherGen.size.y, motherGen.size.y), Random.Range(fatherGen.size.z, motherGen.size.z)); 
            transform.localScale = size; 

            lifeStage = "baby"; 
            octoLocomotion.agent.speed = speed; 

            int mostRecentGen;
            if(fatherGen.generation > motherGen.generation)
            {
                mostRecentGen = fatherGen.generation; 
            }
            else
            {
                mostRecentGen = motherGen.generation;
            }

            generation = mostRecentGen + 1; 
            if(generation > gameSpeed.highestGen)
                gameSpeed.highestGen = generation; 

            gameSpeed.generationTxt.text = "GENERATION: " + gameSpeed.highestGen; 

            Mutate(); 
        }
    }

    private void Update() 
    {
        Age();   
        Hunger(); 
        Breed(); 
    }

    private void Mutate() 
    {
        int mutateChance = Random.Range(0,11);

        if(Random.Range(0,11) == mutateChance)
        {
            maxHunger = maxHunger + (Random.Range(-100,100));
            speed = (Random.Range(-0.1f, 0.1f));
            size = new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f,0.1f), Random.Range(-0.1f,0.1f));
            octoLocomotion.agent.speed = speed;  
            print("mutated!");
        }
    }

    private void Hunger() 
    {
        hunger -= Time.deltaTime * 3f;

        if(hunger >= (maxHunger / 2))
        {
            hungry = false; 
        }
        else
        {
            hungry = true; 
        }
    }

    private void Age() 
    {
        ageTimerReset += Time.deltaTime;

        if(lifeStage == "baby")
            ageTimer = 60; 
        else
            ageTimer = 120; 
        

        if(ageTimerReset >= ageTimer)
        {
            if(lifeStage == "baby" && ageTimerReset != 0)
            {
                ageTimerReset = 0;
                lifeStage = "adolescent";

            }
            
            if(lifeStage == "adolescent" && ageTimerReset != 0)
            {
                ageTimerReset = 0;
                lifeStage = "adult";
                age++; 
            }

            if(lifeStage == "adult" && ageTimerReset != 0)
            {
                ageTimerReset = 0;
                age++; 
            }
        }
    }

    private void Breed() 
    {
        if(lifeStage == "adult" && !hungry)
        {
            if(gender == "male" && !octoBehavior.FinishedBreeding)
            {
                breedSearch = true; 
            }
        }
    }




}
