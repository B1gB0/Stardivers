﻿using UnityEditor;
using UnityEngine;

namespace YG.Insides
{
    [CreateAssetMenu(fileName = "NewPlatformYG", menuName = "YG2/New Platform")]
    public partial class PlatformSettings : ScriptableObject
    {
        public string nameFull = "Error";
        public static string currentPlatformFullName
        {
            get
            {
                if (InfoYG.instance && InfoYG.instance.Basic.platform)
                    return InfoYG.instance.Basic.platform.nameFull;
                return "NullPlatform";
            }
        }
        public static string currentPlatformBaseName
        {
            get
            {
                if (InfoYG.instance && InfoYG.instance.Basic.platform)
                    return InfoYG.instance.Basic.platform.nameFull.Replace("Platform", string.Empty);
                return "Null";
            }
        }

        public ProjectSettings projectSettings = new ProjectSettings();

        public string NameBase()
        {
            return nameFull.Replace("Platform", string.Empty);
        }

#if UNITY_EDITOR
        public void ApplyProjectSettings()
        {
            projectSettings.ApplySettings();

            CallAction.CallIByAttribute(typeof(ApplySettingsAttribute), typeof(PlatformInfo), YG2.infoYG.platformInfo);
            CallAction.CallIByAttribute(typeof(ApplySettingsAttribute), typeof(CommonOptions), YG2.infoYG.common);

            if (YG2.infoYG.platformToggles.selectWebGLTemplate && projectSettings.selectWebGLTemplate)
            {
                string templateName = currentPlatformBaseName;
                string templatePath = $"Assets/WebGLTemplates/{templateName}";

                if (AssetDatabase.IsValidFolder(templatePath))
                    PlayerSettings.WebGL.template = "PROJECT:" + templateName;
            }
        }

        public static void SelectPlatform()
        {
            CallAction.CallIByAttribute(typeof(SelectPlatformAttribute), typeof(PlatformInfo), YG2.infoYG.platformInfo);
            CallAction.CallIByAttribute(typeof(SelectPlatformAttribute), typeof(CommonOptions), YG2.infoYG.common);
        }
        public static void DeletePlatform()
        {
            CallAction.CallIByAttribute(typeof(DeletePlatformAttribute), typeof(PlatformInfo), YG2.infoYG.platformInfo);
            CallAction.CallIByAttribute(typeof(DeletePlatformAttribute), typeof(CommonOptions), YG2.infoYG.common);
        }
#endif
    }
}