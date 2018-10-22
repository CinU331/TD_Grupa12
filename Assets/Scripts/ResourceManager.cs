using UnityEngine;
using System.Collections;
public static class ResourceManager {
    public static int ScrollWidth { get { return 15; } }
    public static float ScrollSpeed { get { return 45; } }
    public static float ScrollZoomSpeed { get { return 1000; } }
    public static float RotateAmount { get { return 10; } }
    public static float RotateSpeed { get { return 100; } }
    public static float MinCameraHeight { get { return 45; } }
    public static float MaxCameraHeight { get { return 70; } }
    
    public static float MinCameraX { get { return 50; } }
    public static float MaxCameraX { get { return 250; } }
    public static float MinCameraZ { get { return 20; } }
    public static float MaxCameraZ { get { return 215; } }

    public static float MinCameraRotationX { get { return 50; } }
    public static float MaxCameraRotationX { get { return 250; } }
    public static float MinCameraRotationZ { get { return 20; } }
    public static float MaxCameraRotationZ { get { return 215; } }

    public static float KeyboardScrollSpeed { get { return 45; } }
}