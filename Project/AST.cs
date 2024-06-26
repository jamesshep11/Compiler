﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    abstract class AST
    {
        public abstract Object check(Checker checker);
    }

    class program : AST
    {
        public Command command;

        public program(Command command) {
            this.command = command;
        }

        public override Object check(Checker checker) {
            return checker.checkProgram(this);
        }
    }

    abstract class Command : AST
    {
    }

    class IfStatement : Command
    {
        public Expression exp;
        public Command thenCommand;
        public Command elseCommand;

        public IfStatement(Expression exp, Command thenCommand, Command elseCommand) {
            this.exp = exp;
            this.thenCommand = thenCommand;
            this.elseCommand = elseCommand;
        }

        public override Object check(Checker checker) {
            return checker.checkIfCommand(this);
        }
    }

    class AssignmentCommand : Command
    {
        public VName vName;
        public Expression exp;

        public AssignmentCommand(VName vName, Expression exp) {
            this.vName = vName;
            this.exp = exp;
        }

        public override Object check(Checker checker) {
            return checker.checkAssignCommand(this);
        }
    }

    class LetCommand : Command
    {
        public Declaration declaration;
        public Command command;

        public LetCommand(Declaration declaration, Command command) {
            this.declaration = declaration;
            this.command = command;
        }

        public override Object check(Checker checker) {
            return checker.checkLetCommand(this);
        }
    }

    class Expression : AST
    {
        public PrimaryExpression P1;
        public Operator O;
        public PrimaryExpression P2;
        
        public Type type;

        public Expression(PrimaryExpression P1, Operator O, PrimaryExpression P2) {
            this.P1 = P1; this.O = O; this.P2 = P2;
        }

        public override Object check(Checker checker) {
            return checker.checkExpression(this);
        }
    }

    abstract class PrimaryExpression : AST
    {
    }

    class IdentifierPE : PrimaryExpression
    {
        public Terminal T;
        public Type type;
        public bool variable;

        public IdentifierPE(Terminal T) {
            this.T = T;
        }

        public override Object check(Checker checker) {
            return checker.checkIdentifier(this);
        }
    }

    class BracketsPE : PrimaryExpression
    {
        public Expression E;

        public BracketsPE(Expression E) {
            this.E = E;
        }

        public override Object check(Checker checker) {
            return checker.checkBracketsPE(this);
        }
    }

    abstract class Declaration : AST
    {
    }

    class SingleDeclaration : Declaration
    {
        public VName vName;
        public TypeDenoter type;

        public SingleDeclaration(VName vName, TypeDenoter type) {
            this.vName = vName;
            this.type = type;
        }

        public override Object check(Checker checker) {
            return checker.checkSingleDeclaration(this);
        }
    }

    class SequentialDeclaration : Declaration
    {
        public Declaration declaration1;
        public Declaration declaration2;

        public SequentialDeclaration(Declaration declaration1, Declaration declaration2) {
            this.declaration1 = declaration1;
            this.declaration2 = declaration2;
        }

        public override Object check(Checker checker) {
            return checker.checkSeqDeclaration(this);
        }
    }

    abstract class Terminal : AST
    {
        public String Spelling;

        public Terminal(String Spelling) {
            this.Spelling = Spelling;
        }
    }

    class VName : Terminal
    {
        public SingleDeclaration declaration;

        public VName(String Spelling) : base(Spelling) {
        }

        public override Object check(Checker checker) {
            return checker.checkVName(this);
        }
    }
    
    class IntLit : Terminal
    {
        public IntLit(String Spelling) : base(Spelling) {
        }

        public override Object check(Checker checker) {
            return checker.checkIntLit(this);
        }
    }

    class Operator : Terminal
    {
        public OperatorDeclaration declaration;

        public Operator(String Spelling) : base(Spelling) {
        }

        public override Object check(Checker checker) {
            return checker.checkOperator(this);
        }
    }

    class TypeDenoter : Terminal
    {
        public Type type;

        public TypeDenoter(String Spelling) : base(Spelling) {
        }

        public override Object check(Checker checker) {
            return checker.checkTypeDenoter(this);
        }
    }

    abstract class OperatorDeclaration : Declaration
    {
        public Operator op;

        public OperatorDeclaration(Operator op) {
            this.op = op;
        }

        public abstract Type getType(Type T1, Type T2);
    }

    class PlusOperatorDeclaration : OperatorDeclaration
    {
        public PlusOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if (T1.equals(Type.integer) && T2.equals(Type.integer))
                return Type.integer;
            if (T1.equals(Type.doub) || T2.equals(Type.doub))
                return Type.doub;
            
            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class MinusOperatorDeclaration : OperatorDeclaration
    {
        public MinusOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if (T1.equals(Type.integer) && T2.equals(Type.integer))
                return Type.integer;
            if (T1.equals(Type.doub) || T2.equals(Type.doub))
                return Type.doub;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class TimesOperatorDeclaration : OperatorDeclaration
    {
        public TimesOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if (T1.equals(Type.integer) && T2.equals(Type.integer))
                return Type.integer;
            if (T1.equals(Type.doub) || T2.equals(Type.doub))
                return Type.doub;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class DivisionOperatorDeclaration : OperatorDeclaration
    {
        public DivisionOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if ((T1.equals(Type.integer) || T1.equals(Type.doub)) &&
                (T2.equals(Type.integer) || T2.equals(Type.doub)))
                return Type.doub;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class LessThanOperatorDeclaration : OperatorDeclaration
    {
        public LessThanOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if ((T1.equals(Type.integer) || T1.equals(Type.doub)) &&
                (T2.equals(Type.integer) || T2.equals(Type.doub)))
                return Type.boolean;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class GreaterThanOperatorDeclaration : OperatorDeclaration
    {
        public GreaterThanOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if ((T1.equals(Type.integer) || T1.equals(Type.doub)) &&
                (T2.equals(Type.integer) || T2.equals(Type.doub)))
                return Type.boolean;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }

    class EqualsOperatorDeclaration : OperatorDeclaration
    {
        public EqualsOperatorDeclaration(Operator op) : base(op) {
        }

        public override Type getType(Type T1, Type T2) {
            if (T1.equals(Type.error) || T2.equals(Type.error))
                return Type.error;

            if (T1.equals(T2))
                return Type.boolean;

            if (T1.equals(Type.integer) && T2.equals(Type.doub))
                return Type.boolean;
            if (T1.equals(Type.doub) && T2.equals(Type.integer))
                return Type.boolean;

            return Type.error;
        }

        public override object check(Checker checker) {
            return null;
        }
    }
}
