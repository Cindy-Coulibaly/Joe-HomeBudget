﻿using System;
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
        private readonly HomeBudget HomeBudget;
        public Presenter(ViewInterface v)
        {
            HomeBudget = new HomeBudget();
            view = v;
        }
    }
}
