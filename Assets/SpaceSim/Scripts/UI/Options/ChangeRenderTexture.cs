using UnityEngine;
using UnityEngine.UI;


namespace SpaceSim.UI.Options
{
    public class ChangeRenderTexture : MonoBehaviour
    {
        [SerializeField]
        private RawImage raw;
        [SerializeField]
        private Camera cam;
        [SerializeField]
        private int rate;

        public int Rate {
            get => rate;
            set {
                rate = value;
                ChangeBitRate(value);
                Debug.Log("rate changed to " + value);
            }
        }

        private void Start() => ChangeBitRate(rate);

        public void ChangeBitRateOption(int index) {
            if (index == 0) Rate = 16;
            else if (index == 1) Rate = 32;
            else if (index == 2) Rate = 4;
            else if (index == 3) Rate = 64;
        }

        private void ChangeBitRate(int rate) {
            if (cam && cam.targetTexture != null) cam.targetTexture.Release();

            RenderTexture texture;

            texture = new RenderTexture(Screen.width, Screen.height, rate);

            if (cam) cam.targetTexture = texture;
            if (raw) raw.texture = texture;
        }
    }
}