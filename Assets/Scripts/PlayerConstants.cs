namespace DefaultNamespace
{
    public static class PlayerConstants
    {
        // Linear mapping constants for player attributes
        public const float SpeedMultiplier = 0.8f;
        public const float SpeedOffset = 2f;

        public const float JumpForceMultiplier = 1.2f;
        public const float JumpForceOffset = 4f;

        public const float AttackSpeedMultiplier = 0.4f;
        public const float AttackSpeedOffset = 1f;

        // Utility function to calculate the actual values
        public static float CalculateSpeed(float rawSpeed)
        {
            return rawSpeed * SpeedMultiplier + SpeedOffset;
        }

        public static float CalculateJumpForce(float rawJumpForce)
        {
            return rawJumpForce * JumpForceMultiplier + JumpForceOffset;
        }

        public static float CalculateAttackSpeed(float rawAttackSpeed)
        {
            return rawAttackSpeed * AttackSpeedMultiplier + AttackSpeedOffset;
        }
    }

}