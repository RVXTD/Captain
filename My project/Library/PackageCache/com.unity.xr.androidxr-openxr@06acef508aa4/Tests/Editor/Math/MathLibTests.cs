#if UNITY_EDITOR && UNITY_ANDROID
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace UnityEditor.XR.OpenXR.Features.Android.Tests.Math
{
    /// <summary>
    /// Class containing methods that triggers math tests on native C++ Android XR plugin
    /// </summary>
    [TestFixture]
    public class MathLibTests
    {
        [DllImport("libUnityARFoundationAndroidXR")]
        static extern bool UnityOpenXRAndroid_RunAllMathUnitTests(StringBuilder outBuffer, int bufferLen);

        /// <summary>
        /// Method that triggers a unit test runner inside of Android XR plugin
        /// </summary>
        [Test]
        public void RunAllMathUnitTests()
        {
            var sb = new StringBuilder(256);
            var success = UnityOpenXRAndroid_RunAllMathUnitTests(sb, sb.Capacity);
            Assert.IsTrue(success, $"Native test failed: {sb}");
        }
    }
}
#endif