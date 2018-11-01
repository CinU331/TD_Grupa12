using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    class TextWaves :MonoBehaviour
    {
        public TextMeshProUGUI wavesCount;
        // Use this for initialization
        void Start()
        {
            wavesCount = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            wavesCount.text = WaveSpawner.wave.ToString() + "/" + WaveSpawner.numberOfWaves.ToString();
        }
    }
}