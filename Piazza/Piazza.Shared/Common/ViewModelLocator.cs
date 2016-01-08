using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Piazza.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Piazza.Common
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ItemViewModel>();
            SimpleIoc.Default.Register<DataVewModel>();
            SimpleIoc.Default.Register<TreeViewPageViewModel>();

            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
 
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            
       }
        public TreeViewPageViewModel TreeViewPageViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<TreeViewPageViewModel>();
            }
        }

        public MainViewModel AppMainViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }

        public ItemViewModel AppItemViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ItemViewModel>();
            }
        }


        public SectionViewModel AppSectionViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SectionViewModel>();
            }
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("Section", typeof(SectionPage));
            navigationService.Configure("Hub", typeof(HubPage));
            navigationService.Configure("Item", typeof(ItemPage));
            return navigationService;
        }
    }
}
