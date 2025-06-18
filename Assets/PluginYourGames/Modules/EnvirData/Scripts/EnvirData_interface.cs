namespace YG
{
    using System;
    using UnityEngine;
#if UNITY_WEBGL
    using System.Runtime.InteropServices;
#endif

    public partial interface IPlatformsYG2
    {
#if PLATFORM_WEBGL
        [DllImport("__Internal")]
        private static extern IntPtr GeneralEnvirData_js();
#endif
        void InitEnirData()
        {
#if PLATFORM_WEBGL
            string jsonString = Marshal.PtrToStringUTF8(GeneralEnvirData_js());
            YG2.envir = JsonUtility.FromJson<YG2.EnvirData>(jsonString);
#else
            YG2.envir.language = "en";
            YG2.envir.browserLang = "en";

            if (Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld)
            {
                YG2.envir.deviceType = "mobile";
                YG2.envir.isMobile = true;
                YG2.envir.isDesktop = false;
            }
#endif
        }

        void GetEnvirData() { }
    }
}