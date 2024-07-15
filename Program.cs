using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace ShannCod
{
    class Program
    {
        public static DecodCodificador Decod;

        static void Main(string[] args)
        {
            Console.WriteLine("ATENÇÃO: Por favor, clique nas teclas usando as letras maiúsculas para ativar os comandos.");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("PROJETO COM PROGRAMAÇÃO ORIENTADA A OBJETOS");
            Console.WriteLine("Projeto: Codificador e decodificador por código de blocos\n\n");
            Console.WriteLine("\nCréditos:\n*Kevin Almeida\n*Davi Risuenho\n*Herick Valente");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Escreva o valor de N");
            int nn = int.Parse(Console.ReadLine());

            Console.WriteLine("Escreva o valor de K");
            int kk = int.Parse(Console.ReadLine());

            Decod = new DecodCodificador(nn, kk);
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
        int[,] MatrizR;

        string[] Mensagens;

        List<string> Codigos;
        List<string> CodigosG;
        List<string> CodigosT;
        List<string> CodigosE;

        public DecodCodificador()
        {
            n = 7;
            k = 4;

            GerarCodigos();
        }

        public DecodCodificador(int x, int y)
        {
            n = x;
            k = y;

            GerarCodigos();
        }

        public void GerarCodigos()
        {
            Console.WriteLine("Gerando um novo codificador...");
            Console.WriteLine("Gerando as matrizes...");

            //MATRIZES
            MatrizI = new int[k, k];
            MatrizP = new int[k, n - k];
            MatrizG = new int[k, n];
            MatrizT = new int[n, n - k];
            MatrizR = new int[n - k, n - k];
            MatrizE = new int[n, n];
            
            //LISTA CÓDIGOS
            Codigos = new List<string>();
            CodigosG = new List<string>();
            CodigosT = new List<string>();
            CodigosE = new List<string>();
            
            //MATRIZ IDENTIDADE N-K*N-K
            for (int c = 0; c < MatrizR.GetLength(1); c++)
            {
                for (int l = 0; l < MatrizR.GetLength(0); l++)
                {
                    if (c == l)
                    {
                        MatrizR[l, c] = 1;
                    }
                    else
                    {
                        MatrizR[l, c] = 0;
                    }

                }
            }

            //MATRIZ IDENTIDADE N*N
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

            //MATRIZ IDENTIDADE K*K
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

            //MATRIZ DE PARIDADE
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

            //MATRIZ GERADORA
            for (int l = 0; l < MatrizG.GetLength(0); l++)
            {
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

                    CodG += "" + MatrizG[l, c];
                }

                CodigosG.Add(CodG);
            }

            //MATRIZ TRANSPORT N*N-K
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
                        MatrizT[l, c] = MatrizR[l - k, c];
                    }

                    CodT += "" + MatrizT[l, c];
                }
                
                CodigosT.Add(CodT);
            }

            Console.WriteLine("Matrizes geradas com sucesso!");
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Matrizes geradora:");
            for (int g = 0; g < CodigosG.Count; g++)
            {
                Console.WriteLine(CodigosG[g]);
            }

            Console.WriteLine("\nMatriz Transposta:");
            for (int g = 0; g < CodigosT.Count; g++)
            {
                Console.WriteLine(CodigosT[g]);
            }

            Console.WriteLine("\nMatriz das Palavras de Erro:\n");
            for (int g = 0; g < CodigosE.Count; g++)
            {
                Console.WriteLine(CodigosE[g]);
            }

            Console.WriteLine("Clique Enter para continar...");
            Console.ReadLine();
            Console.Clear();

            Mensagens = BinarioOpedoroes.Sequencia(k);
            List<string> PalCod = new List<string>();

            for (int m = 0; m < Mensagens.Length; m++)
            {
                PalCod.Add(BinarioOpedoroes.Multi_SomaBinCod(BinarioOpedoroes.CodigoSomar(Mensagens[m], CodigosG.ToArray(), n)));
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

                if(Quantidade > 0 && Quantidade <= dmin || dmin < 1)
                {
                    dmin = Quantidade;
                }
            }

            Console.WriteLine("Dados do codificador/decodificador:");
            d = dmin - 1;
            t = (int)((dmin - 1) / 2);

            Console.WriteLine("Dmin = " + dmin);
            Console.WriteLine("D = " + d);
            Console.WriteLine("T = " + t);

            Console.WriteLine("Clique Enter para continar...");
            Console.ReadLine();
            Console.Clear();

            A:
            Console.WriteLine("Clique C para codificar e D para decodificar");
            string Comando = Console.ReadLine();

            if(Comando == "C")
            {
                Console.WriteLine("Escreva a mensagem de tamanho k = " + k + ":");
                string Mensagem = Console.ReadLine();
                Codificar(Mensagem);
                goto A;
            }

            if (Comando == "D")
            {
                Console.WriteLine("Escreva a  palavra código de tamanho n = " + n + ":");
                string Palavra = Console.ReadLine();
                Decodificar(Palavra);
                goto A;
            }
        }

        public void Codificar(string Mensagem)
        {
            string Palavra = BinarioOpedoroes.Multi_SomaBinCod(BinarioOpedoroes.CodigoSomar(Mensagem, CodigosG.ToArray(), n));
            Console.WriteLine("Mensagem Codificada: " + Mensagem);
            Console.WriteLine("Palavra codificada: " + Palavra);

            Console.WriteLine("Clique Enter para continar...");
            Console.ReadLine();
            Console.Clear();
            return;
        }

        public void Decodificar(string Palavra)
        {
            string Sindrome = BinarioOpedoroes.Multi_SomaBinCod(BinarioOpedoroes.CodigoSomar(Palavra, CodigosT.ToArray(), n - k));
            int IndexSindrome = CodigosT.IndexOf(Sindrome) + 1;
            string PalavraCorr = Palavra;

            if(IndexSindrome > 0)
            {
                PalavraCorr = BinarioOpedoroes.SomaBinCod(Palavra, CodigosE[IndexSindrome - 1]);
            }

            string Mensagem = "";

            for(int i = 0; i < k; i++)
            {
                Mensagem += PalavraCorr[i];
            }

            Console.WriteLine("Palavra Recebida: " + Palavra);
            Console.WriteLine("Síndrome: " + Sindrome);
            Console.WriteLine("Padrão de Erro: " + CodigosE[IndexSindrome - 1]);
            Console.WriteLine("Palavra Corrigida: " + PalavraCorr);
            Console.WriteLine("Mensagem: " + Mensagem);

            Console.WriteLine("Clique Enter para continar...");
            Console.ReadLine();
            Console.Clear();
            return;
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
