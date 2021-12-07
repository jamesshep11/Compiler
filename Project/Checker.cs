using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Checker
    {
        IdTable idTable;

        public Checker(program prog) {
            idTable = new IdTable();
            idTable.add(new PlusOperatorDeclaration(new Operator("+")));
            idTable.add(new MinusOperatorDeclaration(new Operator("-")));
            idTable.add(new TimesOperatorDeclaration(new Operator("*")));
            idTable.add(new DivisionOperatorDeclaration(new Operator("/")));
            idTable.add(new LessThanOperatorDeclaration(new Operator("<")));
            idTable.add(new GreaterThanOperatorDeclaration(new Operator(">")));
            idTable.add(new EqualsOperatorDeclaration(new Operator("=")));

            prog.check(this);
        }

        public object checkProgram(program p) {
            if (p.command != null)
                p.command.check(this);
            return null;
        }

        public object checkIfCommand(IfStatement f) {
            Type expType = Type.error;
            if (f.exp != null)
                expType = (Type) f.exp.check(this);
            if (!expType.equals(Type.boolean))
                Console.WriteLine("ERROR: if expression must be boolean");
            if (f.thenCommand != null)
                f.thenCommand.check(this);
            if (f.elseCommand != null)
                f.elseCommand.check(this);

            return null;
        }

        public object checkAssignCommand(AssignmentCommand com) {
            SingleDeclaration dec = null;
            Type expType = Type.error;
            if (com.vName != null)
                dec = (SingleDeclaration) com.vName.check(this);
            if (com.exp != null)
                expType = (Type) com.exp.check(this);
            if (dec != null && !dec.type.type.equals(expType))
                Console.WriteLine("Error: cannot assign value to variable " + dec.vName.Spelling);
            return null;
        }

        public object checkLetCommand(LetCommand com) {
            idTable.openScope();
            if (com.declaration != null)
                com.declaration.check(this);
            if (com.command != null)
                com.command.check(this);
            idTable.closeScope();
            return null;
        }

        public Type checkExpression(Expression exp) {
            Type T1 = Type.error, T2 = Type.error, result = Type.error;
            OperatorDeclaration OpDec = null;
            if (exp.P1 != null)
                T1 = (Type) exp.P1.check(this);
            if (exp.P2 != null)
                T2 = (Type) exp.P2.check(this);
            if (exp.O != null)
                OpDec = (OperatorDeclaration)exp.O.check(this);

            if (OpDec != null)
                result = OpDec.getType(T1, T2);
           

            if (result == Type.error)
                Console.WriteLine("Error: Invalid type match in expression");

            return result;
        }

        public Type checkIdentifier(IdentifierPE identifier) {
            if (identifier.T is VName) {
                SingleDeclaration dec = (SingleDeclaration) ((VName)identifier.T).check(this);
                identifier.type = dec.type.type;
                identifier.variable = true;
            } else if (identifier.T is IntLit) {
                identifier.type = Type.integer;
                identifier.variable = false;
            } else {
                identifier.type = Type.error;
                identifier.variable = false;
                Console.WriteLine("Error: unknown identifier");
            }
            return identifier.type;
        }

        public Type checkBracketsPE(BracketsPE bracketsPE) {
            if (bracketsPE.E != null)
                return (Type) bracketsPE.E.check(this);
            return Type.error;
        }

        public object checkSeqDeclaration(SequentialDeclaration seqDec) {
            if (seqDec.declaration1 != null)
                seqDec.declaration1.check(this);
            if (seqDec.declaration2 != null)
                seqDec.declaration2.check(this);
            return null;
        }

        public object checkSingleDeclaration(SingleDeclaration dec) {
            if (dec.type != null)
                dec.type.check(this);
            idTable.add(dec);
            return null;
        }

        public SingleDeclaration checkVName(VName vName) {
            vName.declaration = (SingleDeclaration) idTable.retrieve(vName);
            return (SingleDeclaration) vName.declaration;
        }

        public Type checkIntLit(IntLit intLit) {
            return Type.integer;
        }

        public Type checkTypeDenoter(TypeDenoter typeDenoter) {
            switch (typeDenoter.Spelling) {
                case "int":
                    typeDenoter.type = Type.integer;
                    break;
                case "double":
                    typeDenoter.type = Type.doub;
                    break;
                case "boolean":
                    typeDenoter.type = Type.boolean;
                    break;
                default:
                    typeDenoter.type = Type.error;
                    Console.WriteLine("Error: Unknown type denoter " + typeDenoter.Spelling);
                    break;
            }

            return typeDenoter.type;
        }

        public OperatorDeclaration checkOperator(Operator O) {
            O.declaration = (OperatorDeclaration) idTable.retrieve(O);
            return (OperatorDeclaration)O.declaration;
        }
    }
}
