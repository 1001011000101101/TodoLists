namespace TodoLists.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long ID { get; set; }

        public virtual long ListID { get; set; }

        [StringLength(250)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Completed { get; set; }

        [ForeignKey("ListID")]
        public virtual List List { get; set; }
    }
}
