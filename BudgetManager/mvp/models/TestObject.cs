using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager.mvp.models {
    internal class TestObject {
        private String testString;

        public TestObject(String testString) {
            this.testString = testString;
        }

        public TestObject() { }

        public String TestString { get => testString; set => testString = value; }


    }
}
