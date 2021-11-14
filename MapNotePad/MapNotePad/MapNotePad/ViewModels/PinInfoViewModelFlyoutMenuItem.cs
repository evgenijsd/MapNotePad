using System;

namespace MapNotePad.ViewModels
{
    public class PinInfoViewModelFlyoutMenuItem
    {
        public PinInfoViewModelFlyoutMenuItem()
        {
            TargetType = typeof(PinInfoViewModelFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}