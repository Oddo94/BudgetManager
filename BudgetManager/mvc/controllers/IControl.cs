using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    public interface IControl {

        void setView(IView view);
        void setModel(IModel model);        
        void disableViewControls();
        void enableViewControls();    
        void requestData(QueryType option, QueryData paramContainer);
    }
}
