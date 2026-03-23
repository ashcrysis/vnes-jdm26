using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance; 
    [SerializeField] private List<Color> colors = new(){ Color.red, Color.blue, Color.green, Color.yellow };
    private readonly List<Transform> _spawnPoints = new(){ };
    
    private PlayerInputManager _manager;

    public int GetMaxPlayers => _manager.maxPlayerCount;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        _manager = GetComponent<PlayerInputManager>();
        
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Spawnpoint");
        _spawnPoints.AddRange(objects.Select((a) => a.transform));

        if (_spawnPoints.Count == 0)
        {
            Debug.LogError("No Spawnpoints found!");
        }
        
        Instance = this;
    }

    public Color GetTeamColor(int index)
    {
        return colors[index];
    }
    
    public Transform GetSpawnPoint(int index)
    {
        return _spawnPoints[Math.Clamp(index, 0, _spawnPoints.Count - 1)];
    }
}
