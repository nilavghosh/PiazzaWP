using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Piazza.Definitions;
using Windows.Storage;
using Windows.Data.Json;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using Piazza.Messages;
using System.Net.Http;
using System.Linq;


namespace Piazza.ViewModel
{
    public class ItemViewModel : ViewModelBase
    {
        private PiazzaPost _itemPost = new PiazzaPost();
        public CoreDispatcher _dispatcher { get; set; }

        public PiazzaPost ItemPost
        {
            get
            {
                return _itemPost;
            }
            set
            {
                if (value != _itemPost)
                {
                    _itemPost = value;
                    RaisePropertyChanged("ItemPost");
                }
            }
        }
        public ItemViewModel()
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            Messenger.Default.Register<FetchPostMessage>(this, message => 
            {
                Task.Run(() => this.GetPost(message));
            });
            //Task.Run(() => this.LoadPostData()).Wait();
        }


        public async void GetPost(FetchPostMessage message)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.HostUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData["Cookies"] = JsonConvert.DeserializeObject(message.Cookies);
                postData["ClassId"] = message.SelectedClass.CourseID;
                postData["FeedId"] = message.SelectedFeedItem.PostId;

                HttpResponseMessage response = await client.PostAsync("/GetPost", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    //var FeedItem = JsonConvert.DeserializeObject<PiazzaPost>(data.Result.ToString());

                    await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        ItemPost = JsonConvert.DeserializeObject<PiazzaPost>(data.Result.ToString());
                        //FetchUsers(ItemPost.change_log.Select(cl => cl.uid).Distinct().ToList(),message);
                        List<String> uids = new List<String>();
                        foreach (Child ch in ItemPost.children)
                        {
                         if(ch.uid!=null)   uids.Add(ch.uid);
                            foreach (Child ch2 in ch.children)
                            {
                                if(ch2.uid!=null) uids.Add(ch2.uid);
                            }
                        }
                        FetchUsers(uids.Distinct().ToList(),message);
                        
                        //Content = HtmlAgilityPack.HtmlEntity.DeEntitize(FeedItem.history[0].content);
                    });
                }
            }
        }

        public void GetUsers(List<string> userIds, FetchPostMessage message)
        { 
                   
        }

        public async void FetchUsers(List<string> userIds, FetchPostMessage message)
        {
            //https://d1b10bmlvqabco.cloudfront.net/photos/i4g4z0lt7kX/1420303772_35.png

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(App.HostUrl);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData["Cookies"] = JsonConvert.DeserializeObject(message.Cookies);
                postData["ClassId"] = message.SelectedClass.CourseID;
                postData["FeedId"] = message.SelectedFeedItem.PostId;
                postData["Users"] = userIds;
               
                HttpResponseMessage response = await client.PostAsync("/GetUsers", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    //var FeedItem = JsonConvert.DeserializeObject<PiazzaPost>(data.Result.ToString());

                    await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                            List<User> Users = JsonConvert.DeserializeObject<List<User>>(data.Result.ToString());
                            App.Users = Users.ToDictionary(u => u.id);
                            foreach (Child ch in ItemPost.children)
                            {
                                if (ch.uid != null) ch.user = App.Users[ch.uid];
                                foreach (Child ch2 in ch.children)
                                {
                                    if (ch2.uid != null) ch2.user = App.Users[ch2.uid];
                                }
                            }
                      
                    });
                }
            }
        }

        public async void LoadPostData()
        {
            Uri dataUri = new Uri("ms-appx:///DataModel/PostData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        ItemPost = JsonConvert.DeserializeObject<PiazzaPost>(jsonText);
                    });
        }
    }
}