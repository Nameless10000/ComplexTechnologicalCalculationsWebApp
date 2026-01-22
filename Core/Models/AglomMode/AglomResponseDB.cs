using Console.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AglomMode
{
    public class AglomResponseDB : Entity
    {
        public List<ComponentInfoDB> Components { get; set; }
    }
}
