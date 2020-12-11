using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookGetLink.model.bodyvariable
{
    public class ReactionsVariable
    {
        public int count { get; set; }
        public string cursor { get; set; }
        public string feedbackTargetID { get; set; }
        public string reactionType { get; set; }
        public int scale { get; set; }
        public string id { get; set; }
    }
    public class RactionCount
    {
        public string feedbackTargetID { get; set; }
        public string reactionType { get; set; }
        public int scale { get; set; }
    }
}
