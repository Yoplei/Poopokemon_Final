using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Estadodelabatalla {Inicio, Acciondejugador, Movimientodejugador, Movimientodeenemigo, busy }

public class SistemadeBatalla : MonoBehaviour
{
    [SerializeField] JugadorPokemon jugadorP;
    [SerializeField] JugadorPokemon enemigoP;
    [SerializeField] Infobatalla jugadorB;
    [SerializeField] Infobatalla enemigoB;
    [SerializeField] BatallaMensajes mensajesB;

    public event Action<bool> OnBatallaTerminada;

    Estadodelabatalla estado;
    int accionactual;
    int movimientoactual;

    Partysystem jugadorparty;
    Pokemon pokemonsalvaje;

    private bool batallaActiva;
    private bool expOtorgada;


    public void IniciarBatalla(Partysystem jugadorparty, Pokemon pokemonsalvaje)
    {
        this.jugadorparty = jugadorparty;
        this.pokemonsalvaje = pokemonsalvaje;

        
        batallaActiva = true;
        expOtorgada = false;
        StartCoroutine(SetupBatalla());
    }

    public IEnumerator SetupBatalla()
    {
        var pokemonJugador = jugadorparty.GetPokemonVivo();

        if (pokemonJugador != null)
        {
            jugadorP.Setup(pokemonJugador);
            jugadorB.SetData(jugadorP.Pokemon);
            mensajesB.SetMovimientosNombre(jugadorP.Pokemon.Movimientos);
        }
        else{
            Debug.LogWarning("Todos tus pokemon han sido debilitados");
            jugadorparty.CurarTodosLosPokemon();
            OnBatallaTerminada(false);
            yield break;
        }
        
        enemigoP.Setup(pokemonsalvaje);
        enemigoB.SetData(enemigoP.Pokemon);

        mensajesB.SetMovimientosNombre(jugadorP.Pokemon.Movimientos);

        yield return mensajesB.TypeDialog($"Un {enemigoP.Pokemon.Base.Nombre} aparecio.");
        
        yield return new WaitForSeconds(1f);
        Acciondejugador();

        
    }

    void Acciondejugador()
    {
        estado = Estadodelabatalla.Acciondejugador;
        StartCoroutine(mensajesB.TypeDialog("Que deberia hacer"));
        mensajesB.ActivarSeleccionarAccion(true);
    }

    void Movimientodejugador()
    {
        estado = Estadodelabatalla.Movimientodejugador;
        mensajesB.ActivarSeleccionarAccion(false);
        mensajesB.ActivarMensajes(false);
        mensajesB.ActivarSelectordeMovimientos(true);
    }
     
     IEnumerator RealizarMdJugador()
     {
        estado=Estadodelabatalla.busy;

        var movimientos = jugadorP.Pokemon.Movimientos[movimientoactual];
        movimientos.PP--;
        yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} uso {movimientos.Base.Nombre}");
        
        jugadorP.IniciarAnimaciondAtaque();
        yield return new WaitForSeconds(1f);

        enemigoP.IniciarAnimaciondGolpe();

        var detallesdedano = enemigoP.Pokemon.RecibirDano(movimientos, jugadorP.Pokemon);
        yield return enemigoB.ActualizarPS();
        yield return VerDetallesdeDano(detallesdedano);

