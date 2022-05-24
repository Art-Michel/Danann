public class StateNames
{
    public static string[] e=new string[]
    {
        P1C_DASH,P1C_MIXDASH,P1C_SLAM,P1C_TELEPORTATION,P1D_BOOMERANG,P1D_SHOOT,P1D_SPIN,
        P2C_DASH,P2C_MIXDASH,P2C_SLAM,P2C_TELEPORTATION,P2D_BOOMERANG,P2D_SHOOT,P2D_SPIN
    };
    #region  p1
    public const string P1IDLE = "P1Idle";
    public const string P1D_SHOOT = "P1DShoot";
    public const string P1D_BOOMERANG = "P1DBoomerang";
    public const string P1D_SPIN = "P1DSpin";
    public const string P1C_DASH = "P1CDash";
    public const string P1C_MIXDASH = "P1CMixDash";
    public const string P1C_SLAM = "P1CSlam";
    public const string P1C_TELEPORTATION = "P1CTeleportation";
    public const string P1R_SPIRALE = "P1RSPIRALE";
    #endregion

    #region  p2
    public const string P2IDLE = "P2Idle";
    public const string P2D_SHOOT = "P2DShoot";
    public const string P2D_BOOMERANG = "P2DBoomerang";
    public const string P2D_SPIN = "P2DSpin";
    public const string P2C_DASH = "P2CDash";
    public const string P2C_MIXDASH = "P2CMixDash";
    public const string P2C_SLAM = "P2CSlam";
    public const string P2C_TELEPORTATION = "P2CTeleportation";
    public const string P2R_SPIRALE = "P2RSPIRALE";
    #endregion
    
    #region  global
    public const string P1GSHOOT = "P1gshoot";
    public const string P1GDASH = "P1gdash";
    public const string P1GTELEPORTATION ="P1gteleportation";
    public const string P1GBOOMERANG="P1gboomerang";
    #endregion
}
