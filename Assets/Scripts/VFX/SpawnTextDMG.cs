using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnTextDMG : MonoBehaviour
{
    public TextMeshPro txtDmg;
    public Transform spawnPosition;

    public void Spawn(float dmg, bool critical)
    {
        StartCoroutine(SpawnTextDamage(dmg, critical));
    }

    IEnumerator SpawnTextDamage(float dmg, bool critical)
    {
        txtDmg.text = dmg.ToString("F0");

        if (critical)
            txtDmg.color = Color.red;
        else
            txtDmg.color = Color.white;

        Instantiate(txtDmg, spawnPosition.position, Quaternion.Euler(0, 0, 0));
        yield return null;
    }

}