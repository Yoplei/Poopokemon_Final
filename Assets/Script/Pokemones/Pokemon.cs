using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Pokemon
{
    [SerializeField] Pokemones _base;
    [SerializeField] int nivel;
   


    public Pokemones Base {
    get {
        return _base;
    }
}
    public int Nivel{
        get{ 
            return nivel;}
    }

    public int Exp{get; set;}

    public int PS {get; set;}
    
    public List<Movimiento> Movimientos {get; set;}

    public void Init()
    {
    PS = MaxPs;
        
        Movimientos = new List<Movimiento>();
        foreach (var movimiento in Base.AprenderMovimientos)
        {
            if(movimiento.Base !=null && movimiento.Nivel <= nivel)
            {
            Movimientos.Add(new Movimiento(movimiento.Base));

            if(Movimientos.Count >= 4)
            break;
            }
            else if (movimiento.Base == null)
            {
                Debug.LogWarning($"Movimiento nulo en {Base.Nombre} en el nivel {movimiento.Nivel}", Base);
            }
        }

        Exp = Base.GetExporNivel(Nivel);
    }

    public int Ataque{
        get{return Mathf.FloorToInt((Base.Ataque * nivel)/ 100f) + 5; }
    }

    public int Defensa{
        get{return Mathf.FloorToInt((Base.Defensa * nivel)/ 100f) + 5; }
    }

    public int SpAtaque{
        get{return Mathf.FloorToInt((Base.SpAtaque * nivel)/ 100f) + 5; }
    }

    public int SpDefensa{
        get{return Mathf.FloorToInt((Base.SpDefensa * nivel)/ 100f) + 5; }
    }

    public int Velocidad{
        get{return Mathf.FloorToInt((Base.Velocidad * nivel)/ 100f) + 5; }
    }

    public int MaxPs{
        get{return Mathf.FloorToInt((Base.Velocidad * nivel)/ 100f) + 10; }
    }

    public Detallesdedano RecibirDano(Movimiento movimiento,  Pokemon atacante)
    {
        float critico = 1f;    
        if(Random.value * 100f <= 6.25f)
        critico = 2f;

        float tipo = Tipochart.ObtenerEfectividad(movimiento.Base.Tipo, this.Base.Tipo1) * Tipochart.ObtenerEfectividad(movimiento.Base.Tipo, this.Base.Tipo2);


        var detallesdedano = new Detallesdedano()
        {
           TipoEfectividades = tipo,
           Critico = critico,
           Derrotado = false

        };

        float ataque = (movimiento.Base.EsEspecial) ? atacante.SpAtaque : atacante.Ataque;
        float defensa = (movimiento.Base.EsEspecial) ? SpDefensa : Defensa; 
        
        float modificar = Random.Range(0.85f, 1f) * tipo * critico;
        float a =(2* atacante.nivel + 10)/ 250f;
        float d = a*movimiento.Base.Poder * ((float)ataque/defensa) + 2;
        int dano = Mathf.FloorToInt(d * modificar);

        PS -= dano;
        if(PS <= 0)
        {
            PS = 0;
            detallesdedano.Derrotado = true;
        }  
        return detallesdedano;
    }

    public Movimiento GetMovRandom()
    {
        int r = Random.Range(0, Movimientos.Count);
        return Movimientos[r];
    }

    public bool GanarExperiencia(int expGanada)
    {
        int nivelAnterior = nivel;
        Exp += expGanada;
        
        
        while (Exp >= Base.GetExporNivel(nivel + 1) && nivel < Base.NivelMaximo)
        {
            nivel++;
        }
        
        
        if (nivel > nivelAnterior)
        {
            CalcularStats();
            return true;
        }
        
        return false;
    }

     public void CalcularStats()
    {
        
        float porcentajePS = (float)PS / MaxPs;
        
        
        int nuevosMaxPs = MaxPs;
        PS = Mathf.FloorToInt(nuevosMaxPs * porcentajePS);
        
        
        if (PS <= 0 && porcentajePS > 0)
            PS = 1;
    }

     public bool TieneMovimiento(MovimientosBase movimiento)
    {
        foreach (var mov in Movimientos)
        {
            if (mov.Base == movimiento)
                return true;
        }
        return false;
    }

    public bool AprenderMovimiento(MovimientosBase nuevoMovimiento)
    {
        
        if (TieneMovimiento(nuevoMovimiento))
            return false;
        
        
        if (Movimientos.Count < 4)
        {
            Movimientos.Add(new Movimiento(nuevoMovimiento));
            return true;
        }
        
        
        return false;
    }

    public List<MovimientosBase> GetMovimientosDisponibles()
    {
        List<MovimientosBase> movimientosDisponibles = new List<MovimientosBase>();
        
        foreach (var movimiento in Base.AprenderMovimientos)
        {
            if (movimiento.Nivel == nivel && !TieneMovimiento(movimiento.Base))
            {
                movimientosDisponibles.Add(movimiento.Base);
            }
        }
        
        return movimientosDisponibles;
    }

    public float GetPorcentajeExp()
    {
        if (nivel >= Base.NivelMaximo)
            return 1f;
            
        int expNivelActual = Base.GetExporNivel(nivel);
        int expSiguienteNivel = Base.GetExporNivel(nivel + 1);
        
        return (float)(Exp - expNivelActual) / (expSiguienteNivel - expNivelActual);
    
}

    public void Curar()
    {
        PS = MaxPs;
        foreach (var mov in Movimientos)
        {
            mov.PP = mov.Base.PP;
        }
    }

}

public class Detallesdedano
{
    public bool Derrotado {get; set;}
    public float Critico {get; set;}
    public float TipoEfectividades {get; set;}
}