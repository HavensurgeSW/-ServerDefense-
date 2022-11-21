using System.Collections.Generic;

public static class CommonUtils
{
    // CHANGE ALL OF THIS WHEN CHANGING UNITY BUILD SETTINGS
    public enum SCENE
    {
        SPLASH_SCREEN = 0,
        MAIN_MENU = 1,
        TOWER_DEFENSE = 2,
        TUTORIAL = 3
    }

    public static readonly Dictionary<string, SCENE> scenesEnumDictionary = new Dictionary<string, SCENE>
    {
        { "SplashScreen", SCENE.SPLASH_SCREEN },
        { "Menu", SCENE.MAIN_MENU },
        { "Game", SCENE.TOWER_DEFENSE },
        { "Tutorial", SCENE.TUTORIAL }
    };

    public static readonly Dictionary<SCENE, string> scenesStringDictionary = new Dictionary<SCENE, string>
    {
        { SCENE.SPLASH_SCREEN, "SplashScreen" },
        { SCENE.MAIN_MENU, "Menu" },
        { SCENE.TOWER_DEFENSE, "Game" },
        { SCENE.TUTORIAL, "Tutorial" }
    };
}
