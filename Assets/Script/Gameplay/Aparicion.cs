using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aparicion : MonoBehaviour
{
     [SerializeField] List<Pokemon> pokemonsalvajes;

     public Pokemon GetRandomPokemonSalvaje()
     {
        var pokemonsalvaje = pokemonsalvajes[Random.Range(0, pokemonsalvajes.Count)];
        pokemonsalvaje.Init();
        return pokemonsalvaje;
     }
    
}
