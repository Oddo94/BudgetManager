using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager.mvp.views
{
    public interface IExternalAccountStatisticsView {
        event EventHandler loadUserAccountsEvent;
        event EventHandler displayAccountStatisticsEvent;
        event EventHandler displayAccountTransfersEvent;
        int userId { get; set; }
        String accountName { get; set; }
        String startDate { get; set; } 
        String endDate { get; set; }

        void setControlsBindingSource(BindingSource bindingSource1, BindingSource bindingSource2, BindingSource bindingSource3);
    }
}
