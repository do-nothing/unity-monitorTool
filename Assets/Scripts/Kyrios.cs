using UnityEngine;

namespace Kyrios {

    public enum DeviceType {
        Normal = 0,
        Kinect = 1,
        Touch = 2,
        LeapMotion = 3
    }

    public enum NetworkStatus{
        green = 0,
        yellow = 1,
        red = 2,
    }

    public static class BgColor{
        public static Color[] bgColor = new Color[]{
            new Color(0.1765f,0.6824f,0.3255f,0.7412f),
            new Color(0.6824f,0.1765f,0.1765f,0.7412f),
            new Color(0.3490f,0.1765f,0.3255f,0.7843f),
            new Color(0.9567f,0.5333f,0,0.7412f),
            //new Color(0.1765f,0.6824f,0.3255f,0.7412f),
            //new Color(0.1765f,0.6824f,0.3255f,0.7412f),
        };
    }

    public class AppInfo {
        public string name;
        public string version;
        public AppInfo(string name, string version) {
            this.name = name;
            this.version = version;
        }
    }
}
