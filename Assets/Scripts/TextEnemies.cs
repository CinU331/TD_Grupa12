using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class TextEnemies : MonoBehaviour
    {

        public TextMeshProUGUI enemiesCount;
        // Use this for initialization
        void Start()
        {
            enemiesCount = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            enemiesCount.text = WaveSpawner.aliveEnemies.ToString();
        }
    }
}
