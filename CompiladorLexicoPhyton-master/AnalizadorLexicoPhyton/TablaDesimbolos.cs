using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexicoPhyton
{
    public class TablaDesimbolos
    {
        // token codigo 
        // lexema 

        int token;
        string lexema;
        string descripcion;
        int linea;

        public TablaDesimbolos(int token, string lexema, string descripcion, int linea)
        {
            this.Token = token;
            this.Lexema = lexema;
            this.Descripcion = descripcion;
            this.Linea = linea;
        }

        public int Token { get => token; set => token = value; }
        public string Lexema { get => lexema; set => lexema = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Linea { get => linea; set => linea = value; }
    }
}
