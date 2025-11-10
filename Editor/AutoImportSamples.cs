using System.IO;
using UnityEditor;
using UnityEngine;

namespace AengelStudio.PlacementSystem.Editor
{
    [InitializeOnLoad]
    public static class AutoImportSamples
    {
        private const string SamplesSourcePath = "Packages/com.aengelstudio.placement-system/Samples~/Example";
        private const string SamplesDestPath = "Assets/PlacementSystem";
        private const string ImportedFlagFile = "Assets/PlacementSystem/.imported";

        static AutoImportSamples()
        {
            // Check if samples have already been imported
            if (File.Exists(ImportedFlagFile))
                return;

            // Wait for asset database to be ready
            EditorApplication.delayCall += ImportSamples;
        }

        private static void ImportSamples()
        {
            string sourcePath = SamplesSourcePath;
            string destPath = SamplesDestPath;

            // Check if source exists
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogWarning($"[PlacementSystem] Samples not found at {sourcePath}");
                return;
            }

            // Create destination directory
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Copy all files from source to destination using Unity's FileUtil
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destPath, fileName);

                if (!File.Exists(destFile))
                {
                    // Use Unity's FileUtil which handles .meta files automatically
                    FileUtil.CopyFileOrDirectory(file, destFile);
                    Debug.Log($"[PlacementSystem] Imported {fileName} to {destPath}");
                }
            }

            // Create flag file to prevent re-importing
            File.WriteAllText(ImportedFlagFile, "Samples imported automatically");

            // Refresh asset database
            AssetDatabase.Refresh();

            Debug.Log($"[PlacementSystem] Samples automatically imported to {destPath}");
        }
    }
}

