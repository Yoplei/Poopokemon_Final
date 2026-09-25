using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Barradeexp : MonoBehaviour
{

[SerializeField] GameObject experiencia;
[SerializeField] float velocidaddAnimacion = 2f;

    public void SetExp(float expNormal)
    {
        expNormal = Mathf.Clamp01(expNormal);
        experiencia.transform.localScale = new Vector3(expNormal, 1f, 1f);
    }

    public IEnumerator SetExpGradual(float nuevaExp)
    {
         nuevaExp = Mathf.Clamp01(nuevaExp);
        float expActual = experiencia.transform.localScale.x;
       
       if (nuevaExp < expActual)
        {
            SetExp(nuevaExp);
            yield break;
        }

        while (Mathf.Abs(expActual - nuevaExp) > 0.01f)
        {
            expActual = Mathf.MoveTowards(expActual, nuevaExp, velocidaddAnimacion * Time.deltaTime);
            experiencia.transform.localScale = new Vector3(expActual, 1f, 1f);
            yield return null;
        }
        
        
        SetExp(nuevaExp);
    
    }

    public IEnumerator AnimarSubidaNivel(float expFinalNuevoNivel)
    {
        
        yield return SetExpGradual(1f);
        
        
        yield return new WaitForSeconds(0.5f);
        
        
        SetExp(0f);
        yield return new WaitForSeconds(0.2f);
        
       
        yield return SetExpGradual(expFinalNuevoNivel);
    }

     public float GetExpActual()
    {
        return experiencia.transform.localScale.x;
    }

    public void SetVelocidadAnimacion(float nuevaVelocidad)
    {
        velocidaddAnimacion = nuevaVelocidad;
    }
}


