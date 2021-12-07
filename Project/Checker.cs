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

        public void checkProgram(program p) {
            p.command.check(this);
        }

        public void checkIfCommand(IfStatement f) {
            Type expType = f.exp.check(this);
            if (!expType.equals(Type.boolean))
                Console.WriteLine("ERROR: if expression must be boolean");
            f.thenCommand.check(this);
            f.elseCommand.check(this);
        }

        public void checkAssignCommand(AssignmentCommand com) {
            SingleDeclaration dec = com.vName.check(this);
            Type expType = com.exp.check(this);
            if (!dec.type.type.equals(expType))
                Console.WriteLine("Error: cannot assign value to variable " + dec.vName.Spelling);
        }

        public void checkLetCommand(LetCommand com) {
            idTable.openScope();
            com.declaration.check(this);
            com.command.check(this);
            idTable.closeScope();
        }

        public Type checkExpression(Expression exp) {
            Type T1 = exp.P1.check(this);
            Type T2 = exp.P2.check(this);
            OperatorDeclaration OpDec = exp.O.check(this);
            
            Type result = OpDec.getType(T1, T2);
            if (result == Type.error)
                Console.WriteLine("Error: Invalid type match in expression");

            return result;
        }

        public Type checkIdentifier(IdentifierPE identifier) {
            if (identifier.T is VName) {
                SingleDeclaration dec = ((VName)identifier.T).check(this);
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
            return bracketsPE.E.check(this);
        }

        public void checkSeqDeclaration(SequentialDeclaration seqDec) {
            seqDec.declaration1.check(this);
            seqDec.declaration2.check(this);
        }

        public void checkSingleDeclaration(SingleDeclaration dec) {
            dec.type.check(this);
            idTable.add(dec);
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
