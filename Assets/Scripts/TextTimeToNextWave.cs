using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    class TextTimeToNextWave : MonoBehaviour
    {
        public TextMeshProUGUI time;
        void Start()
        {
            time= GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            time.text = WaveSpawner.timeToNextWave.ToString();
        }
    }
}
