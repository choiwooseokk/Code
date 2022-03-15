using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class SubFunction : MonoBehaviour
{
#if UNITY_EDITOR
    //  파일네임으로 가이드인지 파악해서 제거해주는 기능
    [MenuItem("Assets/SubFunctions/DeleteGuide", priority = 1, validate = false)]
    static void DelGuide()
    {
        var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        foreach (var asset in selectedAssets)
        {
            bool check = asset.name.Contains("0_Guide")||asset.name.Contains("0_guide");

            if (check)
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                File.Delete(assetPath);
            }
        }
    }

    //  [폴더명+파일명]으로 파일명을 변경합니다.일반적으로 아바타 리소스에 적용합니다.
    [MenuItem("Assets/SubFunctions/RenameByFolderNameForAvartar", priority = 2, validate = false)]
    static void RenameTextureByFolderName2()
    {
        var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

        foreach (var asset in selectedAssets)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string extension = Path.GetExtension(assetPath);

            string basePath = Path.GetDirectoryName(assetPath);
            var splits = basePath.Split('\\');
            string folderName = splits[splits.Length - 1];
            splits = folderName.Split('_');
            string folderNum = splits[splits.Length - 1];

            string assetName = folderNum + Path.GetFileNameWithoutExtension(assetPath) + extension;
            string newPath = Path.Combine(basePath, assetName);
            AssetDatabase.RenameAsset(assetPath, assetName);
        }
        AssetDatabase.SaveAssets();
    }
#endif
}