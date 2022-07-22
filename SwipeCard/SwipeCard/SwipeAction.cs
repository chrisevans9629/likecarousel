using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeCard
{
    public class SwipeAction<IItem>
    {
        public SwipeAction(SwipeDirection direction, IItem item)
        {
            Direction = direction;
            Item = item;
        }

        public SwipeDirection Direction { get; set; }
        public IItem Item { get; set; }
    }
}
