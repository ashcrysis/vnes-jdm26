using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class TeamUI : MonoBehaviour
    {
        private RectTransform rect;
        private Image image;

        public RectTransform Rect => rect;
        public Image Image => image;
        
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
    }
}