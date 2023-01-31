using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceModels
{
    public class CurrentUser
    {
        public string Id { get; private set; }
        public string Username { get; private set; }

        public CurrentUser(string id, string username)
        {
            Id = id;
            Username = username;
        }
    }
}
