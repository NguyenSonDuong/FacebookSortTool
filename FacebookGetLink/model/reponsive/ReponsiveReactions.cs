using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookGetLink.model.reponsive
{
    class ReponsiveReactions
    {
        public class Rootobject
        {
            public Data data { get; set; }
            public Extensions extensions { get; set; }
        }

        public class Data
        {
            public Node node { get; set; }
        }

        public class Node
        {
            public string __typename { get; set; }
            public Reactors reactors { get; set; }
            public string id { get; set; }
        }

        public class Reactors
        {
            public Edge[] edges { get; set; }
            public Page_Info page_info { get; set; }
        }

        public class Page_Info
        {
            public bool has_next_page { get; set; }
            public string end_cursor { get; set; }
        }

        public class Edge
        {
            public Feedback_Reaction feedback_reaction { get; set; }
            public Node1 node { get; set; }
            public string cursor { get; set; }
        }

        public class Feedback_Reaction
        {
            public Face_Image face_image { get; set; }
        }

        public class Face_Image
        {
            public string uri { get; set; }
        }

        public class Node1
        {
            public string id { get; set; }
            public string __typename { get; set; }
            public Mutual_Friends mutual_friends { get; set; }
            public bool can_viewer_message { get; set; }
            public string friendship_status { get; set; }
            public string subscribe_status { get; set; }
            public string secondary_subscribe_status { get; set; }
            public string invite_status_in_feedback { get; set; }
            public string __isActor { get; set; }
            public string name { get; set; }
            public string __isEntity { get; set; }
            public string profile_url { get; set; }
            public Story_Bucket story_bucket { get; set; }
            public string url { get; set; }
            public Profile_Picture profile_picture { get; set; }
            public string __isContextualProfile { get; set; }
        }

        public class Mutual_Friends
        {
            public int count { get; set; }
        }

        public class Story_Bucket
        {
            public Node2[] nodes { get; set; }
        }

        public class Node2
        {
            public string id { get; set; }
            public First_Story_To_Show first_story_to_show { get; set; }
        }

        public class First_Story_To_Show
        {
            public string id { get; set; }
            public Story_Card_Seen_State story_card_seen_state { get; set; }
        }

        public class Story_Card_Seen_State
        {
            public bool is_seen_by_viewer { get; set; }
        }

        public class Profile_Picture
        {
            public string uri { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int scale { get; set; }
        }

        public class Extensions
        {
            public string[] prefetch_uris { get; set; }
            public bool is_final { get; set; }
        }

    }
}
