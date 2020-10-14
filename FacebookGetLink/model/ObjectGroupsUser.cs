using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookGetLink.model
{
    public class ObjectGroupsUser
    {
        public class Group
        {
            public Datum[] data { get; set; }
            public Paging paging { get; set; }
        }

        public class Paging
        {
            public Cursors cursors { get; set; }
            public string next { get; set; }
            public string previous { get; set; }
        }

        public class Cursors
        {
            public string before { get; set; }
            public string after { get; set; }
        }

        public class Datum
        {
            public DateTime created_time { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int member_count { get; set; }
            public bool administrator { get; set; }
        }
    }
}
