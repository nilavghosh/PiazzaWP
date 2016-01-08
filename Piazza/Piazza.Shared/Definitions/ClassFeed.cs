using Newtonsoft.Json;
using Piazza.DataBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;

namespace Piazza.Definitions
{
    public class ClassFeed
    {
        public String PostId { get; set; }
        public String Subject { get; set; }
        public String ContentSnippet { get; set; }
        public Boolean IsPinned { get; set; }
    }

    public class ClassFeedSource : IIncrementalSource<ClassFeed>
    {

        public async Task<IEnumerable<ClassFeed>> GetPagedItems(int pageIndex, int pageSize)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(App.HostUrl);
                Dictionary<string, object> postData = new Dictionary<string, object>();
                postData["Cookies"] = JsonConvert.DeserializeObject(App.Cookies);
                postData["ClassId"] = App.SelectedClass.CourseID;
                postData["Limit"] = pageSize;
                postData["Offset"] = pageSize*pageIndex;


                var Feeds = new List<ClassFeed>();
                HttpResponseMessage response = await client.PostAsync("/GetFeed", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<RootFeed>(data.Result.ToString());
                    foreach (Feed fd in root.feed)
                    {
                        Feeds.Add(new ClassFeed() { PostId = fd.id, Subject = HtmlUtilities.ConvertToText(HtmlAgilityPack.HtmlEntity.DeEntitize(fd.subject)), IsPinned= fd.pin == 1 ? true : false, ContentSnippet = HtmlUtilities.ConvertToText(HtmlAgilityPack.HtmlEntity.DeEntitize(fd.content_snipet)) });
                    }
                }
                return Feeds;
            }
        }
    }
}

