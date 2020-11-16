using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveZone : MonoBehaviour
{
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private int waveIndex = 0;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public UnityEvent OnWaveCleared;

    private IEnumerator StartWaves()
    {
        GameManager.Instance.InvokeWave(waves[waveIndex], spawnPoints, this);

        while(waves[waveIndex].enemies.Count > 0)
        {
            yield return null;
        }
        waveIndex++;

        if (waveIndex < waves.Count)
        {
            StartCoroutine(StartWaves());
        }
        else
            OnWaveCleared.Invoke();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        waves[waveIndex].enemies.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(StartWaves());
    }
}
