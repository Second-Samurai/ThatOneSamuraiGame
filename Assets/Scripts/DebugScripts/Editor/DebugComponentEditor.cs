using UnityEditor;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.DebugScripts.Editor
{

    [CustomEditor(typeof(DebugComponent), editorForChildClasses: true)]
    public class DebugComponentEditor : UnityEditor.Editor
    {

        #region - - - - - - Methods - - - - - -

        public override void OnInspectorGUI()
        {
            GUI.backgroundColor = new Color(0.0f, 0.4f, 1.0f, 1.0f);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label($"<b><color=#000000>{target.GetType().Name}</color></b>", new GUIStyle
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            });
            EditorGUILayout.EndVertical();

            GUI.backgroundColor = Color.white;
            DrawDefaultInspector();
        }

        #endregion Methods
  
    }

}