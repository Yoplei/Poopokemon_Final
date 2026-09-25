using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Crear nuevo pokemon")]

public class Pokemones : ScriptableObject
{
    [SerializeField]string nombre;

    [TextArea]
    [SerializeField]string descripcion;
    
    [SerializeField] Sprite spritefrente;
    [SerializeField] Sprite spritedetras;

    [SerializeField] Tipo tipo1;
    [SerializeField] Tipo tipo2;

    [SerializeField] int maxPs;
    [SerializeField] int ataque;
    [SerializeField] int defensa;
    [SerializeField] int spAtaque;
    [SerializeField] int spDefensa;
    [SerializeField] int velocidad;


    [SerializeField] int exp;
    [SerializeField] Crecimiento crecimiento;


    [SerializeField] List<AprenderMovimientos> aprenderMovimientos;

    public int NivelMaximo = 100;

    public int GetExporNivel(int nivel)
    {   
    if(crecimiento == Crecimiento.Rapido)
    {
        return 4*(nivel * nivel * nivel)/5;
    }
    else if(crecimiento == Crecimiento.MedioRapido)
    {
        return nivel*nivel*nivel;
    }
    return -1;
    }

    public string Nombre{
       get {return nombre;}
    }

    public string Descripcion{
        get {return descripcion;}
    }

    public Sprite Spritefrente{
        get {return spritefrente;}
    }

    public Sprite Spritedetras{
        get {return spritedetras;}
    }

     public Tipo Tipo1{
        get {return tipo1;}
    }

    public Tipo Tipo2{
        get {return tipo2;}
    }

    public int MaxPs{
        get {return maxPs;}
    }

    public int Ataque{
        get {return ataque;}
    }

    public int Defensa{
        get {return defensa;}
    }

    public int SpAtaque{
        get {return spAtaque;}
    }

    public int SpDefensa{
        get {return spDefensa;}
    }
    
    public int Velocidad{
        get {return velocidad;}
    }

    public List<AprenderMovimientos> AprenderMovimientos{
        get{return aprenderMovimientos;}
    }

    public int Exp => exp;

    public Crecimiento Crecimiento => crecimiento;
}

[System.Serializable]
public class AprenderMovimientos
{
    [SerializeField] MovimientosBase movimientosBase;
    [SerializeField] int nivel;

    public MovimientosBase Base{
        get{ return movimientosBase;}
    }

    public int Nivel{
        get{return nivel;}
    } 

}

public enum Tipo
{
    Ninguno,
    Normal,
    Fuego,
    Agua,
    Planta,
    Electrico,
    Volador,
    Hielo,
    Lucha,
    Veneno,
    Tierra,
    Roca,
    Psiquico,
    Bicho,
    Fantasma,
    Dragon,
    Hada, 
    Siniestro
}

public enum Crecimiento
{
    Rapido,MedioRapido
}
public class Tipochart
{
   static float[][] chart = 
{       
     //               NOR FIR AGU PLA ELE VOL HIE LUC VEN TIE ROC PSI BIC FAN DRA HAD
        /*Normal*/   new float[] {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 1f, 0f, 1f, 1f},
        /*Fuego*/    new float[] {1f, 0.5f, 0.5f, 2f, 1f, 1f, 2f, 1f, 1f, 1f, 2f, 1f, 2f, 1f, 0.5f, 1f},
        /*Agua*/     new float[] {1f, 2f, 0.5f, 0.5f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 1f, 0.5f, 1f},
        /*Planta*/   new float[] {1f, 0.5f, 2f, 0.5f, 1f, 0.5f, 1f, 1f, 0.5f, 2f, 2f, 1f, 0.5f, 1f, 0.5f, 1f},
        /*Electrico*/new float[] {1f, 1f, 2f, 0.5f, 0.5f, 2f, 1f, 1f, 1f, 0f, 1f, 1f, 1f, 1f, 0.5f, 1f},
        /*Volador*/  new float[] {1f, 1f, 1f, 2f, 0.5f, 1f, 1f, 1f, 1f, 1f, 0.5f, 1f, 2f, 1f, 0.5f, 1f},
        /*Hielo*/    new float[] {1f, 0.5f, 0.5f, 2f, 1f, 2f, 0.5f, 1f, 1f, 2f, 1f, 1f, 1f, 1f, 2f, 1f},
        /*Lucha*/    new float[] {2f, 1f, 1f, 1f, 1f, 0.5f, 2f, 1f, 0.5f, 1f, 2f, 0.5f, 0.5f, 0f, 1f, 2f},
        /*Veneno*/   new float[] {1f, 1f, 1f, 2f, 1f, 1f, 1f, 1f, 0.5f, 0.5f, 0.5f, 1f, 1f, 0.5f, 1f, 2f},
        /*Tierra*/   new float[] {1f, 2f, 1f, 0.5f, 2f, 0f, 1f, 1f, 2f, 1f, 2f, 1f, 0.5f, 1f, 1f, 1f},
        /*Roca*/     new float[] {1f, 0.5f, 2f, 1f, 1f, 2f, 1f, 0.5f, 0.5f, 0.5f, 1f, 1f, 2f, 1f, 1f, 1f},
        /*Psiquico*/ new float[] {1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 2f, 1f, 1f, 0.5f, 1f, 1f, 1f, 0f},
        /*Bicho*/    new float[] {1f, 0.5f, 1f, 2f, 1f, 0.5f, 1f, 0.5f, 1f, 1f, 1f, 2f, 1f, 0.5f, 1f, 0.5f},
        /*Fantasma*/ new float[] {0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 2f, 1f, 1f},
        /*Dragon*/   new float[] {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 0f},
        /*Hada*/     new float[] {1f, 0.5f, 1f, 1f, 1f, 1f, 1f, 0.5f, 2f, 1f, 1f, 1f, 1f, 1f, 2f, 1f}
    };

     public static float ObtenerEfectividad(Tipo tipoAtaque, Tipo tipoDefensa)
    {
        // Si el tipo es "Ninguno" o está fuera de rango, devuelve 1
        if (tipoAtaque == Tipo.Ninguno || tipoDefensa == Tipo.Ninguno) return 1f;

        int fila = (int)tipoAtaque - 1;
        int columna = (int)tipoDefensa - 1;
        return chart[fila][columna];
    }
}