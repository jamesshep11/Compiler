using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Scanner
    {
        const int Error = -1;
        const int VName = 1;
        const int IntLit = 2;
        const int Operator = 3;
        const int LPar = 4;
        const int RPar = 5;
        const int If = 6;
        const int Then = 7;
        const int Else = 8;
        const int Assignment = 9;
        const int Let = 10;
        const int In = 11;
        const int SemiColon = 12;
        const int Colon = 13;
        const int Var = 14;
        const int TypeDenoter = 15;

        String Sentence;
        ArrayList TokenList = new ArrayList();
        int curPos;

        public Scanner(String S) {
            Sentence = S;
            curPos = 0;
            BuildTokenList();
        }

        public void DisplayTokens() {
            for (int x = 0; x <= TokenList.Count - 1; x++)
                ((Token)TokenList[x]).showSpelling();
        }

        public ArrayList getTokens() {
            return TokenList;
        }

        String BuildNextToken() {
            String Token = "";
            while (curPos < Sentence.Length && Sentence[curPos] == ' ') curPos++;
            while ((curPos < Sentence.Length) && (Sentence[curPos] != ' ')) {
                Token = Token + Sentence[curPos];
                curPos++;
            }
            return Token;
        }

        int FindType(String Spelling) {
            if (Spelling.Equals("(")) return LPar;
            if (Spelling.Equals(")")) return RPar;

            if (Spelling.Equals("if")) return If;
            if (Spelling.Equals("then")) return Then;
            if (Spelling.Equals("else")) return Else;

            if (Spelling.Equals("let")) return Let;
            if (Spelling.Equals("in")) return In;

            if (Spelling.Equals(":=")) return Assignment;
            if (Spelling.Equals(";")) return SemiColon;
            if (Spelling.Equals(":")) return Colon;
            if (Spelling.Equals("var")) return Var;

            if (Spelling.Equals("+")) return Operator;
            if (Spelling.Equals("-")) return Operator;
            if (Spelling.Equals("*")) return Operator;
            if (Spelling.Equals("/")) return Operator;
            if (Spelling.Equals("<")) return Operator;
            if (Spelling.Equals(">")) return Operator;
            if (Spelling.Equals("=")) return Operator;

            if (Spelling.Equals("a")) return VName;
            if (Spelling.Equals("b")) return VName;
            if (Spelling.Equals("c")) return VName;
            if (Spelling.Equals("d")) return VName;
            if (Spelling.Equals("e")) return VName;

            if (Spelling.Equals("1")) return IntLit;
            if (Spelling.Equals("2")) return IntLit;
            if (Spelling.Equals("3")) return IntLit;

            if (Spelling.Equals("int")) return TypeDenoter;
            if (Spelling.Equals("double")) return TypeDenoter;
            if (Spelling.Equals("boolean")) return TypeDenoter;

            else {
                Console.WriteLine("Error: Unable to identify token " + Spelling);
                Console.ReadLine();
                Environment.Exit(1);
                return Error;
            }
        }

        void BuildTokenList() {
            Token newOne = null;
            while (curPos < Sentence.Length) {
                {
                    String nextToken = BuildNextToken();
                    newOne = new Token(nextToken, FindType(nextToken));
                }
                TokenList.Add(newOne);
            }
        }

    }
}
