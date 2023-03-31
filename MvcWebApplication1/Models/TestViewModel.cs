namespace MvcWebApplication1.Models
{
    public class TestViewModel
    {
        public string? Name { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(Name);
    }
}