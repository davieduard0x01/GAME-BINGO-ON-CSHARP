using System;
using System.Linq;

namespace ProjetoBingo
{
    class JogoBingo
    {
        static int[,] minhaCartela = new int[5, 5];
        static int[,] numerosMarcados = new int[5, 5];
        static int[] idades;
        static char[] sexos;

        static void PreencherCartela(int[,] cartela)
        {
            int a = cartela.GetLength(0);
            int b = cartela.GetLength(1);
            Random r = new Random();

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    cartela[i, j] = r.Next(1, 75);
                }
            }

            cartela[2, 2] = 0;

            Console.WriteLine("\n\n ");

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    // Aqui parece que deveria imprimir algo, mas está comentado
                    // Console.Write("BINGO - DAVI EDUARDO PUC MINAS - SISTEMAS DE INFORMAÇÃO ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n ");
        }

        static void ObterConfiguracoesJogo(out int numJogadores, out int[] numCartelasPorJogador, out string[] nomes)
        {
            Console.Write("Digite o número de jogadores (entre 2 e 5): ");
            numJogadores = int.Parse(Console.ReadLine());

            if (numJogadores < 2 || numJogadores > 5)
            {
                Console.WriteLine("Número inválido de jogadores. Por favor, digite um número entre 2 e 5.");
                numCartelasPorJogador = null;
                nomes = null;
                idades = null;
                sexos = null;
                return;
            }

            numCartelasPorJogador = new int[numJogadores];
            nomes = new string[numJogadores];
            idades = new int[numJogadores];
            sexos = new char[numJogadores];

            for (int i = 0; i < numJogadores; i++)
            {
                Console.Write($"Jogador {i + 1}, digite o número de cartelas: ");
                numCartelasPorJogador[i] = int.Parse(Console.ReadLine());

                Console.Write($"Informe o nome do Jogador {i + 1}: ");
                nomes[i] = Console.ReadLine();

                Console.Write($"Informe a idade do Jogador {i + 1}: ");
                idades[i] = int.Parse(Console.ReadLine());

                Console.Write($"Informe o sexo do Jogador {i + 1} (M/F): ");
                sexos[i] = char.Parse(Console.ReadLine().ToUpper());
            }
        }

        static void PreencherCartelas(int numJogadores, int[] numCartelasPorJogador, string[] nomes)
        {
            for (int i = 0; i < numJogadores; i++)
            {
                Console.WriteLine($"\nCartelas para o jogador {nomes[i]} (Idade: {idades[i]}, Sexo: {sexos[i]}):");

                for (int j = 0; j < numCartelasPorJogador[i]; j++)
                {
                    int[,] cartela = GerarCartela();
                    ImprimirCartela(cartela);
                }
            }
        }

        static int[,] GerarCartela()
        {
            int a = 5, b = 5;
            Random r = new Random();

            int[,] novaCartela = new int[a, b];

            int[] numerosDisponiveis = Enumerable.Range(1, 75).ToArray();

            EmbaralharArray(numerosDisponiveis);

            int index = 0;

            for (int coluna = 0; coluna < b; coluna++)
            {
                for (int linha = 0; linha < a; linha++)
                {
                    novaCartela[linha, coluna] = numerosDisponiveis[index++];
                }
            }

            novaCartela[2, 2] = 0;

            return novaCartela;
        }

        static void ImprimirCartela(int[,] cartela)
        {
            int a = 5, b = 5;

            Console.ForegroundColor = ConsoleColor.Red;

            for (int i = 0; i < a; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < b; j++)
                {
                    Console.ForegroundColor = (i == 2 && j == 2) ? ConsoleColor.Yellow : ConsoleColor.Gray;
                    Console.Write(cartela[i, j].ToString().PadLeft(2));

                    if (j < b - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" | ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                Console.Write(" |");
                Console.WriteLine();
                if (i < a - 1)
                {
                    Console.WriteLine("+" + new string('-', 29) + "+");
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        static int SortearNumero()
        {
            Random random = new Random();
            return random.Next(1, 75);
        }

        static void MarcarCartela(string nome, int[,] cartelaDoJogador)
        {
            Console.WriteLine($"{nome}, sua Cartela:");
            ImprimirCartela(cartelaDoJogador);

            Console.Write("Digite a linha onde deseja marcar o número: ");
            int linha = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Digite a coluna onde deseja marcar o número: ");
            int coluna = int.Parse(Console.ReadLine()) - 1;

            if (linha < 0 || linha >= cartelaDoJogador.GetLength(0) || coluna < 0 || coluna >= cartelaDoJogador.GetLength(1))
            {
                Console.WriteLine("Posição inválida. Tente novamente.");
                return;
            }

            if (numerosMarcados[linha, coluna] == 1)
            {
                Console.WriteLine("Essa posição já foi marcada.");
            }
            else
            {
                Console.WriteLine($"Número {cartelaDoJogador[linha, coluna]} marcado na cartela!");
                numerosMarcados[linha, coluna] = 1;
            }
        }

        static bool VerificarBingo()
        {
            if (VerificarLinhasBingo() || VerificarColunasBingo())
            {
                return true;
            }

            return false;
        }

        static bool VerificarLinhasBingo()
        {
            int a = minhaCartela.GetLength(0);
            int b = minhaCartela.GetLength(1);

            for (int i = 0; i < a; i++)
            {
                if (VerificarSequenciaBingo(minhaCartela, i, 0, 0, 1))
                {
                    return true;
                }
            }

            return false;
        }

        static bool VerificarColunasBingo()
        {
            int a = minhaCartela.GetLength(0);
            int b = minhaCartela.GetLength(1);

            for (int j = 0; j < b; j++)
            {
                if (VerificarSequenciaBingo(minhaCartela, 0, j, 1, 0))
                {
                    return true;
                }
            }

            return false;
        }

        static bool VerificarSequenciaBingo(int[,] matriz, int linha, int coluna, int deltaLinha, int deltaColuna)
        {
            int a = matriz.GetLength(0);
            int b = matriz.GetLength(1);

            for (int i = 0; i < a; i++)
            {
                if (matriz[linha, coluna] != 0 && numerosMarcados[linha, coluna] != 1)
                {
                    return false;
                }

                linha += deltaLinha;
                coluna += deltaColuna;
            }

            return true;
        }

        static void EmbaralharArray(int[] array)
        {
            Random random = new Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        static void Main(string[] args)
        {
            Console.Write("BINGO - DAVI EDUARDO PUC MINAS - SISTEMAS DE INFORMAÇÃO  \n");

            ObterConfiguracoesJogo(out int numJogadores, out int[] numCartelasPorJogador, out string[] nomes);

            PreencherCartela(minhaCartela);

            PreencherCartelas(numJogadores, numCartelasPorJogador, nomes);

            bool jogoContinua = true;

            while (jogoContinua)
            {
                int numeroSorteado = SortearNumero();
                Console.WriteLine($"\nNúmero sorteado: {numeroSorteado}");

                for (int i = 0; i < numJogadores; i++)
                {
                    Console.WriteLine($"\n{nomes[i]} (Idade: {idades[i]}, Sexo: {sexos[i]}), o que deseja fazer?");
                    Console.WriteLine("1 - Marcar um número");
                    Console.WriteLine("2 - Prosseguir");
                    Console.WriteLine("3 - Gritar Bingo");

                    int escolha = int.Parse(Console.ReadLine());

                    if (escolha == 1)
                    {
                        MarcarCartela(nomes[i], minhaCartela);

                        if (VerificarCartelaCompleta(minhaCartela))
                        {
                            Console.WriteLine($"{nomes[i]} completou a cartela!");
                            jogoContinua = false;
                            break;
                        }
                    }
                    else if (escolha == 2)
                    {
                        Console.Clear();
                        PreencherCartelas(1, new int[] { numCartelasPorJogador[i] }, new string[] { nomes[i] });
                    }
                    else if (escolha == 3)
                    {
                        if (VerificarBingo())
                        {
                            Console.WriteLine($"{nomes[i]} gritou Bingo!");
                            jogoContinua = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Bingo inválido. Prosseguindo para o próximo jogador...");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida. Prosseguindo para o próximo jogador...");
                    }
                }
            }

            Console.ReadKey();
        }

        static bool VerificarCartelaCompleta(int[,] cartela)
        {
            int a = cartela.GetLength(0);
            int b = cartela.GetLength(1);

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    if (cartela[i, j] != 0 && numerosMarcados[i, j] != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
