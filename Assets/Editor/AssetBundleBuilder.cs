using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;


public class AssetBundleBuilder
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        // create directory if not exist
        if (!Directory.Exists("Assets/AssetBundles/" + EditorUserBuildSettings.activeBuildTarget))
        {
            Directory.CreateDirectory("Assets/AssetBundles/" + EditorUserBuildSettings.activeBuildTarget);
        }

        // filter current platform asset bundles
        string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
        List<string> filted_assetBundles = new List<string>();
        foreach (string assetBundle in assetBundles)
        {
            if (assetBundle.Contains(".all") || assetBundle.Contains(EditorUserBuildSettings.activeBuildTarget.ToString()))
            {
                filted_assetBundles.Add(assetBundle);
            }
        }
        AssetBundleBuild[] buildMap = new AssetBundleBuild[filted_assetBundles.Count];

        for (int i = 0; i < filted_assetBundles.Count; i++)
        {
            buildMap[i].assetBundleName = filted_assetBundles[i];
            buildMap[i].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(filted_assetBundles[i]);
        }

        // build asset bundles
        BuildPipeline.BuildAssetBundles(
            "Assets/AssetBundles/" + EditorUserBuildSettings.activeBuildTarget,
            buildMap,
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget
        );

        // update version.json and push to git
        UpdateVersionJson();
        CommitAndPushToGit();
    }


    private static void UpdateVersionJson()
    {
        // local version.json
        string versionFilePath = "Assets/AssetBundles/" + EditorUserBuildSettings.activeBuildTarget + "/version.json";
        if (!File.Exists(versionFilePath))
        {
            File.Create(versionFilePath).Close();
            File.WriteAllText(versionFilePath, JsonUtility.ToJson(new VersionConfig()));
        }
        string json = File.ReadAllText(versionFilePath);
        VersionConfig versionData = JsonUtility.FromJson<VersionConfig>(json) ?? new VersionConfig();

        // filter current platform asset bundles
        string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
        List<string> filted_assetBundles = new List<string>();
        foreach (string assetBundle in assetBundles)
        {
            if (assetBundle.Contains(".all") || assetBundle.Contains(EditorUserBuildSettings.activeBuildTarget.ToString()))
            {
                filted_assetBundles.Add(assetBundle);
            }
        }

        // Add new bundle to versionData.bundles
        if (versionData.bundles == null)
        {
            versionData.bundles = new List<VersionConfig.AssetBundleInfo>();
        }
        foreach (var bundle in filted_assetBundles)
        {
            if (versionData.bundles.Find(b => b.name == bundle) == null)
            {
                versionData.bundles.Add(new VersionConfig.AssetBundleInfo
                {
                    name = bundle,
                    version = "1.0",
                    url = "https://raw.githubusercontent.com/IceWaterNotIce/WordCurse/main/Assets/AssetBundles/" + EditorUserBuildSettings.activeBuildTarget + "/" + bundle
                });
            }
        }

        // Remove deleted bundle from versionData.bundles
        foreach (var bundle in versionData.bundles)
        {
            if (filted_assetBundles.Find(b => b == bundle.name) == null)
            {
                versionData.bundles.Remove(bundle);
            }
        }

        // Update version number
        foreach (var bundle in versionData.bundles)
        {
            //version format is 1.0
            bundle.version = (float.Parse(bundle.version) + 0.1f).ToString();
        }



        File.WriteAllText(versionFilePath, JsonUtility.ToJson(versionData));
        UnityEngine.Debug.Log("Version.json updated");

    }


    private static void CommitAndPushToGit()
    {
        RunGitCommand("git add .");
        RunGitCommand("git commit -m \"Auto commit from Unity Asset Bundle Builder\"");
        RunGitCommand("git push origin main");

        UnityEngine.Debug.Log("Git commit and push done");
    }

    private static void RunGitCommand(string command)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        processStartInfo.WorkingDirectory = Application.dataPath;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = false;

        using (Process process = Process.Start(processStartInfo))
        {
            process.WaitForExit();
            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
        }
    }


}
