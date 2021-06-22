using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetManager {
    //Enum ce defineste tipurile de data calendaristica(data de inceput, data de final)
    public enum DateType {
        START_DATE,
        END_DATE
    }

    //Enum ce defineste tipurile de selectoare de date din punct 
    //de vedere al rolului (pt data de inceput/final, pt selectarea unei anumite luni dintr-un an, pt selectarea unui an intreg)
    public enum DataUpdateControl {
        START_PICKER,
        END_PICKER,
        MONTHLY_PICKER,
        YEARLY_PICKER,
        REFRESH_BUTTON,
        UNDEFINED
    }
    public interface IView {
        void updateView(IModel model);
        void disableControls();//dezactivare controale view
        void enableControls();//activare controale view      
    }
}
