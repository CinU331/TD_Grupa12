using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AbstractTrap : MonoBehaviour
    {
        public abstract int TrapId { get; set; }
        public abstract string Name { get; set; }
        public abstract int Cost { get; set; }
    }
}