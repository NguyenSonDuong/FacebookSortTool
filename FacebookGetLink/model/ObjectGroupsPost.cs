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
            public string link { get; set; }
            public string full_picture { get; set; }
            public string picture { get; set; }
            public DateTime created_time { get; set; }
        }

    }
}
