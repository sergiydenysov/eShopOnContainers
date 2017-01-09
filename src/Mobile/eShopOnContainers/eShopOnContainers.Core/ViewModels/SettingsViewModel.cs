﻿using eShopOnContainers.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using eShopOnContainers.Core.Helpers;

namespace eShopOnContainers.Core.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private bool _useAzureServices;
        private string _endpoint;

        public SettingsViewModel()
        {
            UseAzureServices = !Settings.UseMocks;
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public bool UseAzureServices
        {
            get { return _useAzureServices; }
            set
            {
                _useAzureServices = value;

                // Save use mocks services to local storage
                Settings.UseMocks = !_useAzureServices;
                RaisePropertyChanged(() => UseAzureServices);
            }
        }

        public string Endpoint
        {
            get { return _endpoint; }
            set
            {
                _endpoint = value;

                if(!string.IsNullOrEmpty(_endpoint))
                {
                    UpdateEndpoint(_endpoint);
                }

                RaisePropertyChanged(() => Endpoint);
            }
        }

        public ICommand MockServicesCommand => new Command(MockServices);

        private void MockServices()
        {
            ViewModelLocator.Instance.UpdateDependencies(!UseAzureServices);
            UpdateInfo();
        }

        public override Task InitializeAsync(object navigationData)
        {
            UpdateInfo();

            Endpoint = Settings.UrlBase;

            return base.InitializeAsync(navigationData);
        }

        private void UpdateInfo()
        {
            if (!UseAzureServices)
            {
                Title = "Use Mock Services";
                Description = "Mock Services are simulated objects that mimic the behavior of real services in controlled ways";
            }
            else
            {
                Title = "Use Azure Services";
                Description = "Azure Services are real objects that required a valid internet connection";
            }
        }

        private void UpdateEndpoint(string endpoint)
        {
            // Update remote endpoint (save to local storage)
            Settings.UrlBase = endpoint;
        }
    }
}