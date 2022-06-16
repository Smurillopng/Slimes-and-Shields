using NaughtyAttributes;
using UnityEngine;

public class DebuffCheck : MonoBehaviour
{
    [ReadOnly] public bool canPoison; // O ataque do player pode envenenar o inimigo
    [ReadOnly] public bool canSlow; // O ataque do player pode reduzir a velocidade do inimigo
    [ReadOnly] public bool canStun; // O ataque do player pode atordoar o inimigo
    [ReadOnly] public bool canBurn; // O ataque do player pode queimar o inimigo
    [ReadOnly] public bool canFreeze; // O ataque do player pode congelar o inimigo
    [ReadOnly] public bool canRoot; // O ataque do player pode travar o inimigo
}