using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalspawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int numGoal;
    [SerializeField] private float xRange;
    [SerializeField] private float zRange;

    private void Start()
    {
        for (int i = 0; i < numGoal; i++)
        {
            float xPos = Random.Range(-xRange, xRange);
            float zPos = Random.Range(-zRange, zRange);
            Vector3 spawnPos = new Vector3(xPos, 0f, zPos);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
