using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private PlayerProfile player = null;
    [SerializeField] private Transform playerInitialPoint = null;
    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

    [SerializeField] private Slider playerHealthBar = null;

    public GameObject healthPrefab;

    private PlayerCharacter playerCharacter;

    private void Start()
    {
        InvokePlayer();
    }

    public void InvokePlayer()
    {
        playerCharacter = Instantiate(playerPrefab, playerInitialPoint.position, Quaternion.identity).GetComponent<PlayerCharacter>();
        playerCharacter.Init(player.currentHero.ranger);

        virtualCamera.Follow = playerCharacter.transform;
    }

    public void InvokeWave(Wave wave, List<Transform> spawnPoints, WaveZone waveZone)
    {
        foreach(Enemy enemy in wave.enemies)
        {
            EnemyController _enemy = Instantiate(enemy.characterModel, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity).GetComponent<EnemyController>();

            _enemy.Init(enemy);
            _enemy.player = playerCharacter;
            _enemy.OnEnemyDie += waveZone.RemoveEnemy;
        }
    }

    public void UpdateHUD(float health)
    {
        playerHealthBar.maxValue = player.currentHero.ranger.maxHealth;
        playerHealthBar.value = health;
    }

    public void OnPlayerDie()
    {
        SceneManager.Instance.ChangeScene("Game Over");
    }

    public void OnLevelFinished()
    {
        SceneManager.Instance.ChangeScene("Victory Screen");
    }
}
