﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PD3100sifra43.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=10.0.9.4;Initial Catalog=PioRS;Persist Security Info=True;User ID=sa;" +
            "Password=$idi0us;")]
        public string Konekcija {
            get {
                return ((string)(this["Konekcija"]));
            }
            set {
                this["Konekcija"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=radenko-pc\\MSSQL2008;Initial Catalog=PioRS;Persist Security Info=True" +
            ";User ID=sa;Password=pegla80;")]
        public string PioRSConnectionString {
            get {
                return ((string)(this["PioRSConnectionString"]));
            }
            set {
                this["PioRSConnectionString"] = value;
            }
        }
    }
}