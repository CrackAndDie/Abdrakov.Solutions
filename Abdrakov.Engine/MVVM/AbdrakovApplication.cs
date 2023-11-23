﻿using Abdrakov.Engine.Interfaces;
using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Engine.Services;
using Abdrakov.Logging.Interfaces;
using Abdrakov.Logging.Services;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Abdrakov.Engine.Localization.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Resolution;

namespace Abdrakov.Engine.MVVM
{
    public class AbdrakovApplication : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LocalizationManager.Initialize();
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            if (Container.IsRegistered<IPreviewWindow>())
            {
                Container.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Subscribe(OnPreviewDone);
                return Container.Resolve<IPreviewWindow>() as Window;
            }
            return Container.Resolve<IBaseWindow>() as Window;
        }

        private void OnPreviewDone()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Container.Resolve<IBaseWindow>() as Window;
                window.Show();
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // BaseWindowSettings with DialogWindowSettings should be registered by a concrete project
            containerRegistry.RegisterInstance<ILoggingService>(Log4netLoggingService.GetMainInstance());
            containerRegistry.RegisterSingleton<IViewModelResolverService, ViewModelResolverService>();
            containerRegistry.RegisterSingleton<IWindowProgressService, WindowProgressService>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelResolver = Container.Resolve<IViewModelResolverService>();
                return viewModelResolver.ResolveViewModelType(viewType);
            });

            ViewModelLocationProvider.SetDefaultViewModelFactory((view, viewModelType) =>
            {
                var viewModelResolver = Container.Resolve<IViewModelResolverService>();
                return viewModelResolver.ResolveViewModel(viewModelType, view);
            });
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var container = new UnityContainer();
            container.AddExtension(new Diagnostic());

            return new UnityContainerExtension(container);
        }
    }
}
