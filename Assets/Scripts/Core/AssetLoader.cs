using UnityEditor;
using UnityEngine;

namespace Core
{
    public class AssetLoader
    {
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            string editorPath = "Assets/" + path;
            T asset = AssetDatabase.LoadAssetAtPath<T>(editorPath);
            return asset;
#endif
            return null;
        }

        public static void UnloadAsset(Object asset)
        {
            Resources.UnloadAsset(asset);
        }

        public static void DestroyGameObjectAsset(GameObject go, bool unloadUnused = false)
        {
            GameObject.DestroyImmediate(go, true);
            go = null;
            if (unloadUnused)
            {
                Resources.UnloadUnusedAssets();
            }
        }
    }
}
