using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Piazza.Definitions;
using System.Threading.Tasks;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using WinRTXamlToolkit.Tools;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Piazza.Common;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Windows.Data.Html;
using Windows.UI.Xaml.Data;
using GalaSoft.MvvmLight.Ioc;
using Piazza.DataBinding;
using System.Linq;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Imaging;
using Windows.UI;
using GalaSoft.MvvmLight.Messaging;
using Piazza.Messages;


namespace Piazza.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService)
        {
            //var itemmodel = SimpleIoc.Default.GetInstance<ItemViewModel>();
            _navigationService = navigationService;
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            this.Filters = new ObservableCollection<string>();
            this.RegisteredClasses = new ObservableCollection<RegisteredClass>();
            this.Feeds = new ObservableCollection<ClassFeed>();
            NetworkSelectedCommand = new RelayCommand(new System.Action(() => NetworkSelectedExecute(null)), new System.Func<bool>(() => NetworkSelectedCanExecute(null)));
            PostSelectedCommand = new RelayCommand(new System.Action(() => PostSelectedExecute(null)), new System.Func<bool>(() => PostSelectedCanExecute(null)));
            Content = "ok";
            HostUrl = @"http://104.215.144.150/";
            //HostUrl = @"http://127.0.0.1:8021/";

            LoadData();
            Feeds2 = new IncrementalLoadingCollection<ClassFeedSource, ClassFeed>(20);
            _branchId = 1;
            TreeItems = BuildTree(5, 5);
        }
        private int _branchId;
        private Random _rand = new Random();
        private List<Color> _namedColors = ColorExtensions.GetNamedColors();
        private ObservableCollection<TreeItemViewModel> BuildTree(int depth, int branches)
        {
            var tree = new ObservableCollection<TreeItemViewModel>();

            if (depth > 0)
            {
                var depthIndices = Enumerable.Range(0, branches).Shuffle();

                for (int i = 0; i < branches; i++)
                {
                    var d = depthIndices[i] % depth;
                    var b = _rand.Next(branches / 2, branches);

                    tree.Add(
                        new TreeItemViewModel
                        {
                            Text = "Branch " + _branchId++,
                            Brush = new SolidColorBrush(_namedColors[_rand.Next(0, _namedColors.Count)]),
                            Children = BuildTree(d, b)
                        });
                }
            }

            return tree;
        }
        public ObservableCollection<TreeItemViewModel> TreeItems;
       

        public String HostUrl
        {
            get
            {
                return App.HostUrl;
            }
            set
            {
                App.HostUrl = value;
            }
        }
        private INavigationService _navigationService;
        public ObservableCollection<String> Filters { get; set; }
        public CoreDispatcher _dispatcher { get; set; }

        public static String Cookies
        {
            get
            {
                return App.Cookies;
            }
            set
            {
                App.Cookies = value;
            }
        }
        public RegisteredClass SelectedClass
        {
            get
            {
                return App.SelectedClass;
            }
            set
            {
                App.SelectedClass = value;
            }
        }
        public ClassFeed SelectedFeedItem { get; set; }
        public IncrementalLoadingCollection<ClassFeedSource, ClassFeed> Feeds2 { get; set; }

        public ObservableCollection<RegisteredClass> RegisteredClasses { get; set; }
        public ObservableCollection<ClassFeed> Feeds { get; set; }
        public RelayCommand NetworkSelectedCommand { get; set; }
        public RelayCommand PostSelectedCommand { get; set; }

        public async void LoginAndGetClasses()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(HostUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    Cookies = data.Result.ToString();
                    Task.Run(() => this.GetClasses()).Wait();
                }
            }
        }


        public async void GetPost()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(HostUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData["Cookies"] = JsonConvert.DeserializeObject(Cookies);
                postData["ClassId"] = SelectedClass.CourseID;
                postData["FeedId"] = SelectedFeedItem.PostId;
                
                HttpResponseMessage response = await client.PostAsync("/GetPost", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    var FeedItem = JsonConvert.DeserializeObject<PiazzaPost>(data.Result.ToString());

                    await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        Content = HtmlAgilityPack.HtmlEntity.DeEntitize(FeedItem.history[0].content);
                    });
                }
            }
        }

        public async void GetClasses()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(HostUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                HttpResponseMessage response = await client.PostAsync("/GetClasses", new StringContent(Cookies, Encoding.UTF8, "application/json"));

                //  HttpResponseMessage response = await client.PostAsync(.GetAsync("/GetClasses");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    var classes = JsonConvert.DeserializeObject<List<RegClass>>(data.Result.ToString());
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (RegClass cs in classes)
                        {
                            RegisteredClasses.Add(new RegisteredClass() { CourseName = cs.name, Source = cs.num, Term = cs.term, CourseID = cs.nid, IsActive = cs.is_ta });
                            //Items.Add(new ItemViewModel() { Subject = feed.subject, ContentSnippet = feed.content_snipet });
                        }
                        //feedData.tags.popular.ForEach(tag => Filters.Add(tag));
                    });
                }
            }
        }

        public async void GetFeed()
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(HostUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData["Cookies"] = JsonConvert.DeserializeObject(Cookies);
                postData["ClassId"] = SelectedClass.CourseID;
                postData["Limit"] = 10;
                postData["Offset"] = 0;

                HttpResponseMessage response = await client.PostAsync("/GetFeed", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));

                //  HttpResponseMessage response = await client.PostAsync(.GetAsync("/GetClasses");
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<RootFeed>(data.Result.ToString());
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Feeds.Clear();
                        foreach (Feed fd in root.feed)
                        {
                            //RegisteredClasses.Add(new RegisteredClass() { CourseName = cs.name, Source = cs.num, Term = cs.term, CourseID = cs.nid, IsActive = cs.is_ta });
                            Feeds.Add(new ClassFeed() { PostId = fd.id, Subject = fd.subject, ContentSnippet = HtmlUtilities.ConvertToText(HtmlAgilityPack.HtmlEntity.DeEntitize(fd.content_snipet)) });
                        }
                        //feedData.tags.popular.ForEach(tag => Filters.Add(tag));
                    });
                }
            }
        }



        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            Task.Run(() => this.LoginAndGetClasses()).Wait();
            this.IsDataLoaded = true;
        }

        #region INotifyProperties
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }
        #endregion


        #region Commands
        public bool NetworkSelectedCanExecute(object param)
        {
            return true;
        }

        public void NetworkSelectedExecute(object param)
        {
            Feeds2.Clear();
            _navigationService.NavigateTo("Section", SelectedClass);
        }

        public bool PostSelectedCanExecute(object param)
        {
            return true;
        }

        public void PostSelectedExecute(object param)
        {
            _navigationService.NavigateTo("Item", SelectedFeedItem);
            Messenger.Default.Send<FetchPostMessage>(new FetchPostMessage() { Cookies = Cookies, SelectedClass = SelectedClass, SelectedFeedItem = SelectedFeedItem });
            //Task.Run(() => this.GetPost());
        }
        #endregion


    }
}