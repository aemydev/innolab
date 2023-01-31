﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceModels.Relationships
{
    public class UserGroups
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
