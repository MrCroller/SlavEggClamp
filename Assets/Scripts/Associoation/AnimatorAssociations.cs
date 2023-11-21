using SEC.Enums;

namespace SEC.Associations
{
    public static class AnimatorAssociations
    {
        public const string Jump       = "onJumping";
        public const string isEggTake  = "isEggTake";
        public const string isGrounded = "isGrounded";
        public const string Dead       = "onDead";
        public const string Alive      = "onAlive";
        public const string xVelocity  = "xVelocity";
        public const string yVelocity  = "yVelocity";
        public const string Bump       = "onBump";
        public const string Kick       = "onKick";
        public const string GoLeft     = "GoLeft";
        public const string GoRight    = "GoRight";
        public const string EndAnim    = "EndAnimation";

        public static string GetSideHelpArrow(OrientationLR side) => side switch
        {
            OrientationLR.Right => GoLeft,
            OrientationLR.Left => GoRight,
            _ => throw new System.ArgumentException()
        };
    }
}
