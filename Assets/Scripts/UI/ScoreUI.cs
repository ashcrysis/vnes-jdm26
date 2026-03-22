using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace UI
{
    [Serializable]
    public class TeamData {
        public Color color;
        public int score;
    }
    
    [RequireComponent(typeof(RectTransform))]
    public class ScoreUI : MonoBehaviour
    {
        private RectTransform _parent;
        [SerializeField] private GameObject teamUIPrefab;
        private float _maxWidth;
        
        //fakedata
        [SerializeField] private List<TeamData> teams;
        
        public int TotalTilesPainted
        {
            get
            {
                int value = 0;
                foreach (TeamData teamData in teams)
                {
                    value += teamData.score;
                }
                return value;
            }
        }

        private void Awake()
        {
            _parent = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _maxWidth = _parent.rect.width;
        }

        private void Update()
        {
            
        }
    }
}
