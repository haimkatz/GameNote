using Microsoft.WindowsAzure.Mobile.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameNote.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }

    public class Team : EntityData
    {

        public string team_id { get; set; }
        public string abbreviation { get; set; }
        public bool active { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string conference { get; set; }
        public string division { get; set; }
        public string site_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string full_name { get; set; }
        public string gamenotes { get; set; }
        public virtual ICollection<Player> players { get; set; }
    }

    public class Player : EntityData

    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int int_id { get; set; }

        public string last_name { get; set; }
        public string first_name { get; set; }
        public string display_name { get; set; }
        public string birthdate { get; set; }
        public int age { get; set; }
        public string birthplace { get; set; }
        public int height_in { get; set; }
        public double height_cm { get; set; }
        public double height_m { get; set; }
        public string height_formatted { get; set; }
        public int weight_lb { get; set; }
        public double weight_kg { get; set; }
        public string position { get; set; }
        public int uniform_number { get; set; }
        public string team_id { get; set; }

        [ForeignKey("team")]
        public string mobileteam_id { get; set; }

        public string playernote { get; set; }
        public virtual Team team { get; set; }
    }

    public class Event:EntityData
    {
        public string event_id { get; set; }
        public string event_status { get; set; }
        public string sport { get; set; }
        public string start_date_time { get; set; }
        public string season_type { get; set; }
        public string away_teamid { get; set; }
        public string home_teamid { get; set; }
        public string sys_htid { get; set; }
        public string sys_atid { get; set; }
        public List<int> away_period_scores { get; set; }
        public List<int> home_period_scores { get; set; }
        public int away_points_scored { get; set; }
        public int home_points_scored { get; set; }
        public string GameNote { get; set; }
        //public virtual string display_time
        //{
        //    get
        //    {
        //        int timestart = start_date_time.IndexOf("T");

        //        return start_date_time.Substring(timestart + 1, 5);
        //    }
        //}

    }

    public class UserGameNote:EntityData
    {
        public string UserID { get; set; }
        public string gameid { get; set; }
        public string GameNote { get; set; }
        public bool? MakePublic { get; set; }
       }

    public class UserTeamNote : EntityData
    {
        public string UserID { get; set; }
        public string teamid { get; set; }
        public string TeamNote { get; set; }
        public bool MakePublic { get; set; }

    }

    public class UserPlayerNote : EntityData
    {
        public string UserID { get; set; }
        public string playerid { get; set; }
        public string playernote { get; set; }
        public bool? MakePublic { get; set; }
    }

}