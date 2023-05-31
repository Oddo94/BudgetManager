using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvp.views
{
    internal interface IExternalAccountStatisticsView {
        event EventHandler loadUserAccountsEvent;
        event EventHandler displayAccountStatisticsEvent;

        void setControlsBindingSource(BindingSource bindingSource1, BindingSource bindingSource2);
    }
}
