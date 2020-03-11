#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class BuildPostProcessor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.iOS)
        {
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            string targetName = PBXProject.GetUnityTargetName();
            string targetGUID = project.TargetGuidByName(targetName);

            // Add `-ObjC` to "Other Linker Flags".
            project.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC");

            if (GameSettings.Instance.General.LinkerAll)
            {
                project.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-all_load");
            }

            // Add frameworks
            project.AddFrameworkToProject(targetGUID, "AdSupport.framework", false);
            project.AddFrameworkToProject(targetGUID, "CoreTelephony.framework", false);
            project.AddFrameworkToProject(targetGUID, "StoreKit.framework", false);
            project.AddFrameworkToProject(targetGUID, "WebKit.framework", false);
            project.AddFrameworkToProject(targetGUID, "CoreData.framework", false);
            project.AddFrameworkToProject(targetGUID, "SystemConfiguration.framework", false);

            // Write
            File.WriteAllText(projectPath, project.WriteToString());

            // Read plist
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Update value
            PlistElementDict newDict = plist.root.CreateDict("NSAppTransportSecurity");
            newDict.SetBoolean("NSAllowsArbitraryLoads", true); //https://developers.ironsrc.com/ironsource-mobile/unity/unity-plugin/#step-3
            newDict.SetBoolean("ITSAppUsesNonExemptEncryption", false); //Fix for testflight "Missing Compliance", https://stackoverflow.com/questions/35841117/missing-compliance-in-status-when-i-add-built-for-internal-testing-in-test-fligh
            //newDict.SetBoolean("NSAllowsArbitraryLoadsInWebContent", true);
            //newDict.SetBoolean("NSAllowsArbitraryLoadsForMedia", true);

            if (plist.root["NSCalendarsUsageDescription"] == null)
                plist.root["NSCalendarsUsageDescription"] = new PlistElementString("Advertisement would like to create a calendar event.");

            if (plist.root["GADApplicationIdentifier"] == null)
                plist.root["GADApplicationIdentifier"] = new PlistElementString(GameSettings.Instance.AdsMediation.AdMobAppIdIos);

            if (plist.root["UIApplicationExitsOnSuspend"] != null)
                plist.root.values.Remove("UIApplicationExitsOnSuspend");
            
            // Write plist
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
#endif