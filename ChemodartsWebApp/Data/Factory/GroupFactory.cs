using ChemodartsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ChemodartsWebApp.Data.Factory
{
    public class GroupFactory : FactoryBase<Group>
    {
        public override string Controller { get; } = "Group";
        public string Name { get; set; }
        public bool HasRematch { get; set; }

        [ScaffoldColumn(false)]
        public Round? R { get; set; }

        public GroupFactory() { } //Needed for POST
        public GroupFactory(string action, Group? g) : base(action)
        {
            if (g is object)
            {
                Name = g.GroupName;
                HasRematch = g.GroupHasRematch;
            }
            else
            {
                Name = String.Empty;
            }
        }

        public override Group? Create()
        {
            if (R is null) return null;

            Group g = new Group();
            Update(ref g);
            g.RoundId = R.RoundId;

            return g;
        }

        public override void Update(ref Group g)
        {
            g.GroupName = Name;
            g.GroupHasRematch = HasRematch;
        }

    }

    public class GroupFactoryRR : GroupFactory
    {
        public int PlayersPerGroup { get; set; }

        public GroupFactoryRR() { } //Needed for POST
        public GroupFactoryRR(string action) : base(action, null)
        {
            PlayersPerGroup = 0;
        }
    }

    public class GroupFactoryKO : GroupFactory
    {
        public int NumberOfPlayers { get; set; }

        public GroupFactoryKO() { } //Needed for POST
        public GroupFactoryKO(string action) : base(action, null)
        {
            NumberOfPlayers = 0;
        }
    }
}
