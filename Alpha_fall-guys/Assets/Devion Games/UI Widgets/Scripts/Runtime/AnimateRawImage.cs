using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.UIWidgets
{
    public class AnimateRawImage : MonoBehaviour
    {
        public Vector2 animRate = new Vector2(1f, 0f);
        private RawImage image;
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            Rect rect = image.uvRect;
            rect.x += animRate.x * Time.deltaTime;
            rect.y += animRate.y * Time.deltaTime;
            image.uvRect = rect;
        }
    }
}