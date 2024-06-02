using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery_Module40.Pages
{
    public class PinReEntredFlyoutMenuItem
    {
        public PinReEntredFlyoutMenuItem()
        {
            TargetType = typeof(PinReEntredFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}