using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IScreen
    {

        //Holds a representation of the screen in some way.
        object screenRepresentation { get; set; }


    }
}
