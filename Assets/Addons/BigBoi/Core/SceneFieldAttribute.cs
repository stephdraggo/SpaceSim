using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace BigBoi
{
    [Serializable]
    public class SceneFieldAttribute : PropertyAttribute
    {
        /// <summary>
        /// Converts a full path to a scene manager friendly path for loading scene
        /// </summary>
        /// <param name="_path">original path Assets/.unity</param>
        /// <returns>friendlier path</returns>
        public static string LoadableName(string _path)
        {
            //remove these from string
            string start = "Assets/";
            string end = ".unity";

            //figure out if path has start and or end and remove them if present
            if (_path.StartsWith(start))
            {
                _path = _path.Substring(start.Length);
            }

            if (_path.EndsWith(end))
            {
                _path = _path.Substring(0, _path.LastIndexOf(end));
            }

            //return modified string
            return _path;
        }
    }

    #region editor class

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SceneFieldAttribute))]
    public class SceneFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, _label, _property);

            //load current scene
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(_property.stringValue);

            //check if change in inspector
            EditorGUI.BeginChangeCheck();

            //draw scene field as object field as scene asset
            var newScene = EditorGUI.ObjectField(_position, _label, oldScene, typeof(SceneAsset), false) as SceneAsset;

            //did change??
            if (EditorGUI.EndChangeCheck())
            {
                //sure did, I guess
                //set string to path of scene
                string path = AssetDatabase.GetAssetPath(newScene);
                _property.stringValue = path;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) =>
            EditorGUIUtility.singleLineHeight;
    }
#endif

    #endregion
}