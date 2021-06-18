﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager {
    class ViewCalculator {


        public static double calculatePercentage(int value, int total) {

            if (total == 0 || value < 0) {
                return 0;
            }

            //Calculates the percentage result as a decimal values
            double result = (value * 100) / (double)total;

            //Limits the number of decimal places of the result to 2 (by rounding)
            return Math.Round(result, 2);
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
