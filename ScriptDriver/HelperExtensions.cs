using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ScriptDriver
{
    public static class HelperExtensions
    {
        public static T GetChildOfType<T>(this DependencyObject depObj, string typeName = null)
                                                where T : DependencyObject
        {
            T toReturn = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                bool typeNameMatch = child.IsTypeNameMatch<T>(typeName);
                if (typeNameMatch)
                {
                    toReturn = child as T;
                    break;
                }
                else
                    toReturn = GetChildOfType<T>(child, typeName);

                if (toReturn != null)
                    break;
            }
            return toReturn;
        }

        private static bool IsTypeNameMatch<T>(this DependencyObject child, string typeName) where T : DependencyObject
        {
            return (child is T) && (string.IsNullOrEmpty(typeName) || (child.GetType().Name.Equals(typeName)));
        }

        private static bool IsPathNameMatch<T>(this DependencyObject child, DependencyProperty property, string pathName) where T : DependencyObject
        {
            bool? match = true;
            Debug.WriteLine(BindingOperations.GetBinding(child, property)?.Path?.Path);
            if (!string.IsNullOrEmpty(pathName))
                match = BindingOperations.GetBinding(child, property)?.Path?.Path.Equals(pathName);
            return match == true;
        }

        public static T GetChildWithPath<T>(this DependencyObject depObj, DependencyProperty property = null, string pathName = null) where T : DependencyObject
        {
            T toReturn = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                bool pathNameMatch = (child is T) && child.IsPathNameMatch<T>(property, pathName);
                if (pathNameMatch)
                {
                    toReturn = child as T;
                    break;
                }
                else
                    toReturn = GetChildWithPath<T>(child, property, pathName);

                if (toReturn != null)
                    break;
            }
            return toReturn;
        }

        public static void SimulateClick(this CheckBox chkBox)
        {
            var peer = new CheckBoxAutomationPeer(chkBox);
            var provider = peer.GetPattern(PatternInterface.Toggle) as IToggleProvider;
            provider.Toggle();
        }
    }
}
