using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements genome for the creature. 
/// It also implements crossover and mutation of genes.
/// </summary>
public class DNA 
{
    public List<Vector2> genes = new List<Vector2>(); //List of genes of creature

    /// <summary>
    /// First constructor for DNA class which creates parent genome with specified lenght.
    /// </summary>
    /// <param name="GenomLength"></param> Lenght of genome for creature in our genetic algorithm.
    public DNA(int GenomLength = 50)
    {
        for(int i = 0; i < GenomLength; i++)
        {
            genes.Add(new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f)));
        }
    }

    /// <summary>
    /// Second constructor for DNA class which makes offspring genome based on DNA of her parents.
    /// Implements crossover and mutation of genes in Genetic Algorithm.
    /// </summary>
    /// <param name="parent"></param> DNA of first parent
    /// <param name="partner"></param> DNA of second parent
    /// <param name="mutationRate"></param> Probability for the mutation of genome to occure
    public DNA(DNA parent,DNA partner, float mutationRate = 0.01f)
    {
        for(int i = 0; i < parent.genes.Count;i++)
        {
            float mutationChance = Random.Range(0.0f, 1.0f); //Random number between 0-1 which determines the probabilty of mutation for this offspring
            if(mutationChance<=mutationRate) //If offspring's mutationChance is lower than probability of mutation then mutation occurs
            {
                genes.Add(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
            }
            else
            {
                int chance = Random.Range(0, 2); 
                if(chance <= 1 ) //Half of the genes will come from one of the parents and second half will come from second parent
                {
                    genes.Add(parent.genes[i]);
                }
                else
                {
                    genes.Add(partner.genes[i]);
                }
            }
        }
    }
}
