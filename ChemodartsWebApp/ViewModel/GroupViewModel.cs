using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class GroupViewModel : RoundViewModel
    {
        public Group? G { get; set; }
        public GroupFactory? GF { get; set; }

        public GroupViewModel(Group? g) : base(g?.Round)
        {
            G = g;
        }

        public GroupViewModel(Group? g, GroupFactory gf) : base(g?.Round)
        {
            G = g;
            GF = gf;
        }

        public GroupViewModel(Round? round, GroupFactory gf) : base(round) 
        {
            GF = gf;
        }
    }
}
