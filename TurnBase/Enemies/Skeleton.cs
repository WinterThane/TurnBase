namespace TurnBase.Enemies
{
    class Skeleton : Actor
    {
        private string name = "";

        private int level = 1;
        private int experience = 0;

        private int health = 200;
        private int mana = 0;

        private int strength = 55;
        private int dexterity = 40;
        private int intelligence = 20;
        private int initination = 50;

        private float minMeleeDmg = 1f;
        private float maxMeleeDmg = 2f;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Level
        {
            get { return level; }
            set { value = level; }
        }

        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        public int Dexterity
        {
            get { return dexterity; }
            set { dexterity = value; }
        }

        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }

        public int Initination
        {
            get { return initination; }
            set { initination = value; }
        }

        public float MinMeleeDamage
        {
            get { return minMeleeDmg; }
            set { value = minMeleeDmg; }
        }

        public float MaxMeleeDamage
        {
            get { return maxMeleeDmg; }
            set { value = maxMeleeDmg; }
        }

        public string Damage(float min, float max, int str)
        {
            float newMax = min + 1f;
            return "Physical damage: " + min.ToString() + " - " + (newMax * str / 100).ToString();
        }
    }
}
