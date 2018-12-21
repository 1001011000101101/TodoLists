namespace TodoLists.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Task
    {
        public object ToJson()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result["ID"] = ID;
            result["Name"] = Name;
            result["Description"] = Description;
            result["Completed"] = Completed;
            result["ListID"] = ListID;

            return result;
        }
    }
}
