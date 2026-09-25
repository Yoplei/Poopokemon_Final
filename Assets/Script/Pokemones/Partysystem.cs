using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Partysystem : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemones;

    private void Start()
    {
        foreach (var pokemon in pokemones)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetPokemonVivo()
        {
            return pokemones.Where(x => x.PS > 0).FirstOrDefault();
        }
    
    public bool TodosDebilitados()
    {
        return pokemones.All(p => p.PS <= 0);
    }

    public void CurarTodosLosPokemon()
    {
        foreach (var pokemon in pokemones)
        {
            pokemon.Curar();
        }
    }
}
