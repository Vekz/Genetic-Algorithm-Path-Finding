﻿using OxyPlot.Series;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{

    #region Fields
    List<Genompathfinder> population = new List<Genompathfinder>();

    List<List<float>> heritage = new List<List<float>>();

    private int generationNumber = 0;

    public GameObject creaturePrefab;

    public int populationSize = 100;

    public int genomLength = 50;

    public float cutoff = 0.3f;

    public bool EnableScreenShots = false;

    public Transform SpawnPoint;

    public Transform End;

    public int survivorKeep = 5;

    public string screenShotFilepath = @"Screenshot";

    private int genNumber = 0;

    [Range(0f,1f)]
    public float mutationRate;
    #endregion

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
        if(!hasActive())
        {
            if(EnableScreenShots)
                StartCoroutine(WaitAndScreen());
            NextGeneration();
            genNumber++;
        }
    }

    void InitPopulation()
    {

        for(int i = 0; i < populationSize;i++)
        {
            GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);

            GO.GetComponent<Genompathfinder>().initCreature(new DNA(genomLength), End.position);

            population.Add(GO.GetComponent<Genompathfinder>());
        }
    }

    void NextGeneration()
    {
        List<Genompathfinder> survivors = new List<Genompathfinder>();

        ScribeHeritage();

        int survivalCut = Mathf.RoundToInt(populationSize*cutoff);

        for(int i = 0; i < survivalCut; i ++)
        {
            survivors.Add(getFittest());
        }

        for(int i = 0; i < population.Count; i ++ )
        {
            Destroy(population[i].gameObject);
        }

        population.Clear();

        for(int i = 0; i < survivorKeep; i ++ )
        {
            GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);
            GO.GetComponent<Genompathfinder>().initCreature(survivors[i].dna, End.position);
            population.Add(GO.GetComponent<Genompathfinder>());
        }

        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count; i++)
            {
                GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);

                GO.GetComponent<Genompathfinder>().initCreature(new DNA(survivors[i].dna, survivors[Random.Range(0,10)].dna, this.mutationRate),End.position);

                population.Add(GO.GetComponent<Genompathfinder>());

                if(population.Count >= populationSize)
                {
                    break;
                }
            }
        }

        for(int i = 0; i < survivors.Count; i ++)
        {
            Destroy(survivors[i].gameObject);
        }

        generationNumber++;
    }

    Genompathfinder getFittest()
    {
        float maxFitness = float.MinValue;

        int index = 0;

        for(int i=0; i < population.Count;i++)
        {
            if(population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;
            }
        }

        Genompathfinder fittest = population[index];

        population.Remove(fittest);

        return fittest;
    }


    bool hasActive()
    {
        for(int i = 0; i < population.Count; i++)
        {
            if(!population[i].hasFinished)
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

}
