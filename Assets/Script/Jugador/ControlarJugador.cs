using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlarJugador : MonoBehaviour
{
    public float velocidad;
    public LayerMask ObjetosLayer;
    public LayerMask HierbaAltaLayer;
    private bool movimiento;
    private Vector2 input;
    public event Action Onencuentro; 

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if (!movimiento)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("MoverX", input.x);
                animator.SetFloat("MoverY", input.y);

                var Posicion = transform.position;
                Posicion.x += input.x;
                Posicion.y += input.y;

                if (IsWalkable(Posicion))
                    StartCoroutine(Move(Posicion));
            }
        }

        animator.SetBool("movimiento", movimiento);
    }


    IEnumerator Move(Vector3 Posicion)
    {
        movimiento = true;

        while ((Posicion - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, Posicion, velocidad * Time.deltaTime);
            yield return null;
        }
        transform.position = Posicion;

        movimiento = false;

        Encuentros();
    }

    private bool IsWalkable(Vector3 Posicion)
    {
        if (Physics2D.OverlapCircle(Posicion, 0.2f, ObjetosLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void Encuentros()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, HierbaAltaLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                 animator.SetBool("movimiento", false);
                Onencuentro();
            }
        }
    }
}