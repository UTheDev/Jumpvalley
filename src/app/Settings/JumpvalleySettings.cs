using System;
using JumpvalleyApp.Settings.Display;

namespace JumpvalleyApp.Settings
{
    /// <summary>
    /// Main list of user settings for Jumpvalley
    /// </summary>
    public partial class JumpvalleySettings : IDisposable
    {
        public SettingGroup Group { get; private set; }
        public SettingsFile File { get; private set; }

        public JumpvalleySettings()
        {
            SettingGroup group = new SettingGroup();

            group.AddSettingGroup(new DisplaySettings());
            group.ShouldDisplayTitle = false;

            Group = group;

            File = new SettingsFile();
        }

        public void Dispose()
        {
            Group.Dispose();
        }
    }
}
