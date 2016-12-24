//------------------------------------------------------------------------------
// <copyright file="VsTrelloPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel;
using System.Windows;
using Scrubs.VisualStudio;
using VsTrello.UI;
using VsTrello.Services;
using System.ComponentModel.Design;
using VsTrello.ViewModels;

namespace VsTrello
{
    [Guid(Guids.OptionsPage)]
    public class OptionPageCustom : UIElementDialogPage
    {
        OptionsPageControl child;
        IPackageSettings packageSettings;

        protected override UIElement Child
        {
            get { return child ?? (child = new OptionsPageControl()); }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            packageSettings = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                packageSettings.Save();
            }

            base.OnApply(args);
        }
    }

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Guids.Package)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideOptionPage(typeof(OptionPageCustom), "VsTrello", "General", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(CardToolWindow))]
    [ProvideService(typeof(SToolWindowManager))]

    public sealed class VsTrelloPackage : Package, IToolWindowManager, SToolWindowManager
    {
        public static string AppName = "VsTrello";
        public static VsTrelloPackage Package;

        /// <summary>
        /// Initializes a new instance of the <see cref="VsTrelloPackage"/> class.
        /// </summary>
        public VsTrelloPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
            Package = this;
            IServiceContainer serviceContainer = this;
            ServiceCreatorCallback creationCallback = CreateService;
            serviceContainer.AddService(typeof(SToolWindowManager), creationCallback, true);
        }

        private object CreateService(IServiceContainer container, Type serviceType)
        {
            if (container != this)
            {
                return null;
            }
            if (typeof(SToolWindowManager) == serviceType)
            {
                return this;
            }
            return null;
        }

        public void OpenCardEditorWindow(CardViewModel vm)
        {
            ToolWindowPane windowPane = FindToolWindow(typeof(CardToolWindow), 0, true);
            var control = windowPane.Content as CardToolWindowControl;
            if (control != null)
            {
                var frame = windowPane.Frame as IVsWindowFrame;
                if (frame != null)
                {
                    frame.Show();
                }
                if(control.DataContext != null && control.DataContext is IDisposable)
                {
                    ((IDisposable)control.DataContext).Dispose();
                }
                control.DataContext = vm;
            }
        }

        public void ShowCardWindow()
        {
            CardToolWindow window = (CardToolWindow)VsTrelloPackage.Package.FindToolWindow(typeof(CardToolWindow), 0, true); // True means: crate if not found. 0 means there is only 1 instance of this tool window
            if (null == window || null == window.Frame)
                throw new NotSupportedException("CardToolWindow not found");
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Scrubs.VisualStudio.Services.PackageServiceProvider = this;

            
        }

        #endregion
    }
}
