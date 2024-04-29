using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Entity.System;

namespace WzFrame.Entity.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MenuPageOptionAttribute : Attribute
    {

        public MenuPageOptionAttribute(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public long Id { get; set; }
        public long ParentId { get; set; }

        public string Name { get; set; }
        public string? Assembly { get; set; }
        public string? Class { get; set; }
        public string? Icon { get; set; }
        public string? Path { get; set; }

        public MenuType Type { get; set; } = MenuType.Menu;

        public int Order { get; set; }
        public string? Permission { get; set; }




    }
}
