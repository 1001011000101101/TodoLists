namespace TodoLists.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long ID { get; set; }

        [StringLength(250)]
        public virtual string Name { get; set; }

        [StringLength(256)]
        public virtual string UserName { get; set; } //Need make foreign key to Users

        public virtual string UrlShared { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
