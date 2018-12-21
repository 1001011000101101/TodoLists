namespace TodoLists.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;


    public partial class List
    {
        public object ToJson()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result["ID"] = ID;
            result["Name"] = Name;
            result["UserName"] = UserName;
            result["UrlShared"] = UrlShared;

            return result;
        }

        public object ToJson(string EntityMode)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if ("download".Equals(EntityMode, StringComparison.OrdinalIgnoreCase))
            {
                result = (Dictionary<string, object>)ToJson();
                result["Tasks"] = Tasks?.Select(x => x.ToJson());
            }
            return result;
        }
    }
}
