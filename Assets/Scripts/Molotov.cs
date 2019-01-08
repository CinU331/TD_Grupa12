namespace Assets.Scripts
{
    class Molotov : AbstractTrap
    {
        private Molotov()
        {
            TrapId = 2;
            Name = "Molotov";
            Cost = 10;
        }

        public override int TrapId { get; set; }
        public override string Name { get; set; }
        public override int Cost { get; set; }
    }
}
