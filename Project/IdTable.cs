using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class IdTable
    {
        private ArrayList idTable;
        private int level;

        public IdTable() {
            idTable = new ArrayList();
            level = 0;
        }

        public void add(Declaration dec) {
            ArrayList row = new ArrayList();
            row.Add(level);
            if (dec is SingleDeclaration)
                row.Add(((SingleDeclaration)dec).vName.Spelling);
            else 
                row.Add(((OperatorDeclaration)dec).op.Spelling);
            row.Add(dec);

            idTable.Add(row);
        }

        public Declaration retrieve(Terminal target) {
            for (int i = idTable.Count-1; i >= 0; i--) {
                ArrayList row = (ArrayList) idTable[i];
                if (row[1].Equals(target.Spelling))
                    return (Declaration)row[2];
            }

            Console.WriteLine("Error: " + target.Spelling + " not found.");
            return null;
        }

        public void openScope() {
            level++;
        }

        public void closeScope() {
            level--;
            foreach (ArrayList row in idTable)
                if ((int)row[0] > level)
                    idTable.Remove(row);
        }

    }
}
