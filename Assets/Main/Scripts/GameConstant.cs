namespace Main.Scripts
{
    public static class GameConstant
    {
        // hexagon config
        public const float X = 0.5f;       // Kc tam -> edge 
        public const float Y = 0.2f;       // Chieu day
        public const float Y_BUILD = 0.4f; // Chieu day khi build vi thuc te co 2 tam
        public const float Z = -0.58f / 2; // kc tam -> vertex /2
        
        // Layer config
        public const int   ENTITY_LAYER         = 1 << 7;
        
        // Box config
        public const float BOX_MOVE_DURATION    = 0.2f;
        public const float BOX_START_POSITION   = 4f;
        public const float BOX_DELAY_TIME       = 2f;
        public const float DISTANCE_BETWEEN_BOX = 4;
        public const int   BOX_CAPACITY         = 3;
        public const float BOX_Y_POSITION       = 0f;
        public const float BOX_Z_POSITION       = 10f;

        public const float SLOT_OFFSET_HEIGHT = 1f;
    }
}