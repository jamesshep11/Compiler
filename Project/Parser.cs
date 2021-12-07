using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Parser
    {
        ArrayList TokenList;
        Token CurrentToken;
        int CurTokenPos;

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

        public Parser(String Sentence) {
            Scanner S = new Scanner(Sentence);
            TokenList = S.getTokens();
            CurTokenPos = -1; FetchNextToken();
            program P = parseProgram();
            Checker checker = new Checker(P);
        }

        void FetchNextToken() {
            CurTokenPos++;
            if (CurTokenPos < TokenList.Count)
                CurrentToken = (Token)TokenList[CurTokenPos];
            else
                CurrentToken = null;
        }

        void accept(int Type) {
            if (CurrentToken == null)
                Console.WriteLine("Null Error: Something is missing at then end of your code.");

            else if (CurrentToken.matchesType(Type))
                FetchNextToken();
            else
                Console.WriteLine("Syntax Error in accept");
        }

        void acceptIt() {
            FetchNextToken();
        }

        program parseProgram() {
            program prog;
            Command command = parseCommand();
            prog = new program(command);
            return prog;
        }

        Command parseCommand() {
            Command command;
            if (CurrentToken == null)
                return null;
            switch (CurrentToken.getType()) {
                case If:
                    acceptIt();
                    Expression exp = parseExpression();
                    accept(Then);
                    Command thenCommand = parseCommand();
                    accept(Else);
                    Command elseCommand = parseCommand();
                    command = new IfStatement(exp, thenCommand, elseCommand);
                    break;
                case VName:
                    VName vName = parseVName();
                    accept(Assignment);
                    exp = parseExpression();
                    command = new AssignmentCommand(vName, exp);
                    break;
                case Let:
                    acceptIt();
                    Declaration declaration = parseDeclaration();
                    accept(In);
                    Command command1 = parseCommand();
                    command = new LetCommand(declaration, command1);
                    break;
                default:
                    Console.WriteLine("Syntax Error in Command");
                    command = null;
                    break;
            }
            return command;
        }

        Expression parseExpression() {
            Expression ExpAST;
            PrimaryExpression P1 = parsePrimary();
            Operator O = parseOperator();
            PrimaryExpression P2 = parsePrimary();
            ExpAST = new Expression(P1, O, P2);
            return ExpAST;
        }

        PrimaryExpression parsePrimary() {
            PrimaryExpression PE;
            if (CurrentToken == null)
                return null;
            switch (CurrentToken.getType()) {
                case VName:
                    VName V = parseVName();
                    PE = new IdentifierPE(V);
                    break;
                case IntLit:
                    IntLit I = parseIntLit();
                    PE = new IdentifierPE(I);
                    break;
                case LPar:
                    acceptIt();
                    PE = new BracketsPE(parseExpression());
                    accept(RPar);
                    break;
                default:
                    Console.WriteLine("Syntax Error in Primary");
                    PE = null;
                    break;
            }
            return PE;
        }

        Declaration parseDeclaration() {
            Declaration declaration;
            SingleDeclaration singleDec = parseSingleDeclaration();
            if (CurrentToken.getType() == SemiColon) {
                acceptIt();
                declaration = new SequentialDeclaration(singleDec, parseDeclaration());
            } else
                declaration = singleDec;
            return declaration;
        }

        SingleDeclaration parseSingleDeclaration() {
            SingleDeclaration singleDec;
            accept(Var);
            VName vName = parseVName();
            accept(Colon);
            TypeDenoter typeDenoter = parseTypeDenoter();
            singleDec = new SingleDeclaration(vName, typeDenoter);
            return singleDec;
        }

        TypeDenoter parseTypeDenoter() {
            TypeDenoter T = new TypeDenoter(CurrentToken.getSpelling());
            accept(TypeDenoter);
            return T;
        }

        VName parseVName() {
            VName V = new VName(CurrentToken.getSpelling());
            accept(VName);
            return V;
        }

        IntLit parseIntLit() {
            IntLit I = new IntLit(CurrentToken.getSpelling());
            accept(IntLit);
            return I;
        }

        Operator parseOperator() {
            Operator O = new Operator(CurrentToken.getSpelling());
            accept(Operator);
            return O;
        }

    }
}