        if (enemigoP.Pokemon.PS <= 0)
        {
            yield return HandlePokemonDerrotado(enemigoP);
        }
        else
        {
            StartCoroutine(Movimientodeenemigo());
        }
     }

     IEnumerator Movimientodeenemigo()
     {
        estado = Estadodelabatalla.Movimientodeenemigo;

        var movimiento = enemigoP.Pokemon.GetMovRandom();
        movimiento.PP--;
        yield return mensajesB.TypeDialog($"{enemigoP.Pokemon.Base.Nombre} uso {movimiento.Base.Nombre}");
        
        enemigoP.IniciarAnimaciondAtaque();
        yield return new WaitForSeconds(1f);

        jugadorP.IniciarAnimaciondGolpe();

        var detallesdedano = jugadorP.Pokemon.RecibirDano(movimiento, jugadorP.Pokemon);
        yield return jugadorB.ActualizarPS();
        yield return VerDetallesdeDano(detallesdedano);

        if (detallesdedano.Derrotado)
        {
            yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} Derrotado"); 
            jugadorP.IniciarAnimaciondDerrota();
             
             yield return new WaitForSeconds(2f);
             var siguientepokemon = jugadorparty.GetPokemonVivo();
             
             if(siguientepokemon !=null)
             {
                jugadorP.Setup(siguientepokemon);
                jugadorB.SetData(siguientepokemon);
                

        mensajesB.SetMovimientosNombre(siguientepokemon.Movimientos);
         yield return mensajesB.TypeDialog($"Tu puedes {siguientepokemon.Base.Nombre}");
            Acciondejugador();
             }
             else
             {
                OnBatallaTerminada(false);
             }
            
        }
        else
        {
            Acciondejugador();
        }
     }

     IEnumerator VerDetallesdeDano(Detallesdedano detallesdedano)
     {
        if(detallesdedano.Critico > 1f)
        yield return mensajesB.TypeDialog("Un Golpe Critico!");

        if (detallesdedano.TipoEfectividades > 1f)
        yield return mensajesB.TypeDialog("Es Super Efectivo!");
        
        else if (detallesdedano.TipoEfectividades < 1f)
        yield return mensajesB.TypeDialog("No Es Muy Efectivo!");
     }


    public void HandleUpdate()
    {
        if(estado == Estadodelabatalla.Acciondejugador)
        {
            ManejarSeleccion();
        }
        else if (estado == Estadodelabatalla.Movimientodejugador)
        {
            ManejarMovimiento();
        }
    }

    void ManejarSeleccion()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(accionactual < 1)
            ++accionactual;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(accionactual > 0)
            --accionactual;
        }
        
        mensajesB.ActualizacionSeleccionAccion(accionactual);

        if(Input.GetKeyDown(KeyCode.Z))
        {
                 if(accionactual == 0)
                 {
                    //pelea
                    Movimientodejugador();
                 }
                 else if (accionactual == 1)
                 {
                    //huir
                    if(pokemonsalvaje != null)
                    {
                        StartCoroutine(HuirDeBatalla());
                    }
                    else
                    {
                        StartCoroutine(mensajesB.TypeDialog("No puedes huir de esta batalla!"));
                    }
                 }
        }
    }
    void ManejarMovimiento()
        {
             if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(movimientoactual < jugadorP.Pokemon.Movimientos.Count - 1)
            ++movimientoactual;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(movimientoactual > 0)
            --movimientoactual;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(movimientoactual < jugadorP.Pokemon.Movimientos.Count - 2)
            movimientoactual += 2;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (movimientoactual > 1)
            movimientoactual -=2;
        }
        mensajesB.ActualizacionSeleccionMovimiento(movimientoactual, jugadorP.Pokemon.Movimientos[movimientoactual]);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            mensajesB.ActivarSelectordeMovimientos(false);
            mensajesB.ActivarMensajes(true);
            StartCoroutine(RealizarMdJugador());
        }
        
    }

    IEnumerator HandlePokemonDerrotado(JugadorPokemon pokemonDerrotado)
    {
        
        yield return mensajesB.TypeDialog($"{pokemonDerrotado.Pokemon.Base.Nombre} derrotado");
        pokemonDerrotado.IniciarAnimaciondDerrota();
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(DarExperiencia(pokemonDerrotado));
       OnBatallaTerminada(true);

       accionactual = 0;
       movimientoactual = 0;
       estado = Estadodelabatalla.Inicio;
       
       }

        IEnumerator DarExperiencia (JugadorPokemon pokemonDerrotado)
        {
            int expBase = pokemonDerrotado.Pokemon.Base.Exp;
            int niveldeenemigo = pokemonDerrotado.Pokemon.Nivel;

            float multiplicador = 1f;
            int expGanada = Mathf.FloorToInt((expBase * niveldeenemigo * multiplicador)/7);

            yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} gano {expGanada} puntos de experiencia!");

            int nivelAnterior = jugadorP.Pokemon.Nivel;
            bool subiodeNivel = jugadorP.Pokemon.GanarExperiencia(expGanada);
             

            if(subiodeNivel)
            {
                yield return jugadorB.ActualizarExpConSubida(jugadorP.Pokemon.GetPorcentajeExp());
                yield return StartCoroutine(ManejarSubidaDeNivel());
            }
            else{
                yield return jugadorB.ActualizarExp();
            }
            
            yield return new WaitForSeconds(1f);
        }
        
         
         
    IEnumerator ManejarSubidaDeNivel()
    {
        
        yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} subio al nivel {jugadorP.Pokemon.Nivel}!");
        
        
        jugadorB.SetData(jugadorP.Pokemon);
    
       
        
        yield return new WaitForSeconds(2f);
    }

   // IEnumerator VerificarNuevosMovimientos()
   // {
       
        //var nuevosMovimientos = jugadorP.Pokemon.Base.GetMovimientosDisponibles();
        
       // foreach (var movimiento in nuevosMovimientos)
       // {
            
          //  if (jugadorP.Pokemon.AprenderMovimiento(movimiento))
          //  {
           //     yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} aprendio {movimiento.Nombre}!");
//
  //              mensajesB.SetMovimientosNombre(jugadorP.Pokemon.Movimientos);
    //        }
      //      else if(jugadorP.Pokemon.Movimientos.Count >= 4)
        //    {
          //      yield return mensajesB.TypeDialog($"{jugadorP.Pokemon.Base.Nombre} quiere aprender {movimiento.Nombre}, pero ya conoce 4 movimientos");
            //}
        //}
        //
      //  yield return null;
//    }

    

    IEnumerator HuirDeBatalla()
{
    estado = Estadodelabatalla.busy;

    int chance = UnityEngine.Random.Range(0, 100);
    if (chance < 75) 
    {
        yield return mensajesB.TypeDialog("Huiste exitosamente!");
        yield return new WaitForSeconds(1f);
        OnBatallaTerminada(false);
    }
    else
    {
        yield return mensajesB.TypeDialog("No pudiste escapar!");
        StartCoroutine(Movimientodeenemigo());
    }
}
}

