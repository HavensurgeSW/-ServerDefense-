#if UNITY_EDITOR
using UnityEngine;

using UnityEditor;

namespace ServerDefense.Gameplay.Gameplay.Modules.Map
{
    public class TileMapSaver : EditorWindow
    {
        private Grid tilemapsGrid = null;
        private MapSO mapToLoad = null;
        private string saveLocation = string.Empty;
        private float spaceBetweenParts = 12.0f;

        [MenuItem("ServerDefense/Tilemap Saver")]
        public static void ShowWindow()
        {
            GetWindow<TileMapSaver>("Tilemap Saver");
        }

        private void OnGUI()
        {
            DrawGridSelectionWindow();

            if (!IsGridConfigured())
            {
                return;
            }

            GUILayout.Space(spaceBetweenParts);

            GUILayout.BeginHorizontal();

            DrawLoadGridWindow();

            GUILayout.Space(spaceBetweenParts);

            DrawSaveAssetWindow();

            GUILayout.EndHorizontal();
        }

        private void DrawGridSelectionWindow()
        {
            GUILayout.Label("Select a Grid:", EditorStyles.boldLabel);

            tilemapsGrid = (Grid)EditorGUILayout.ObjectField(tilemapsGrid, typeof(Grid), true);
        }

        private void DrawSaveAssetWindow()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Select a save location:", EditorStyles.boldLabel);

            if (GUILayout.Button("Open save folder", GUILayout.ExpandWidth(false)))
            {
                string selectedSaveLocation = EditorUtility.OpenFolderPanel("Select Save Location", Application.dataPath, string.Empty);
                bool validPath = !string.IsNullOrEmpty(selectedSaveLocation);
                saveLocation = validPath ? selectedSaveLocation : string.Empty;
            }

            if (!string.IsNullOrEmpty(saveLocation))
            {
                GUILayout.Label("Save location:", GUILayout.ExpandWidth(false));
                GUILayout.Label(saveLocation, GUILayout.ExpandWidth(false));
            }
            else
            {
                DrawColoredText("No Save location selected", false);
            }

            GUILayout.Space(spaceBetweenParts);

            GUI.enabled = IsValidToSave();
            if (GUILayout.Button("Save asset", GUILayout.ExpandWidth(false)))
            {
                MapSO so = ScriptableObject.CreateInstance<MapSO>();
                //so.Configure
                AssetDatabase.CreateAsset(so, saveLocation);
            }
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        private void DrawLoadGridWindow()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Select a Map to load:", EditorStyles.boldLabel);
            mapToLoad = (MapSO)EditorGUILayout.ObjectField(mapToLoad, typeof(MapSO), true, GUILayout.ExpandWidth(false));

            GUI.enabled = IsValidToLoad();
            if (GUILayout.Button("Load Map", GUILayout.ExpandWidth(false)))
            {
                MapSO so = ScriptableObject.CreateInstance<MapSO>();
            }
            GUI.enabled = true;

            GUILayout.EndVertical();
        }

        private void DrawColoredText(string text, bool active)
        {
            GUI.color = active ? Color.green : Color.red;
            GUILayout.Label(text, GUILayout.ExpandWidth(false));
            GUI.color = Color.white;
        }

        private bool IsGridConfigured()
        {
            return tilemapsGrid != null;
        }

        private bool IsSaveLocationConfigured()
        {
            return !string.IsNullOrEmpty(saveLocation);
        }

        private bool IsValidToSave()
        {
            return IsGridConfigured() && IsSaveLocationConfigured();
        }

        private bool IsValidToLoad()
        {
            return IsGridConfigured() && IsMapToLoadConfigured();
        }

        private bool IsMapToLoadConfigured()
        {
            return mapToLoad != null;
        }
    }
}
#endif