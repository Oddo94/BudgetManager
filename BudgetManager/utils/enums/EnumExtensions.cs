using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace BudgetManager.utils.enums {
   public static class EnumExtensions {

        //Method for returning the description (set using the DescriptionAttribute class) from the values contained by an enum object
        public static String getEnumDescription(this Enum enumValue) {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptionAttributes.Length > 0) {
                return descriptionAttributes[0].Description;
            }

            return enumValue.ToString();
        }
    }
}
