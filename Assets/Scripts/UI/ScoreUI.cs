using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
        [SerializeField] private TeamUI teamUIPrefab;
        
        [SerializeField] private List<TeamData> teams; //fakedata
        private readonly List<TeamUI> _teamsUIs = new List<TeamUI>();
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
        
        public float MaxWidth => _parent.rect.width;

        private void Awake()
        {
            _parent = GetComponent<RectTransform>();
        }

        private void Start()
        {
            foreach (TeamData team in teams)
            {
                TeamUI ui = Instantiate<TeamUI>(teamUIPrefab, _parent);
                ui.Image.color = team.color;
                ui.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth / teams.Count);
                _teamsUIs.Add(ui);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _teamsUIs.Count; i++)
            {
                var ui = _teamsUIs[i];

                int score = TilePaintManager.Instance.GetScore(i);
                teams[i].score = score;

                float percentage = score / (float)Mathf.Max(1, TotalTilesPainted);
                ui.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth * percentage);
            }
        }
    }
}
