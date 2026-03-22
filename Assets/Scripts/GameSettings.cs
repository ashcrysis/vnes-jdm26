using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance; 
    [SerializeField] private List<Color> colors = new List<Color>(){ Color.red, Color.blue, Color.green, Color.yellow };
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>(){ };
    
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
        
        Instance = this;
    }

    public Color GetTeamColor(int index)
    {
        return colors[index];
    }
    
    public Transform GetSpawnPoint(int index)
    {
        return spawnPoints[Math.Clamp(index, 0, spawnPoints.Count - 1)];
    }
}
