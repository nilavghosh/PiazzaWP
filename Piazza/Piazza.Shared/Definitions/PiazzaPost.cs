using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Piazza.Definitions
{
    public class ChangeLog
    {
        public string anon { get; set; }
        public string data { get; set; }
        public string type { get; set; }
        public string uid { get; set; }
        public string when { get; set; }
        public string to { get; set; }
    }

    public class Config
    {
    }

    public class Data
    {
        public List<object> embed_links { get; set; }
    }

    public class History
    {
        public string anon { get; set; }
        public string content { get; set; }
        public string created { get; set; }
        public string subject { get; set; }
        public string uid { get; set; }
    }

    public class TagEndorse
    {
        public bool admin { get; set; }
        public object facebook_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string photo { get; set; }
        public string role { get; set; }
        public bool us { get; set; }
    }

    public class Child : INotifyPropertyChanged
    {
        public string bucket_name { get; set; }
        public int bucket_order { get; set; }
        public List<Child> children { get; set; }
        public Config config { get; set; }
        public string created { get; set; }
        public Data data { get; set; }
        public List<object> folders { get; set; }
        public List<History> history { get; set; }
        public string id { get; set; }
        public bool is_tag_endorse { get; set; }
        public List<TagEndorse> tag_endorse { get; set; }
        public List<string> tag_endorse_arr { get; set; }
        public string type { get; set; }
        public string anon { get; set; }
        public int? no_answer { get; set; }
        public int? no_upvotes { get; set; }
        public string subject { get; set; }
        public string uid { get; set; }
        public string updated { get; set; }
        private User _user;
        public User user
        {
            get
            {
                return _user;
            }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    NotifyPropertyChanged("user");
                }
            }
        }
        public Uri selfie
        {
            get
            {
                return new Uri("https://d1b10bmlvqabco.cloudfront.net/photos/i4h9vkyjZOa/1420306737_35.png");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }

    public class Config2
    {
    }

    public class Data2
    {
        public List<object> embed_links { get; set; }
    }

    public class History2
    {
        public string anon { get; set; }
        public string content { get; set; }
        public string created { get; set; }
        public string subject { get; set; }
        public string uid { get; set; }
    }

    public class TagGood
    {
        public bool admin { get; set; }
        public object facebook_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string photo { get; set; }
        public string role { get; set; }
        public bool us { get; set; }
    }

    public class PiazzaPost
    {
        public int bookmarked { get; set; }
        public string bucket_name { get; set; }
        public int bucket_order { get; set; }
        public List<ChangeLog> change_log { get; set; }
        public List<Child> children { get; set; }
        public Config2 config { get; set; }
        public string created { get; set; }
        public Data2 data { get; set; }
        public string default_anonymity { get; set; }
        public List<string> folders { get; set; }
        public List<History2> history { get; set; }
        public List<object> i_edits { get; set; }
        public string id { get; set; }
        public bool is_bookmarked { get; set; }
        public bool is_tag_good { get; set; }
        public bool my_favorite { get; set; }
        public int no_answer { get; set; }
        public int no_answer_followup { get; set; }
        public int nr { get; set; }
        public int num_favorites { get; set; }
        public List<object> q_edits { get; set; }
        public int request_instructor { get; set; }
        public bool request_instructor_me { get; set; }
        public List<object> s_edits { get; set; }
        public string status { get; set; }
        public long t { get; set; }
        public List<TagGood> tag_good { get; set; }
        public List<string> tag_good_arr { get; set; }
        public List<string> tags { get; set; }
        public string type { get; set; }
        public int unique_views { get; set; }
        public List<object> upvote_ids { get; set; }
    }

    public class User
    {
        public bool admin { get; set; }
        public string facebook_id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string photo { get; set; }
        public string role { get; set; }
        public bool us { get; set; }
        public Uri photoUri
        {
            get
            {
                if (photo != null && id !=null)
                {
                    return new Uri(@"https://d1b10bmlvqabco.cloudfront.net/photos/" + id + @"/" + photo);
                }
                else
                {
                    return new Uri(@"ms-appx:///Assets/default_user.png");
                }
            }
        }
    }
}

