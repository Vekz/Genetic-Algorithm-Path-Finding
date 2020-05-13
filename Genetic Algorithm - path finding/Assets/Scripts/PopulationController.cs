using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{

    #region Fields
    List<Genompathfinder> population = new List<Genompathfinder>();

    public GameObject creaturePrefab;

    public int populationSize = 100;

    public int genomLength;

    public float cutoff = 0.3f;

    public Transform SpawnPoint;

    public Transform End;
    #endregion

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
        if(!hasActive())
        {
            NextGeneration();
        }
    }

    void InitPopulation()
    {

        for(int i = 0; i < populationSize;i++)
        {
            GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);

            GO.GetComponent<Genompathfinder>().initCreature(new DNA(genomLength = 50 ), End.position);

            population.Add(GO.GetComponent<Genompathfinder>());
        }
    }

    void NextGeneration()
    {
        List<Genompathfinder> survivors = new List<Genompathfinder>();

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


        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count; i++)
            {
                GameObject GO = Instantiate(creaturePrefab, SpawnPoint.position, Quaternion.identity);

                GO.GetComponent<Genompathfinder>().initCreature(new DNA(survivors[i].dna, survivors[Random.Range(0,10)].dna),End.position);

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

}
