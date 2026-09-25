using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarradeVida : MonoBehaviour
{
    [SerializeField] GameObject vida;

    
    public void SetPS(float psnormales)
    {
        vida.transform.localScale = new Vector3(psnormales, 1f);
    }

    public IEnumerator SetPSGradual(float nuevoPS)
    {
        float actualPS = vida.transform.localScale.x;
        float cambiarV = actualPS - nuevoPS;

        while ( actualPS - nuevoPS > Mathf.Epsilon)
        {
            actualPS -= cambiarV * Time.deltaTime;
            vida.transform.localScale = new Vector3(actualPS, 1f);
            yield return null;
        }
        vida.transform.localScale = new Vector3(nuevoPS, 1f);
    }
}
