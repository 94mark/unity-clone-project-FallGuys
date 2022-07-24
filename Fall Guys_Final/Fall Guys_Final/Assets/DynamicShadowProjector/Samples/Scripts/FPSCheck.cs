using UnityEngine;

namespace DynamicShadowProjector.Sample
{
    public class FPSCheck : MonoBehaviour
    {
        int frameCount = -1;
        float startTime = 0;
        float fps = 0;
        public UnityEngine.UI.Text text;
        private void Update()
        {
            if (frameCount == -1)
            {
                startTime = Time.realtimeSinceStartup;
            }
            if (++frameCount == 100)
            {
                fps = 100.0f / (Time.realtimeSinceStartup - startTime);
                frameCount = -1;
                int n = Mathf.FloorToInt(100 * fps);
                text.text = "FPS: " + Mathf.FloorToInt(n / 100) + "." + (n % 100);
                text.SetAllDirty();
            }
        }
    }
}
