using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UserServiceModels.Relationships;

namespace UserServiceModels
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        //public virtual ICollection<FolderClaim> Claims { get; set; }

        public /*virtual*/  ICollection<UserGroups> Groups { get; set; }


    }
}
