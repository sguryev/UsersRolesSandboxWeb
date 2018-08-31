namespace UsersRolesSandboxWeb.Data.Model
{
    using System.Collections.Generic;

    public class Agency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
