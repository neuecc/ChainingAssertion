using System.Windows;
using Microsoft.Silverlight.Testing;

namespace ChainingAssertion.SL
{
    public static class ApplicationExtensions
    {
        public static void StartTestRunnerDelayed(this Application application)
        {
            application.RootVisual = UnitTestSystem.CreateTestPage();
        }

        public static void StartTestRunnerImmediate(this Application application)
        {
            UnitTestSettings settings = UnitTestSystem.CreateDefaultSettings();
            settings.StartRunImmediately = true;
            application.RootVisual = UnitTestSystem.CreateTestPage(settings);
        }
    }
}
