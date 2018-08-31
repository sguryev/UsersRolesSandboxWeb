namespace UsersRolesSandboxWeb.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Model;

    public class UserModel
    {
        public UserModel(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            AgencyName = user.Agency.Name;
            GroupIds = user.UserGroups.Select(ug => ug.GroupId).ToList();

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AgencyName { get; set; }

        public List<int> GroupIds { get; set; }
    }
}
