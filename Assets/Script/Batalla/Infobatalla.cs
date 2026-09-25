using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infobatalla : MonoBehaviour
{
   [SerializeField] Text nombreText;
    [SerializeField] Text nivelText;
    [SerializeField] BarradeVida barradeVida;
     [SerializeField] Barradeexp barradeExp;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nombreText.text = pokemon.Base.Nombre;
        nivelText.text = "Nv" + pokemon.Nivel;

        if(barradeExp != null)
        {
            barradeExp.SetExp(pokemon.GetPorcentajeExp());
        }


        barradeVida.SetPS((float)pokemon.PS / pokemon.MaxPs);
    }

    public IEnumerator ActualizarExp()
    {
        if(barradeExp != null && _pokemon != null)
        {
            yield return barradeExp.SetExpGradual(_pokemon.GetPorcentajeExp());
        }
        yield return null;
    }

    public IEnumerator ActualizarExpConSubida(float expFinalNuevoNivel)
    {
        if(barradeExp != null)
        {
            yield return barradeExp.AnimarSubidaNivel(expFinalNuevoNivel);
        }
    }

    public IEnumerator ActualizarPS()
    {
       yield return barradeVida.SetPSGradual((float)_pokemon.PS / _pokemon.MaxPs);
    }

}
