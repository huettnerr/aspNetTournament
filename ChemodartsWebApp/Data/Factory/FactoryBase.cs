using System.ComponentModel.DataAnnotations;

namespace ChemodartsWebApp.Data.Factory
{
    public abstract class FactoryBase
    {
        [ScaffoldColumn(false)]
        public abstract string Controller { get; }

        [ScaffoldColumn(false)]
        public abstract string Action { get; set; }

    }

    public abstract class FactoryBase<T> : FactoryBase where T : class
    {
        public override string Action { get; set; }


        public FactoryBase() { } //Needed for POST

        public FactoryBase(string action)
        {
            Action = action;
        }

        public abstract T? Create();
        public abstract void Update(ref T reference);
    }
}
