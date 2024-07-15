using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace ShannCod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Escreva o valor de N");
            int nn = int.Parse(Console.ReadLine());

            Console.WriteLine("Escreva o valor de K");
            int kk = int.Parse(Console.ReadLine());

            DecodCodificador Instancia = new DecodCodificador
            {
                n = nn,
                k = kk
            };

            Instancia.GerarCodigos();


            Console.WriteLine("Escreva um código de 1 e 0, com " + Instancia.n + " elementos para codificação");
            string CodigoLido = Console.ReadLine();
            Instancia.Decodificar(CodigoLido);
        }
    }

    public class DecodCodificador
    {
        public int n { get; set; }
        public int k { get; set; }

        public int dmin;
        public int d;
        public int t;

        int[,] MatrizI;
        int[,] MatrizP;
        int[,] MatrizG;
        int[,] MatrizT;
        int[,] MatrizE;

        string[] Mensagens;

        List<string> Codigos;
        List<string> CodigosG;
        List<string> CodigosT;
        List<string> CodigosE;

        public void GerarCodigos()
        {
            MatrizI = new int[k, k];
            MatrizP = new int[k, n - k];
            MatrizG = new int[k, n];
            MatrizT = new int[n, n - k];
            MatrizE = new int[n, n];

            Codigos = new List<string>();
            CodigosG = new List<string>();
            CodigosT = new List<string>();
            CodigosE = new List<string>();

            for (int c = 0; c < MatrizE.GetLength(1); c++)
            {
                string CodE = "";

                for (int l = 0; l < MatrizE.GetLength(0); l++)
                {
                    if (c == l)
                    {
                        MatrizE[l, c] = 1;
                    }
                    else
                    {
                        MatrizE[l, c] = 0;
                    }

                    CodE += MatrizE[l, c];
                }

                CodigosE.Add(CodE);
            }

            for (int c = 0; c < MatrizI.GetLength(1); c++)
            {
                for (int l = 0; l < MatrizI.GetLength(0); l++)
                {
                    if (c == l)
                    {
                        MatrizI[l, c] = 1;
                    }
                    else
                    {
                        MatrizI[l, c] = 0;
                    }
                }
            }

            for (int c = 0; c < MatrizP.GetLength(1); c++)
            {
                for (int l = 0; l < MatrizP.GetLength(0); l++)
                {
                    if (c == l)
                    {
                        MatrizP[l, c] = 0;
                    }
                    else
                    {
                        MatrizP[l, c] = 1;
                    }
                }
            }

            string Status = "GERADOR DE CODIGOS LINEARES\n\n\nMatriz Geradora:";
            for (int l = 0; l < MatrizG.GetLength(0); l++)
            {
                Status += "\n";
                string CodG = "";

                for (int c = 0; c < MatrizG.GetLength(1); c++)
                {
                    if (c < k)
                    {
                        MatrizG[l, c] = MatrizI[l, c];
                    }
                    else
                    {
                        MatrizG[l, c] = MatrizP[l, c - k];
                    }

                    Status += " " + MatrizG[l, c];
                    CodG += "" + MatrizG[l, c];
                }

                CodigosG.Add(CodG);
            }

            for (int l = 0; l < MatrizT.GetLength(0); l++)
            {
                string CodT = "";

                for (int c = 0; c < MatrizT.GetLength(1); c++)
                {
                    if(l < k)
                    {
                        MatrizT[l, c] = MatrizP[l, c];
                    }
                    else
                    {
                        MatrizT[l, c] = MatrizI[l - k, c];
                    }

                    CodT += "" + MatrizT[l, c];
                }
                
                CodigosT.Add(CodT);
            }

            Console.WriteLine(Status + "\n\nPalavras da Geradora:\n");

            for (int g = 0; g < CodigosG.Count; g++)
            {
                Console.WriteLine(CodigosG[g]);
            }

            Console.WriteLine("\n\nPalavras da Transposta:\n");

            for (int g = 0; g < CodigosT.Count; g++)
            {
                Console.WriteLine(CodigosT[g]);
            }

            Console.WriteLine("\n\nPalavras de Erro:\n");

            for (int g = 0; g < CodigosE.Count; g++)
            {
                Console.WriteLine(CodigosE[g]);
            }

            Mensagens = BinarioOpedoroes.Sequencia(k);
            List<string> PalCod = new List<string>();

            for(int m = 0; m < Mensagens.Length; m++)
            {
                PalCod.Add(BinarioOpedoroes.Multi_SomaBinCod(BinarioOpedoroes.CodigoSomar(Mensagens[m], CodigosG.ToArray(), n)));
            }

            for (int p = 0; p < Mensagens.Length; p++)
            {
                Console.WriteLine("Palavra Codigo Validas " + PalCod[p]);
            }

            for (int p = 0; p < CodigosG.Count; p++)
            {
                int Quantidade = 0;

                for (int m = 0; m < CodigosG[p].Length; m++)
                {
                    if(CodigosG[p][m] == '1')
                    {
                        Quantidade++;
                    }
                }

                Console.WriteLine("Codigo G {0} e quantidade é {1}", p, Quantidade);
                if(Quantidade > 0 && Quantidade <= dmin || dmin < 1)
                {
                    dmin = Quantidade;
                }
            }

            d = dmin - 1;
            t = (int)((dmin - 1) / 2);

            Console.WriteLine("Dmin = " + dmin);
            Console.WriteLine("D = " + d);
            Console.WriteLine("T = " + t);
        }
        public void Decodificar(string Palavra)
        {
            string Sindrome = BinarioOpedoroes.Multi_SomaBinCod(BinarioOpedoroes.CodigoSomar(Palavra, CodigosT.ToArray(), n - k));
            Console.WriteLine("\n\nPalavara Recebida: {0}\nSindrome: {1}", Palavra, Sindrome);

            int IndexSindrome = CodigosT.IndexOf(Sindrome) + 1;
            string PalavraCorr = Palavra;

            if(IndexSindrome > 0)
            {
                PalavraCorr = BinarioOpedoroes.SomaBinCod(Palavra, CodigosE[IndexSindrome - 1]);
                Console.WriteLine("\n\nPadrão Erro: {0}\nCorreção: {1}", CodigosE[IndexSindrome - 1], PalavraCorr);
            }
            else
            {
                Console.WriteLine("\n\nPadrão Erro: {0}\nCorreção: {1}", "SEM ERRO", PalavraCorr);
            }                  
        }
    }

    public class BinarioOpedoroes
    {
        public BinaryReader Binario;
        public BinaryWriter Binarios;

        public static string GerarCodZero(int NumBits)
        {
            string Retorno = "";

            for (int b = 0; b < NumBits; b++)
            {
                Retorno += '0';
            }

            return Retorno;
        }

        public static string[] Sequencia(int NumBits)
        {
            Console.WriteLine("\nGeração de Mensagens:\n");
            List<string> Sequencias = new List<string>();
            int Tamanho = (int)Math.Pow(2, NumBits);
            string Incremento = "";

            Console.WriteLine("Total Mensagens: " + Tamanho);
            Console.WriteLine("Tamanho (bits): " + NumBits);
            Console.WriteLine("----------------------------");

            char Carry = '0';

            for (int i = 0; i < Tamanho; i++)
            {
                string SequenciaNum = "";

                for (int b = 0; b < NumBits; b++)
                {
                    if (i == 0)
                    {
                        if (b < NumBits - 1)
                        {
                            Incremento += "0";
                        }
                        else
                        {
                            Incremento += "1";
                        }

                        SequenciaNum += "0";
                    }
                    else
                    {
                        char aa = Sequencias[i - 1][NumBits - 1 - b];
                        char bb = Incremento[NumBits - 1 - b];

                        SequenciaNum = XOR(Carry, XOR(bb, aa)) + SequenciaNum;
                        Carry = XORCarry(aa, bb, Carry);
                    }
                }

                Sequencias.Add(SequenciaNum);
                Console.WriteLine("Mensagem: " + SequenciaNum);
            }

            Console.WriteLine("----------------------------");

            return Sequencias.ToArray();
        }

        public static char XOR(char a, char b)
        {
            if(((a == '1') && (b == '0')) || ((a == '0') && (b == '1')))
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }

        public static char XORCarry(char a, char b, char c)
        {
            bool Carry = (a == '1' && b == '1') || (c == '1' && b == '1') || (a == '1' && c == '1');

            if (Carry)
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }

        public static string SomaBinCod(string a, string b)
        {
            string Retorno = "";

            for(int i = 0; i < b.Length; i++)
            {
                Retorno += XOR(a[i], b[i]);
            }
            
            return Retorno;
        }

        public static string Multi_SomaBinCod(string[] CodigosReferencia)
        {
            string ValorFinal = CodigosReferencia[0];

            for (int i = 1; i < CodigosReferencia.Length; i++)
            {
                ValorFinal = SomaBinCod(CodigosReferencia[i], ValorFinal);
            }

            return ValorFinal;
        }

        public static string[] CodigoSomar(string Seq, string[] CodRef, int Tamanho)
        {
            List<string> CodSomar = new List<string>();
            CodSomar.Add(GerarCodZero(Tamanho));

            for (int b = 0; b < Seq.Length; b++)
            {
                if (Seq[b] == '1')
                {
                    CodSomar.Add(CodRef[b]);
                }
            }

            return CodSomar.ToArray();
        }
    }



}
