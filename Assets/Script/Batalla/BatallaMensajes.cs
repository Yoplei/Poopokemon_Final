using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatallaMensajes : MonoBehaviour
{
    [SerializeField] int letrasporsegundo;
    [SerializeField] Color colorS; 

    [SerializeField] Text mensajes;
    [SerializeField] GameObject selectordemovimientos;
    [SerializeField] GameObject seleccionaraccion;
    [SerializeField] GameObject detallesdemovimiento;
    
    [SerializeField] List<Text> textodeAccion;
    [SerializeField] List<Text> textodeMovimiento;

     [SerializeField] Text textopp;
      [SerializeField] Text textotipo;
 
    public void SetDialog(string dialog)
    { 
        mensajes.text = dialog;
    }
    public IEnumerator TypeDialog(string dialog)
    {
        mensajes.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            mensajes.text += letter;
            yield return new WaitForSeconds(1f / letrasporsegundo);
        } 

        yield return new WaitForSeconds (1f);
    }

    public void ActivarMensajes(bool activado)
    {
        mensajes.enabled = activado;
    }

    public void ActivarSeleccionarAccion(bool activado)
    {
        seleccionaraccion.SetActive(activado);
    }

    public void ActivarSelectordeMovimientos(bool activado)
    {
        selectordemovimientos.SetActive(activado);
        detallesdemovimiento.SetActive(activado);
    }

    public void ActualizacionSeleccionAccion(int accionseleccionada)
    {
        for (int i=0; i < textodeAccion.Count; ++i)
        {
            if (i == accionseleccionada)
            textodeAccion[i].color=colorS;
            else
            textodeAccion[i].color=Color.black;
        }
    }

    public void ActualizacionSeleccionMovimiento(int movimientoseleccionado, Movimiento movimientos)
    {
        for (int i=0; i<textodeMovimiento.Count; i++)
        {
            if( i == movimientoseleccionado)
            textodeMovimiento[i].color = colorS;
            else
            textodeMovimiento[i].color = Color.black;
        }
         textopp.text = $"PP {movimientos.PP}/{movimientos.Base.PP}";
        textotipo.text = movimientos.Base.Tipo.ToString();
    }

    public void SetMovimientosNombre(List<Movimiento> movimientos)
    {
        for (int i=0; i<textodeMovimiento.Count; i++)
        {
            if (i < movimientos.Count)
            textodeMovimiento[i].text = movimientos[i].Base.Nombre;
            else
            textodeMovimiento[i].text = "-";
        }

       
    }
}
