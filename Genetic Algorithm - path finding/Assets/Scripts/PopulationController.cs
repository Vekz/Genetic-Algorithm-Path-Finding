using OxyPlot.Series;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller of the whole creature population. Takes care of next generation generation.
/// Implements elitism and population control in sense of cutting out part of the population.
/// </summary>
public class PopulationController : MonoBehaviour
{

    #region Fields
    #region Characteristics of the population
    List<Genompathfinder> population = new List<Genompathfinder>(); //List of creatures belonging to population
    public GameObject creaturePrefab; //Unity prefab of the creature
    public int populationSize = 100; //Size of the population
    public int genomLength = 50; //Length of the genome 
    public float cutoff = 0.3f; //Percentage of population to keep for the next generation
    [Range(0f,1f)]
    public float mutationRate; //Percentage of the rate
    public int survivorKeep = 5; //Number of the best creatures to keep without change for the next generation
    public Transform SpawnPoint; //Spawn point of the creatures
    public Transform End; //End target for every creature
    bool hasStartedSim;
    #endregion

    #region Plotting variables
    List<List<float>> heritage = new List<List<float>>();
    private int generationNumber = 0;
    #endregion

    #region Screenshot variables
    public bool EnableScreenShots = false; //Enable screenshots of the simulation
    public string screenShotFilepath = @"Screenshot"; //Filepath for the screenshots to be stored in
    private int genNumber = 0; //Number of the current generation
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Method updated every frame that iterates the generations and makes screenshots for every generation
    /// </summary>
    private void Update()
    {
        if(!hasActive() && hasStartedSim) //If current generation doesn't have active creatures
        {
            if(EnableScreenShots) //If screenshots enabled
                StartCoroutine(WaitAndScreen()); //Wait for the screenshot
            NextGeneration(); //Iterate the generation
            genNumber++; //Iterate the generation number
        }
    }

    /// <summary>
    /// Initialize population
    /// </summary>
    public void InitPopulation()
    {
        for(int i = 0; i < populationSize;i++) //For size of the population
        {
            GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity); //Create Unity Object with creaturePrefab @ spawnPoint position and with default rotation

            GO.GetComponent<Genompathfinder>().initCreature(new DNA(genomLength), End.position); //Initialize creature, its genome and set its target

            population.Add(GO.GetComponent<Genompathfinder>()); //Add creature to population list
        }
        hasStartedSim = true;
    }

    /// <summary>
    /// Iterate the generation
    /// </summary>
    void NextGeneration()
    {
        List<Genompathfinder> survivors = new List<Genompathfinder>(); //List of surviving creatures

        ScribeHeritage();

        int survivalCut = Mathf.RoundToInt(populationSize*cutoff); //Number of creatures to keep in population

        for(int i = 0; i < survivalCut; i ++) //For number of creature to keep
        {
            survivors.Add(getFittest()); //Get creature with greatest fittnes and append it to survivors list
        }

        for(int i = 0; i < population.Count; i ++ ) //For every creature in population
        {
            Destroy(population[i].gameObject); //Destroy Unity Object for creature
        }

        population.Clear(); //Clear the population list

        for(int i = 0; i < survivorKeep; i ++ ) //For number of creature kept without change in genome
        {
            GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity); //Create Unity Object with creaturePrefab @ spawnPoint position and with default rotation
            GO.GetComponent<Genompathfinder>().initCreature(survivors[i].dna, End.position); //Initialize creature with unchanged genome of best creatures in the survivors list and set its target
            population.Add(GO.GetComponent<Genompathfinder>()); //Add creature to population list
        }

        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count; i++)
            {
                GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity); //Create Unity Object with creaturePrefab @ spawnPoint position and with default rotation

                GO.GetComponent<Genompathfinder>().initCreature(new DNA(survivors[i].dna, survivors[Random.Range(0,10)].dna, mutationRate),End.position); //Initialize creature with genes shuffled between parents from survivors list

                population.Add(GO.GetComponent<Genompathfinder>()); //Add creature to population list

                if (population.Count >= populationSize) //If number of creatures in population greater or equal than populationSize
                {
                    break; //Break to not create too much creatures
                }
            }
        }

        for(int i = 0; i < survivors.Count; i ++) //For every creature in surviors list
        {
            Destroy(survivors[i].gameObject); //Destroy Unity Object for creature
        }

        generationNumber++;
    }

    /// <summary>
    /// Get fittest creature in population
    /// </summary>
    /// <returns></returns> Fittest creature in population
    Genompathfinder getFittest()
    {
        float maxFitness = float.MinValue; //Initializing max value as minimal value of float

        int index = 0; //Temporary variable for keeping fittest creature index

        for(int i=0; i < population.Count;i++) //For every creature in population
        {
            if(population[i].fitness > maxFitness) //If creature fitness is greater than maxFitness found
            {
                maxFitness = population[i].fitness; //Set maxFitness as its fitness
                index = i; //Keep its index in temporary variable
            }
        }

        Genompathfinder fittest = population[index]; //Get fittest creature from found index

        population.Remove(fittest); //Remove it from population

        return fittest; //Return fittest creature
    }

    /// <summary>
    /// Does population has active creature?
    /// </summary>
    /// <returns></returns> true: if has active creature(s) / false: if doesn't have active creature(s)
    bool hasActive()
    {
        for(int i = 0; i < population.Count; i++) //For every creature in population
        {
            if(!population[i].hasFinished) //If creature didn't finish
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator  WaitAndScreen()
    {
        yield return new WaitForSeconds(1);

        ScreenCapture.CaptureScreenshot(@screenShotFilepath + genNumber.ToString() + ".png");
    }

    private void ScribeHeritage()
    {
        heritage.Add(new List<float>());
        foreach(Genompathfinder creature in population)
        {
            heritage[generationNumber].Add(creature.fitness);
        }
    }
    
    public void makePlots()
    {
        PlottingClass plots = new PlottingClass();

        plots.AverageFitnessPerGeneration(heritage);
    }
    #endregion
}
