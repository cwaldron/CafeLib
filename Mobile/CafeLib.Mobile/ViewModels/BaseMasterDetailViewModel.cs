﻿using CafeLib.Mobile.Commands;
using CafeLib.Mobile.Views;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.ViewModels
{
    public abstract class BaseMasterDetailViewModel<TObject> : BaseViewModel<TObject> where TObject : class
    {
        protected BaseMasterDetailViewModel()
        {
            PresentedCommand = new XamCommand<bool>(x => { });
        }

        private bool _isPresented;
        public bool IsPresented
        {
            get => _isPresented;
            set
            {
                if (!SetValue(ref _isPresented, value)) return;
                ResolvePage<CafeMasterDetailPage>().IsPresented = value;
                _presentedCommand.Execute(_isPresented);
            }
        }

        /// <summary>
        /// Appearing command.
        /// </summary>
        private IXamCommand<bool> _presentedCommand;
        public IXamCommand<bool> PresentedCommand
        {
            set => SetValue(ref _presentedCommand, value);
        }
    }

    public abstract class BaseMasterDetailViewModel : BaseMasterDetailViewModel<object>
    {
    }
}