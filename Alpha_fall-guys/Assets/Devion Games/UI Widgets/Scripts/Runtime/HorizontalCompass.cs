using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class HorizontalCompass : MonoBehaviour
    {
        public RawImage image;

        void Start()
        {

        }

        void Update()
        {
            image.uvRect = new Rect(Camera.main.transform.localEulerAngles.y / 360f, 0f, 1f, 1f);
        }
    }
}