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
        private readonly  Expenses expense;
        public Presenter(ViewInterface v)
        {
            view = v;
        }
        public static void CheckIfExpenseExists(int id)
        {

        }

    }
}
