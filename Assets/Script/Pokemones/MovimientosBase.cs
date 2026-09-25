using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "Movimiento", menuName = "Pokemon/Crear Movimiento")]

public class MovimientosBase :  ScriptableObject
{
    [SerializeField] string nombre;

    [TextArea]
    [SerializeField] string descripcion;
    
    [SerializeField] Tipo tipo;
    [SerializeField] int poder;
    [SerializeField] int presicion;
    [SerializeField] int pp;

    public string Nombre{
        get{return nombre;}
    }

    public string Descripcion{
        get{return descripcion;}
    }

    public Tipo Tipo{
        get{return tipo;}
    }

    public int Poder{
        get{return poder;}
    }

    public int Presicion{
        get{return presicion;}
    }

    public int PP{
        get{return pp;}
    }

    public bool EsEspecial{
        get{
            if (tipo == Tipo.Fuego || tipo == Tipo.Agua || tipo == Tipo.Planta || tipo == Tipo.Hielo || tipo == Tipo.Electrico || tipo == Tipo.Dragon ) 
            {
                return true;
            }      
            else{
                return false;
            }
        
        
        }
    }
}
