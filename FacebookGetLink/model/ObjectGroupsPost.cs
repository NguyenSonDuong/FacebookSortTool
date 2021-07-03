using FacebookGetLink.model.reponsive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookGetLink.model
{
    public class ObjectGroupsPost
    {

        public class Post
        {
            public Datum[] data { get; set; }
            public Paging paging { get; set; }
        }

        public class Paging
        {
            public string previous { get; set; }
            public string next { get; set; }
        }

        public class Datum
        {
            public string message { get; set; }
            public string id { get; set; }
            public DateTime created_time { get; set; }
            public Reactions reactions { get; set; }
        }

        public class Reactions
        {
            public object[] data { get; set; }
            public Paging1 paging { get; set; }
            public Summary summary { get; set; }
        }

        public class Paging1
        {
            public Cursors cursors { get; set; }
            public string next { get; set; }
        }

        public class Cursors
        {
            public string before { get; set; }
            public string after { get; set; }
        }

        public class Summary
        {
            public int total_count { get; set; }
        }


    }
}
