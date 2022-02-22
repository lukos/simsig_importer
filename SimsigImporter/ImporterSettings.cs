using System;
using System.Configuration;

namespace SimsigImporter
{
    public class ImporterSettings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        public string SelectedSim
        {
            get { return (String)this["SelectedSim"]; }
            set { this["SelectedSim"] = value; }
        }
    }
}
