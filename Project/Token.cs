using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Token
    {
        String Spelling;
        int Type; // -1 - Error, 1 - VName, 2 - IntLit, 3 - Operator, 4 - LPar, 5 - RPar, 6 - If, 7 - Then, 8 - Else, 9 - Assignment, 10 - Let, 11 - In, 12 - SemiColon, 13 - Colon, 14 - Var, 15 - TypeDenoter

        public Token(String S, int T) {
            Spelling = S;
            Type = T;
        }

        public String getSpelling() {
            return Spelling;
        }

        public int getType() {
            return Type;
        }

        public void showSpelling() {
            Console.WriteLine(Spelling);
        }

        public Boolean matches(String other) {
            return (this.Spelling.Equals(other));
        }

        public Boolean matchesType(int T) {
            return (Type == T);
        }

    }
}
