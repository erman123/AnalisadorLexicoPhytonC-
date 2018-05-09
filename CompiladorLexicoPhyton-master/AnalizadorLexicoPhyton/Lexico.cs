using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SyntaxHighlighter;
using System.Drawing;

namespace AnalizadorLexicoPhyton
{
    public class Lexico
    {
        //
        private string ident = (@"\b[a-zA-Z][\w]*\b|\n");
        private string numero = (@"\b([-+]?\d+[,|.][-+]?\d+|[-+]?\d+)\b|\n");
        private string operador = (@"[\=\+\-\/\*\%]|\n");
        private string ope_logico = (@"([\|\|\>|\<||\<=|\>=|\!=|\==]{1,2})|\n");
        private string delimitador = (@"[\(\)\{\}\[\]:]|\n");
        private string cadena = ("\".*?\"|\n");
        private string comentari = @"\/[*].*?[*]\/|\n";
        private string comentario2 = @"\/\/[^\r\n]*.*?|\n";
        public int linea = 1;
        private List<TablaDesimbolos> PalabrasAnalizadas = new List<TablaDesimbolos>();
        public List<TablaDesimbolos> Reservadas;
        List<TablaDesimbolos> operadores;
        List<TablaDesimbolos> opeLogico;
        List<TablaDesimbolos> delimitadores;
        TablaDesimbolos objts;
        private void DefineTokens()
        {
            delimitadores = new List<TablaDesimbolos>()
            {
                new TablaDesimbolos(56, "(", "Parentesis Izq", linea),
                new TablaDesimbolos(57, ")", "Parentesis der", linea),
                new TablaDesimbolos(58, "[", "Corchete Izq", linea),
                new TablaDesimbolos(59, "]", "Corchete der", linea),
                new TablaDesimbolos(60, "{", "Llave Izq", linea),
                new TablaDesimbolos(61, "}", "Llave der", linea),
                new TablaDesimbolos(61, ":", "puntos de finalizacion", linea),
            };
            opeLogico = new List<TablaDesimbolos>()
            {
                new TablaDesimbolos(17, "||", "operador logico OR", linea),
                new TablaDesimbolos(18, "&&", "operador logico AND", linea),
                new TablaDesimbolos(19, "!", "operador logico NOT", linea),
                new TablaDesimbolos(20, "<", "operador logico menor que", linea),
                new TablaDesimbolos(21, "<=", "operador logico menor o igual que", linea),
                new TablaDesimbolos(22, ">", "operador logico mayor que", linea),
                new TablaDesimbolos(23, ">=", "operador logico mayor o igual que", linea),
                new TablaDesimbolos(24, "==", "operado logico igual que", linea),
                new TablaDesimbolos(25, "!=", "operador logico distinto que", linea),
                new TablaDesimbolos(26, "**", "operador potencia", linea),
            };
            operadores = new List<TablaDesimbolos>()
            {
                new TablaDesimbolos(8, "=", "operador asignacion", linea),
                new TablaDesimbolos(9, "+", "operador suma", linea),
                new TablaDesimbolos(10, "-", "operador menos", linea),
                new TablaDesimbolos(11, "++", "incremento de uno", linea),
                new TablaDesimbolos(12, "--", "decremento de uno", linea),
                new TablaDesimbolos(13, "/", "operador de division", linea),
                new TablaDesimbolos(14, "*", "operador de ultiplicacion", linea),
                new TablaDesimbolos(15, "%", "resto con una division entera", linea),
                new TablaDesimbolos(16, "//", "division entera", linea),
            };
            Reservadas = new List<TablaDesimbolos>()
            {
                new TablaDesimbolos(2, "char", "tipo de dato caracteres ", linea),
                new TablaDesimbolos(3, "float", "tipo de dato reales", linea),
                new TablaDesimbolos(4, "double", "tipo de datos numericos rales", linea),
                new TablaDesimbolos(5, "short", "tipo de datos entero", linea),
                new TablaDesimbolos(6, "int", "tipo de datos entero", linea),
                new TablaDesimbolos(7, "long", "tipo de datos entero", linea),
                new TablaDesimbolos(27, "and", "palabra reservada", linea),
                new TablaDesimbolos(28, "as", "palabra reservada", linea),
                new TablaDesimbolos(29, "assert", "palabra reservada", linea),
                new TablaDesimbolos(30, "break", "palabra reservada", linea),
                new TablaDesimbolos(31, "class", "palabra reservada", linea),
                new TablaDesimbolos(32, "continue", "palabra reservada", linea),
                new TablaDesimbolos(33, "def", "palabra reservada", linea),
                new TablaDesimbolos(34, "del", "palabra reservada", linea),
                new TablaDesimbolos(35, "elif", "palabra reservada condicional", linea),
                new TablaDesimbolos(36, "else", "palabra reservada condicional doble", linea),
                new TablaDesimbolos(37, "except", "palabra reservada", linea),
                new TablaDesimbolos(38, "exec", "palabra reservada", linea),
                new TablaDesimbolos(40, "finally", "palabra reservada", linea),
                new TablaDesimbolos(41, "for", "palabra reservada bucle", linea),
                new TablaDesimbolos(42, "from", "palabra reservada", linea),
                new TablaDesimbolos(43, "global", "palabra reservada", linea),
                new TablaDesimbolos(44, "if", "palabra reservada condicional", linea),
                new TablaDesimbolos(45, "import", "palabra reservada", linea),
                new TablaDesimbolos(46, "in", "palabra reservada", linea),
                new TablaDesimbolos(47, "is", "palabra reservada", linea),
                new TablaDesimbolos(48, "lambda", "palabra reservada bucle", linea),
                new TablaDesimbolos(49, "not", "palabra reservada", linea),
                new TablaDesimbolos(50, "or", "palabra reservada", linea),
                new TablaDesimbolos(51, "pass", "palabra reservada condicional", linea),
                new TablaDesimbolos(52, "try", "palabra reservada", linea),
                new TablaDesimbolos(53, "while", "palabra reservada bucle", linea),
                new TablaDesimbolos(54, "with", "palabra reservada", linea),
                new TablaDesimbolos(55, "yield", "palabra reservada", linea),
                new TablaDesimbolos(56, "print", "palabra reservada", linea),
                new TablaDesimbolos(57, "#include", "palabra reservada", linea),
                new TablaDesimbolos(58, "std", "palabra reservada", linea),
                new TablaDesimbolos(59, "cout", "palabra reservada", linea),
                new TablaDesimbolos(60, "endl", "palabra reservada", linea),
                new TablaDesimbolos(61, "main", "palabra reservada", linea),
                new TablaDesimbolos(62, "void", "palabra reservada", linea),
                 new TablaDesimbolos(63, "input", "palabra reservada", linea),
            };

           
        }
        private void Reservada(string cad)
        {
          
            linea = 1;
            foreach (Match identificador in Regex.Matches(cad, ident, RegexOptions.IgnoreCase))
            {
                if (identificador.Value == "\n")
                    linea++;

                int indice = Reservadas.FindIndex(x => x.Lexema == identificador.Value);

                if (indice >= 0 )
                {
                    objts = new TablaDesimbolos(Reservadas[indice].Token, Reservadas[indice].Lexema, Reservadas[indice].Descripcion, linea);
                    PalabrasAnalizadas.Add(objts);
                }
                else if(identificador.Value != " " && identificador.Value!="\n")
                {
                    objts = new TablaDesimbolos(303, identificador.Value, "Identificador , varibles", linea);
                    PalabrasAnalizadas.Add(objts);
                }

            }
        }
        private void numeros(string cad)
        {
            linea = 1;
            foreach (Match num in Regex.Matches(cad, numero, RegexOptions.IgnoreCase))
            {
                string n= num.Value.Replace('.', ',');
                if (num.Value == "\n")
                    linea++;
                else if (tieneParteDecimal(double.Parse(n)))
                {
                    objts = new TablaDesimbolos(102, num.Value, "Numero double", linea);
                    PalabrasAnalizadas.Add(objts);
                }
                else
                {
                    objts = new TablaDesimbolos(101, num.Value, "Numero entero", linea);
                    PalabrasAnalizadas.Add(objts);
                }

            }
        }
        private bool tieneParteDecimal(double d)
        {
            return d != Math.Floor(d);
        }
        private void comentarioL(string cad)
        {
            linea = 1;
            foreach (Match com in Regex.Matches(cad, comentari, RegexOptions.IgnoreCase))
            {
               
                if (com.Value == "\n")
                    linea++;
                else if (com.Value!="\n")
                {
                    objts = new TablaDesimbolos(105, com.Value, "Comentario de linea", linea);
                    PalabrasAnalizadas.Add(objts);
                }
               
            }
        }
        private void comentarioB(string cad)
        {
            linea = 1;
            foreach (Match com in Regex.Matches(cad, comentario2, RegexOptions.IgnoreCase))
            {

                if (com.Value == "\n")
                    linea++;
                else if (com.Value != "\n")
                {
                    objts = new TablaDesimbolos(105, com.Value, "Comentario de bloque", linea);
                    PalabrasAnalizadas.Add(objts);
                }

            }
        }
        private void Operador(string cad)
        {
            linea = 1;
            foreach (Match operador in Regex.Matches(cad, operador, RegexOptions.IgnoreCase))
            {
                if (operador.Value == "\n")
                    linea++;
                int indice = operadores.FindIndex(x => x.Lexema == operador.Value);
                if (indice >= 0)
                {
                    objts = new TablaDesimbolos(operadores[indice].Token, operadores[indice].Lexema, operadores[indice].Descripcion, linea);
                    PalabrasAnalizadas.Add(objts);
                }
            }
        }
        private void OperadoresLogicos(string cad)
        {
            linea = 1;
            foreach (Match opera in Regex.Matches(cad, ope_logico, RegexOptions.IgnoreCase))
            {
                if (opera.Value == "\n")
                    linea++;

                int indice = opeLogico.FindIndex(x => x.Lexema == opera.Value);
                if (indice >= 0)
                {
                    objts = new TablaDesimbolos(opeLogico[indice].Token, opeLogico[indice].Lexema, opeLogico[indice].Descripcion, linea);
                    PalabrasAnalizadas.Add(objts);
                }

            }
        }
        private void Delimitadores(string cad)
        {
            linea = 1;
            foreach (Match opera in Regex.Matches(cad, delimitador, RegexOptions.IgnoreCase))
            {
                if (opera.Value == "\n")
                    linea++;

                int indice = delimitadores.FindIndex(x => x.Lexema == opera.Value);
                if (indice >= 0)
                {
                    objts = new TablaDesimbolos(delimitadores[indice].Token, delimitadores[indice].Lexema, delimitadores[indice].Descripcion, linea);
                    PalabrasAnalizadas.Add(objts);
                }

            }
        }
        private void cadenas(string cad)
        {
            linea = 1;
            foreach (Match num in Regex.Matches(cad, cadena, RegexOptions.IgnoreCase))
            {
              
                if (num.Value == "\n")
                    linea++;
                else 
                {
                    objts = new TablaDesimbolos(100, num.Value, "Cadena de texto", linea);
                    PalabrasAnalizadas.Add(objts);
                }
             

            }
        }
        public  List<TablaDesimbolos> inicio(string cad)
        {
            DefineTokens();
            PalabrasAnalizadas.Clear();
            Reservada(cad); 
            numeros(cad);
            Operador(cad);
            OperadoresLogicos(cad);
            Delimitadores(cad);
            cadenas(cad);
            comentarioL(cad);
            comentarioB(cad);
            var listaOrdenada = from o in PalabrasAnalizadas
                                orderby o.Linea ascending
                                select o;
            List<TablaDesimbolos> dd = listaOrdenada.ToList<TablaDesimbolos>();
           
            return dd;
        }
        public void pintar(SyntaxRichTextBox syntaxRichTextBox1)
        {
            string[] pala = new string[]
            {
                 "char","float", "double", "short", "int", "long", "and","as", "assert", "break",
                  "class", "continue","def", "del", "elif","else", "except","exec", "finally", "for",
                   "from","global","if","import","in","is","lambda","not","or","pass","try","while",
                    "with","yield","print","include", "std", "cout", "endl", "main", "void", "input",
            };
            foreach (var item in pala)
            {
                syntaxRichTextBox1.Settings.Keywords.Add(item);
            }
          

            syntaxRichTextBox1.Settings.Comment = "//";


            syntaxRichTextBox1.Settings.KeywordColor = Color.Blue;
            syntaxRichTextBox1.Settings.CommentColor = Color.Green;
            syntaxRichTextBox1.Settings.StringColor = Color.Orange;
            syntaxRichTextBox1.Settings.IntegerColor = Color.Red;

            syntaxRichTextBox1.Settings.EnableIntegers = false;
            syntaxRichTextBox1.Settings.EnableStrings = true;
            syntaxRichTextBox1.CompileKeywords();
            syntaxRichTextBox1.ProcessAllLines();

        }
    }
}
