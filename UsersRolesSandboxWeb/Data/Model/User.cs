namespace UsersRolesSandboxWeb.Data.Model
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int AgencyId { get; set; }
        public Agency Agency { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
    }
}
