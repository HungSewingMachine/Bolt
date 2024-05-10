namespace Main.Scripts
{
    public static class GameConstant
    {
        public const float X = 0.5f;       // Kc tam -> edge 
        public const float Y = 0.4f;       // Chieu day
        public const float Z = -0.58f / 2; // kc tam -> vertex /2

        public const float BOX_MOVE_DURATION      = 1f;
        public const float DISTANCE_BETWEEN_BOX = 5;
        public const int   BOX_CAPACITY         = 3;
        public const int   ENTITY_LAYER         = 1 << 7;
    }
}