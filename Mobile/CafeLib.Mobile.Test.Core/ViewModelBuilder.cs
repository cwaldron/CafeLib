﻿using System.Windows.Input;
using CafeLib.Mobile.ViewModels;
using Moq;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Test.Core
{
    public class ViewModelBuilder<T> : MockBuilderBase<T> where T : BaseViewModel
    {
        public Mock<ICommand> CloseCommandMock { get; set; }

        public ViewModelBuilder(MobileUnitTest test) 
            : base(test)
        {
            OnCreate = Create;
        }

        public override T Build()
        {
            var vm = OnCreate();
            CloseCommandMock = new Mock<ICommand>();
            vm.CloseCommand = CloseCommandMock.Object;
            return vm;
        }
    }
}