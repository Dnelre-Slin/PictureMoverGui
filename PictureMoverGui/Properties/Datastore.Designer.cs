﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PictureMoverGui.Helpers;

namespace PictureMoverGui.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Datastore : global::System.Configuration.ApplicationSettingsBase {
        
        private static Datastore defaultInstance = ((Datastore)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Datastore())));
        
        public static Datastore Default {
            get {
                return defaultInstance;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsSerializeAs(global::System.Configuration.SettingsSerializeAs.Binary)]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AAEAAAD/////AQAAAAAAAAAMAgAAAEZQaWN0dXJlTW92ZXJHdWksIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACUAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbUGljdHVyZU1vdmVyR3VpLkhlbHBlcnMuU2ltcGxlRXZlbnREYXRhLCBQaWN0dXJlTW92ZXJHdWksIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAAClQaWN0dXJlTW92ZXJHdWkuSGVscGVycy5TaW1wbGVFdmVudERhdGFbXQIAAAAICAkDAAAAAAAAAAAAAAAHAwAAAAABAAAAAAAAAAQnUGljdHVyZU1vdmVyR3VpLkhlbHBlcnMuU2ltcGxlRXZlbnREYXRhAgAAAAs=")]  // Binary value for an empty list of this object
        public System.Collections.Generic.List<SimpleEventData> EventList
        {
            get
            {
                return ((System.Collections.Generic.List<SimpleEventData>)(this["EventList"]));
            }
            set
            {
                this["EventList"] = value;
            }
        }
    }
}
