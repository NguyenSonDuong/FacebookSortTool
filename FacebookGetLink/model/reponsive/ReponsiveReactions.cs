using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookGetLink.model.reponsive
{
    public class ReponsiveReactions
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

    public class ReactionCount
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
            public Associated_Group associated_group { get; set; }
            public Reaction_Display_Config reaction_display_config { get; set; }
            public Top_Reactions top_reactions { get; set; }
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

        public class Associated_Group
        {
            public string id { get; set; }
        }

        public class Reaction_Display_Config
        {
            public string reaction_display_strategy { get; set; }
            public object reaction_sheet_explanation_string { get; set; }
            public __Module_Operation_Cometufireactionsdialogquery __module_operation_CometUFIReactionsDialogQuery { get; set; }
            public __Module_Component_Cometufireactionsdialogquery __module_component_CometUFIReactionsDialogQuery { get; set; }
        }

        public class __Module_Operation_Cometufireactionsdialogquery
        {
            public string __dr { get; set; }
        }

        public class __Module_Component_Cometufireactionsdialogquery
        {
            public string __dr { get; set; }
        }

        public class Top_Reactions
        {
            public int count { get; set; }
            public Summary[] summary { get; set; }
        }

        public class Summary
        {
            public int reaction_count { get; set; }
            public string reaction_count_reduced { get; set; }
            public Reaction reaction { get; set; }
        }

        public class Reaction
        {
            public string reaction_type { get; set; }
            public string localized_name { get; set; }
            public string color { get; set; }
            public Face_Image1 face_image { get; set; }
            public string id { get; set; }
        }

        public class Face_Image1
        {
            public string uri { get; set; }
        }

        public class Extensions
        {
            public string[] prefetch_uris { get; set; }
            public Prefetch_Uris_V2[] prefetch_uris_v2 { get; set; }
            public bool is_final { get; set; }
            public Sr_Payload sr_payload { get; set; }
        }

        public class Sr_Payload
        {
            public Ddd ddd { get; set; }
        }

        public class Ddd
        {
            public Hsrp hsrp { get; set; }
            public Jsmods jsmods { get; set; }
            public string[] allResources { get; set; }
        }

        public class Hsrp
        {
            public Hsdp hsdp { get; set; }
            public Hblp hblp { get; set; }
        }

        public class Hsdp
        {
            public Gkxdata gkxData { get; set; }
            public Qexdata qexData { get; set; }
        }

        public class Gkxdata
        {
            public _708253 _708253 { get; set; }
            public _996940 _996940 { get; set; }
            public _1224637 _1224637 { get; set; }
            public _1263340 _1263340 { get; set; }
            public _729630 _729630 { get; set; }
            public _729631 _729631 { get; set; }
            public _976093 _976093 { get; set; }
            public _1070056 _1070056 { get; set; }
            public _1099893 _1099893 { get; set; }
            public _1105608 _1105608 { get; set; }
            public _1281505 _1281505 { get; set; }
            public _1291023 _1291023 { get; set; }
            public _1294182 _1294182 { get; set; }
            public _1381768 _1381768 { get; set; }
            public _1399218 _1399218 { get; set; }
            public _1401060 _1401060 { get; set; }
            public _1409295 _1409295 { get; set; }
            public _1435443 _1435443 { get; set; }
            public _1441635 _1441635 { get; set; }
            public _1465547 _1465547 { get; set; }
            public _1485055 _1485055 { get; set; }
            public _1584797 _1584797 { get; set; }
            public _1597642 _1597642 { get; set; }
            public _1620803 _1620803 { get; set; }
            public _1647260 _1647260 { get; set; }
            public _1661070 _1661070 { get; set; }
            public _1695831 _1695831 { get; set; }
            public _1722014 _1722014 { get; set; }
            public _1723588 _1723588 { get; set; }
            public _1742795 _1742795 { get; set; }
            public _1745526 _1745526 { get; set; }
            public _1778302 _1778302 { get; set; }
            public _1839774 _1839774 { get; set; }
            public _1840809 _1840809 { get; set; }
            public _1848749 _1848749 { get; set; }
            public _1861546 _1861546 { get; set; }
        }

        public class _708253
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _996940
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1224637
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1263340
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _729630
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _729631
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _976093
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1070056
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1099893
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1105608
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1281505
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1291023
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1294182
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1381768
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1399218
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1401060
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1409295
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1435443
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1441635
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1465547
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1485055
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1584797
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1597642
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1620803
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1647260
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1661070
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1695831
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1722014
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1723588
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1742795
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1745526
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1778302
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1839774
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1840809
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1848749
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class _1861546
        {
            public bool result { get; set; }
            public string hash { get; set; }
        }

        public class Qexdata
        {
            public _1768468 _1768468 { get; set; }
            public _1853853 _1853853 { get; set; }
            public _1853854 _1853854 { get; set; }
        }

        public class _1768468
        {
            public bool r { get; set; }
        }

        public class _1853853
        {
            public bool r { get; set; }
            public string l { get; set; }
        }

        public class _1853854
        {
            public bool r { get; set; }
        }

        public class Hblp
        {
            public int sr_revision { get; set; }
            public Consistency consistency { get; set; }
            public Rsrcmap rsrcMap { get; set; }
            public Compmap compMap { get; set; }
        }

        public class Consistency
        {
            public int rev { get; set; }
        }

        public class Rsrcmap
        {
            public Csr_1U_0 csr_1u_0 { get; set; }
            public Csr_1U_1 csr_1u_1 { get; set; }
            public Qx34n Qx34n { get; set; }
        }

        public class Csr_1U_0
        {
            public string type { get; set; }
            public string src { get; set; }
        }

        public class Csr_1U_1
        {
            public string type { get; set; }
            public string src { get; set; }
        }

        public class Qx34n
        {
            public string type { get; set; }
            public string src { get; set; }
            public int d { get; set; }
            public int nc { get; set; }
            public string p { get; set; }
        }

        public class Compmap
        {
            public Cometufireactiondialogexplanationstring_ReactiondisplayconfigNormalizationGraphql CometUFIReactionDialogExplanationString_reactionDisplayConfignormalizationgraphql { get; set; }
            public CometufireactiondialogexplanationstringReact CometUFIReactionDialogExplanationStringreact { get; set; }
        }

        public class Cometufireactiondialogexplanationstring_ReactiondisplayconfigNormalizationGraphql
        {
            public string[] r { get; set; }
        }

        public class CometufireactiondialogexplanationstringReact
        {
            public string[] r { get; set; }
        }

        public class Jsmods
        {
            public object[][] define { get; set; }
            public object[][] require { get; set; }
        }

        public class Prefetch_Uris_V2
        {
            public string uri { get; set; }
            public object label { get; set; }
        }

    }
}
