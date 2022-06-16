using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DadosRoda : MonoBehaviour
{
    public GameObject dice;
    public GameObject diceTxt;

    private bool _rolling;
    private bool _startRoll; 
    public bool roll;
    private bool _wait;

    public Animator anmDice;
    public AnimationClip[] clips;

    private float _timer;
    private float _anmTimer;
    public float fadeDelay = 5;
    private float _timerReserva;
 
    public int result;
    private static readonly int Rodar = Animator.StringToHash("Rodar");

    // Start is called before the first frame update
    private void Start()
    {
        diceTxt.SetActive(false);
        dice.SetActive(false);
        _timerReserva = fadeDelay;
        clips = anmDice.runtimeAnimatorController.animationClips;

        foreach(var clip in clips)
        {
            if(clip.name == "Rodada")
            {
                //Para confirmar se ele ja chegou no objetivo para ir para o proximo ponto
                _timer = clip.length;
                break;    
            }
        }

        _anmTimer = _timer;
        
    }

    // Update is called once per frame
    private void Update()
    {
        Rodando();
    }

    private void Rodando()
    {
        
        if (roll)
        {
            Time.timeScale = 0;
            result = Random.Range(1,20);
            _startRoll = true;
            _rolling = true;
            roll = false;
            dice.SetActive(true);
        }

        RotationTime();

    }

    private void RotationTime()
    {
        if (_rolling)
        {
           if(_startRoll)
           {
              anmDice.SetTrigger(Rodar);
              _startRoll = false;
                     
           }

           if (_anmTimer > 0)
               _anmTimer -= Time.unscaledDeltaTime;

           else
           {
               _rolling = false;
               SetNumber();
               _anmTimer = _timer;
           }
        }
        else if(_wait)
        {
            if (_timerReserva < 4)
            {
                diceTxt.SetActive(true);
            }

            if (_timerReserva > -1)
            {
                _timerReserva -= Time.unscaledDeltaTime;
            }
    
            else
            {
                _wait = false;
                _timerReserva = fadeDelay;
                dice.SetActive(false);
                diceTxt.SetActive(false);
                Time.timeScale = 1;
            }

        }

    }

    private void SetNumber()
    {
        diceTxt.GetComponent<TMP_Text>().SetText(result.ToString());
        _wait = true;
    }
}
