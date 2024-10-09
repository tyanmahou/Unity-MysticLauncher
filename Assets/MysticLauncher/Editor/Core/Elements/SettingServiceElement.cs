using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class SettingServiceElement : ActionElement<OpenSettingServiceAction>
    {
        public static SettingServiceElement Create(string text, SettingsScope scope, string path, string tooltip = "", string icon = null)
        {
            return new SettingServiceElement()
            {
                Label = Label.Create(text, tooltip, icon),
                _action = new OpenSettingServiceAction()
                {
                    Path = SettingServicePath.Create(scope, path)
                }
            };
        }
        public static SettingServiceElement CreateProject(string text, string path, string tooltip = "", string icon = null)
            => Create(text, SettingsScope.Project, path, tooltip, icon);
        public static SettingServiceElement CreateUser(string text, string path, string tooltip = "", string icon = null) 
            => Create(text, SettingsScope.User, path, tooltip, icon);
    }
}
