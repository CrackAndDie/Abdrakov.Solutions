﻿using Abdrakov.Engine.Interfaces.Presentation;
using Abdrakov.Engine.MVVM;
using Abdrakov.Engine.MVVM.Events;
using Abdrakov.Styles.Other.Events;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Abdrakov.Tests.Views
{
    public partial class PreviewWindowView : Window, IPreviewWindow
    {
        private DispatcherTimer timer;
        public PreviewWindowView()
        {
            InitializeComponent();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(4),
            };
            timer.Tick += (s, a) => { CallPreviewDoneEvent(); };
            timer.Start();
        }

        public void CallPreviewDoneEvent()
        {
            timer.Stop();
            var cont = (Application.Current as AbdrakovApplication).Container;
            cont.Resolve<IEventAggregator>().GetEvent<PreviewDoneEvent>().Publish();
            this.Close();
        }
    }
}
