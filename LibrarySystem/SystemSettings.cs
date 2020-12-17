using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrarySystem
{
    class SystemSettings
    {
        private string emp_id;

        public SystemSettings(string emp)
        {
            emp_id = emp;
        }

        public string getEmpId
        {
            get
            {
                return emp_id;
            }
            set
            {
                emp_id = value;
            }
        }
        public string display()
        {
            return emp_id;
        }
    }
}
