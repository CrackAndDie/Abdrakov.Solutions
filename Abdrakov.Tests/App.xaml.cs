﻿using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.MVVM;
using Abdrakov.Styles;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Abdrakov.CommonWPF.Views;
using System.ComponentModel;
using Abdrakov.Tests.Interfaces;
using Abdrakov.Tests.Views;
using Abdrakov.Engine.Utils.Settings;
using System.Windows.Media;
using Prism.Modularity;
using Abdrakov.Tests.Modules;

namespace Abdrakov.Tests
{
    public partial class App : AbdrakovApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Add(new AbdrakovBundledTheme()
            {
                PrimaryColor = Colors.Gray,
            });
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            var viewModelService = Container.Resolve<IViewModelResolverService>();
            viewModelService.RegisterViewModelAssembly(Assembly.GetExecutingAssembly());

            return base.CreateShell();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.RegisterInstance(new BaseWindowSettings()
            {
                ProductName = "Tests",
                LogoImage = "pack://application:,,,/Abdrakov.Tests;component/Resources/AbdrakovSolutions.png",
                WindowProgressBrush = new SolidColorBrush(Colors.Azure),
            });
            containerRegistry.RegisterSingleton<IBaseWindow, MainWindowView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
        }
    }
}
