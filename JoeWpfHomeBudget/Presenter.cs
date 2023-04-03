using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace JoeWpfHomeBudget
{
    class Presenter
    {
        private readonly ViewInterface view;
        private readonly HomeBudget model;

        //need to provide file name for 
        public Presenter(ViewInterface v, string databaseFile)
        {
            model = new HomeBudget(databaseFile, true);
            view = v;
        }
    }

}
