﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoeWpfHomeBudget
{
    public interface ViewInterface
    {
        void ShowError(string message);
        void ShowValid(string message);
        void ClearExpense();
        void CancelExpense();

    }
}
