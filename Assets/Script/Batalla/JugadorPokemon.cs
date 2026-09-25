using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JugadorPokemon : MonoBehaviour
{
    
    [SerializeField] bool pokemonJugador;
    
    
    public Pokemon Pokemon { get; set; }

    Image imagen;
    Vector3 posicionoriginal;
    Color colorOriginal;

    private void Awake()
    {
        imagen = GetComponent<Image>();
        posicionoriginal = imagen.transform.localPosition;
        colorOriginal = imagen.color;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if(Pokemon == null || Pokemon.Base == null)
        {
            Debug.LogWarning("NULO");
            imagen.sprite = null;
            imagen.color = new Color(1,1,1,0);
            return;
        }
        if (pokemonJugador)
            imagen.sprite = Pokemon.Base.Spritedetras;
            else
            imagen.sprite = Pokemon.Base.Spritefrente;
            
            imagen.color = colorOriginal;
            IniciarAnimaciondeEntrada();
    }

    public void IniciarAnimaciondeEntrada()
    {
        if(pokemonJugador)
        imagen.transform.localPosition = new Vector3(-500f, posicionoriginal.y);
        else
        imagen.transform.localPosition = new Vector3(500f, posicionoriginal.y);

        imagen.transform.DOLocalMoveX(posicionoriginal.x, 1f);
    }

    public void IniciarAnimaciondAtaque()
    {
        var secuencia = DOTween.Sequence();
        if (pokemonJugador)
        secuencia.Append(imagen.transform.DOLocalMoveX(posicionoriginal.x + 50f, 0.25f));
        else
        secuencia.Append(imagen.transform.DOLocalMoveX(posicionoriginal.x - 50f, 0.25f));

        secuencia.Append(imagen.transform.DOLocalMoveX(posicionoriginal.x, 0.25f));
    }

    public void IniciarAnimaciondGolpe()
    {
        var secuencia = DOTween.Sequence();
        secuencia.Append(imagen.DOColor(Color.gray, 0.1f));
        secuencia.Append(imagen.DOColor(colorOriginal, 0.1f));
    } 

    public void IniciarAnimaciondDerrota()
    {
        var secuencia = DOTween.Sequence();
        secuencia.Append(imagen.transform.DOLocalMoveY(posicionoriginal.y - 150f, 0.5f));
        secuencia.Join(imagen.DOFade(0f, 0.5f));
    }
}
