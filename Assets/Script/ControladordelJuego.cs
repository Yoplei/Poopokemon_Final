using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Escenariodeljuego {Mundo, Batalla}

public class ControladordelJuego : MonoBehaviour
{
    [SerializeField] ControlarJugador controlarjugador;
    [SerializeField] SistemadeBatalla sistemadebatalla;
    [SerializeField] Camera camaradeMundo;
    
    Escenariodeljuego escenario;

    private void Start()
    {
        controlarjugador.Onencuentro += IniciarBatalla;
        sistemadebatalla.OnBatallaTerminada += TerminarBatalla;
    }


    void IniciarBatalla()
    {
        escenario = Escenariodeljuego.Batalla;
        sistemadebatalla.gameObject.SetActive(true);
        camaradeMundo.gameObject.SetActive(false);
        
        var jugadorparty = controlarjugador.GetComponent<Partysystem>();
        var pokemonsalvaje = Object.FindAnyObjectByType<Aparicion>().GetComponent<Aparicion>().GetRandomPokemonSalvaje();
        
        sistemadebatalla.IniciarBatalla(jugadorparty, pokemonsalvaje);
    }

     void TerminarBatalla(bool ganar)
    {
        escenario = Escenariodeljuego.Mundo;
        sistemadebatalla.gameObject.SetActive(false);
        camaradeMundo.gameObject.SetActive(true);
        
    }

    private void Update()
    {

        if (escenario == Escenariodeljuego.Mundo)
        {
            controlarjugador.HandleUpdate();
        }
        else if (escenario == Escenariodeljuego.Batalla)
        {
            sistemadebatalla.HandleUpdate();
        }
    }
}
