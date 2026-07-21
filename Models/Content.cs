using System;
using System.Collections.Generic;
using System.Text;

namespace SVGLClubConfigHelper.Models
{
    public class Content
    {
        public List<Car> Cars { get; set; }

        public Content(List<Car> cars)
        {
            Cars = cars;
        }
    }
}
