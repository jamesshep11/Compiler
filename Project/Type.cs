using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Type
    {
        private byte kind;      //int, double, bool, error

        static byte BOOL = 1, INT = 2, DOUBLE = 3, ERROR = 0;

        public Type(byte kind) {
            this.kind = kind;
        }

        public bool equals(Type other) {
            return this.kind == other.kind
                || this.kind == ERROR
                || other.kind == ERROR;
        }
        
        public static Type integer = new Type(INT);
        public static Type doub = new Type(DOUBLE);
        public static Type boolean = new Type(BOOL);
        public static Type error = new Type(ERROR);
    }
}
