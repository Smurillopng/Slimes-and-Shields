using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject boss;
    public GameObject teleporter;
    public GameObject reward;
    private DadosRoda _diceRoll;
    private bool _activated;

    private void Start()
    {
        _diceRoll = FindObjectOfType<DadosRoda>();
    }

    private void Update()
    {
        if (_activated is true) return;
        if (!boss.GetComponent<EnemyController>().morreu) return;
        _activated = true;
        teleporter.SetActive(true);
        _diceRoll.roll = true;
        Instantiate(reward, transform.position, Quaternion.identity);
    }
}
