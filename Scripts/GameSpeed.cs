using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameSpeed : MonoBehaviour
{
    private Slider slider; 
    public Text generationTxt; 
    public int highestGen; 

    public Text sliderTxt; 

    public Text populationTxt; 
    public Text deathTxt;

    private Population population;

    private void Start() 
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>(); 
        population = GameObject.Find("GAME MANAGER").GetComponent<Population>();

    }

    public void ControlSpeed() 
    {
        Time.timeScale = slider.value;
    }

    private void Update() 
    {
        sliderTxt.text = "x" + (int) slider.value; 
        populationTxt.text = "CURRENT POPULATION: " + population.currentPopulation; 
        deathTxt.text = "TOTAL DIED: " + population.currentDied;

    }
}
