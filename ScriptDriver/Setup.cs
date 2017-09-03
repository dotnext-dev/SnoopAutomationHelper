using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ScriptDriver
{
    public class Setup
    {
        public static bool Start()
        {
            Dispatcher dispatcher;
            if (Application.Current == null)
                dispatcher = Dispatcher.CurrentDispatcher;
            else
                dispatcher = Application.Current.Dispatcher;
            
            dispatcher.Invoke(AutomateApp);
            return true;
        }

        public static void AutomateApp()
        {
            Window root = null;
            foreach (PresentationSource presentationSource in PresentationSource.CurrentSources)
            {
                root = presentationSource.RootVisual as Window;

                if (root == null)
                    continue;

                if ("NordVPN ".Equals(root.Title))
                    break;
            }

            if(root == null)
            {
                Debug.WriteLine("Unable to locate root window.");
                return;
            }

#if DEBUG
            //uncomment if you want to debug automation
            //if(!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif
            
            var settingsView = root.GetChildOfType<UserControl>("SettingsView");
            if (settingsView == null)
            {
                Debug.WriteLine("Unable to locate settings window.");
                return;
            }

            var checkBoxNames = new[]
            {
                "CyberSec", "AutomaticUpdates", "AutoConnect",
                "StartOnStartup", "KillSwitch", "ShowNotifications",
                "StartMinimized", "ShowServerList", "ShowMap",
                "UseCustomDns", "ObfuscatedServersOnly"
            };

            foreach(var path in checkBoxNames)
            {
                var chkBox = settingsView.GetChildWithPath<CheckBox>(CheckBox.IsCheckedProperty, path);
                if(chkBox != null && chkBox.IsEnabled)
                    chkBox.SimulateClick();
            }
        }
    }
}
