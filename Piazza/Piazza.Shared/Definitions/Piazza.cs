using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piazza.Definitions
{
    public class RegClass
    {
        public bool is_ta { get; set; }
        public string name { get; set; }
        public string nid { get; set; }
        public string num { get; set; }
        public string term { get; set; }
    }


    public class Draft
    {
    }

    public class Drafts
    {
    }

    public class Log
    {
        public string n { get; set; }
        public string t { get; set; }
        public string u { get; set; }
        public string cid { get; set; }
        public string w { get; set; }
    }

    public class Feed
    {
        public string bucket_name { get; set; }
        public int bucket_order { get; set; }
        public string content_snipet { get; set; }
        public string fol { get; set; }
        public List<string> folders { get; set; }
        public int gd { get; set; }
        public string id { get; set; }
        public bool is_new { get; set; }
        public List<Log> log { get; set; }
        public object m { get; set; }
        public int main_version { get; set; }
        public string modified { get; set; }
        public int no_answer_followup { get; set; }
        public int nr { get; set; }
        public int num_favorites { get; set; }
        public int pin { get; set; }
        public int request_instructor { get; set; }
        public int rq { get; set; }
        public double score { get; set; }
        public string status { get; set; }
        public string subject { get; set; }
        public int tag_good_prof { get; set; }
        public List<string> tags { get; set; }
        public string type { get; set; }
        public int unique_views { get; set; }
        public string updated { get; set; }
        public int version { get; set; }
        public int view_adjust { get; set; }
    }

    public class BestAnswer
    {
        public int nr { get; set; }
        public string text { get; set; }
        public int time { get; set; }
        public string uid { get; set; }
        public int when { get; set; }
    }

    public class Hof
    {
        public List<BestAnswer> best_answer { get; set; }
    }

    public class NotificationSubjects
    {
    }

    public class InstructorCount
    {
        public int assignment1 { get; set; }
        public int assignment2 { get; set; }
        public int assignment3 { get; set; }
        public int assignment6 { get; set; }
        public int lecture1 { get; set; }
        public int lecture2 { get; set; }
        public int lecture3 { get; set; }
        public int lecture4 { get; set; }
        public int lecture5 { get; set; }
        public int lecture6 { get; set; }
        public int logistics { get; set; }
        public int quizzes { get; set; }
    }

    public class InstructorUpd
    {
        public long assignment1 { get; set; }
        public long assignment2 { get; set; }
        public long assignment3 { get; set; }
        public long assignment4 { get; set; }
        public long assignment5 { get; set; }
        public long assignment6 { get; set; }
        public long hw1 { get; set; }
        public long lecture1 { get; set; }
        public long lecture2 { get; set; }
        public long lecture3 { get; set; }
        public long lecture4 { get; set; }
        public long lecture5 { get; set; }
        public long lecture6 { get; set; }
        public long logistics { get; set; }
        public long quizzes { get; set; }
    }

    public class PopularCount
    {
        public int assignment1 { get; set; }
        public int assignment2 { get; set; }
        public int assignment3 { get; set; }
        public int assignment4 { get; set; }
        public int assignment5 { get; set; }
        public int assignment6 { get; set; }
        public int lecture1 { get; set; }
        public int lecture2 { get; set; }
        public int lecture3 { get; set; }
        public int lecture4 { get; set; }
        public int lecture5 { get; set; }
        public int lecture6 { get; set; }
        public int logistics { get; set; }
        public int quizzes { get; set; }
        public int regrades { get; set; }
    }

    public class Tags
    {
        public List<string> instructor { get; set; }
        public InstructorCount instructor_count { get; set; }
        public InstructorUpd instructor_upd { get; set; }
        public List<string> popular { get; set; }
        public PopularCount popular_count { get; set; }
    }

    public class RootFeed
    {
        public int avg { get; set; }
        public object avg_cnt { get; set; }
        public Draft draft { get; set; }
        public Drafts drafts { get; set; }
        public List<Feed> feed { get; set; }
        public Hof hof { get; set; }
        public List<string> last_networks { get; set; }
        public bool more { get; set; }
        public int no_open_teammate_search { get; set; }
        public NotificationSubjects notification_subjects { get; set; }
        public List<object> notifications { get; set; }
        public string sort { get; set; }
        public long t { get; set; }
        public Tags tags { get; set; }
        public int users { get; set; }
        public int users_7 { get; set; }
    }

}
