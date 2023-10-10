using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ViewModel
{
    public class GroupViewModel : RoundViewModel
    {
        private Group? _g;
        public Group? G
        {
            get => _g;
            set
            {
                _g = value;
                if (_g != null) base.R = _g.Round;
            }
        }
        public GroupFactory? GF { get; set; }
    }
}
