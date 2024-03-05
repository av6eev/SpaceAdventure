using UnityEditor;
using UnityEngine;

namespace Space.Preview.Editor
{
    [CustomEditor(typeof(SpaceMapPreviewGenerator))]
    public class SpaceMapPreviewGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var previewGenerator = (SpaceMapPreviewGenerator)target;

            if (DrawDefaultInspector())
            {
                if (previewGenerator.IsAutoUpdate)
                {
                    previewGenerator.GenerateMap();
                }
            }

            if (GUILayout.Button("Generate new"))
            {
                previewGenerator.GenerateMap();
            }
            
            if (GUILayout.Button("Save to file"))
            {
                if (previewGenerator.SpaceMapGraph.NodesByCenterPosition.Count == 0)
                {
                    return;
                }

                previewGenerator.SaveMap();
            }
            
            if (GUILayout.Button("Load last save"))
            {
                previewGenerator.LoadMapFromFile();
            }
        }
    }
}