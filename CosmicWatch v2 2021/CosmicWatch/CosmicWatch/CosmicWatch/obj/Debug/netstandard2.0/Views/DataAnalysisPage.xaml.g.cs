//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("CosmicWatch.Views.DataAnalysisPage.xaml", "Views/DataAnalysisPage.xaml", typeof(global::CosmicWatch.Views.DisplayAnalysisPage))]

namespace CosmicWatch.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\DataAnalysisPage.xaml")]
    public partial class DisplayAnalysisPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Picker Datasets;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::System.Array supportedDevicesList;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Picker Display_Type;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::System.Array supportedGraphsList;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Picker X_Axis;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::System.Array generatedXAxisOptions;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Picker Y_Axis;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::System.Array generatedYAxisOptions;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Label statusDisplay;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.HtmlWebViewSource HtmlSource;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(DisplayAnalysisPage));
            Datasets = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Picker>(this, "Datasets");
            supportedDevicesList = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::System.Array>(this, "supportedDevicesList");
            Display_Type = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Picker>(this, "Display_Type");
            supportedGraphsList = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::System.Array>(this, "supportedGraphsList");
            X_Axis = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Picker>(this, "X_Axis");
            generatedXAxisOptions = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::System.Array>(this, "generatedXAxisOptions");
            Y_Axis = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Picker>(this, "Y_Axis");
            generatedYAxisOptions = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::System.Array>(this, "generatedYAxisOptions");
            statusDisplay = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Label>(this, "statusDisplay");
            HtmlSource = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.HtmlWebViewSource>(this, "HtmlSource");
        }
    }
}
