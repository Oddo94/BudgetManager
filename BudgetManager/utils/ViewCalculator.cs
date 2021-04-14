using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class ViewCalculator {


        public static int calculatePercentage(int value, int total) {

            if (total == 0 || value < 0) {
                return 0;
            }

            return (value * 100) / total;
        }

        public static int calculateSum(int[] inputArray) {

            if (inputArray == null) {
                return 0;
            }

            int sum = 0;         
            foreach (int currentValue in inputArray) {
                sum += currentValue;
            }

            return sum;
        }

    }
}

