using System;
using System.Text.RegularExpressions;

namespace ImplementacaoAutomatos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            { //menu para ficar repetindo ate o usuario sair (escolha 3)
                Console.WriteLine("------- Implementação de AFD e APD -------\n");
                Console.WriteLine("Selecione o tipo do Autômato");
                Console.WriteLine("1 - Autômato Finito Determinístico (AFD)");
                Console.WriteLine("2 - Autômato de Pilha Determinístico (APD)");
                Console.WriteLine("3 - Sair");
                if (!int.TryParse(Console.ReadLine(), out int escolha))
                    escolha = 0;

                switch (escolha)
                {
                    case 1:
                        Console.WriteLine();
                        AFD(); //executando o AFD
                        break;

                    case 2:
                        Console.WriteLine();
                        APD(); //executando o APD
                        break;

                    case 3:
                        return;

                    default:
                        Console.WriteLine("\nOpção inválida!\n");
                        break;
                }
            }
        }

        static void AFD()
        {
            try
            {
                Console.Clear(); //limpando a tela
                string[] estados = { "q0", "q1", "q2" }; //array dos estados
                int estadoInicial = 0; //definindo o estado inicial do automato (q0)
                int estadoFinal = 0; //definindo o estado final do automato (q0)
                int estadoAtual; //estado atual do automato

                Console.WriteLine("Autômato Finito Determinístico (AFD)\n");
                Console.WriteLine("Estados: {q0, q1, q2}\nAlfabeto: {0, 1}\nFunção de Transição:\r\n(q0, 0) = q0\r\n(q0, 1) = q1\r\n(q1, 0) = q2\r\n(q1, 1) = q1\r\n(q2, 0) = q0\r\n(q2, 1) = q1\nEstado Inicial: q0\nEstados Finais: {q0}\n");
                Console.WriteLine("Digite a palavra:");
                string palavra = Console.ReadLine();
                if (!Regex.IsMatch(palavra, @"^[01]+$")) //se a palavra nao for um numero binario, exibir um erro
                    throw new Exception("A palavra digitada não está em binário!");

                Console.WriteLine($"\nPassos da Função de Transição da palavra {palavra}\n");

                estadoAtual = estadoInicial; //comecando pelo estado inicial

                for (int i = 0; i < palavra.Length; i++)
                {// para cada simbolo da palavra, fazer a funcao de transicao
                    estadoAtual = FuncaoTransicao(estadoAtual, palavra[i], estados); //funcao de transicao
                }

                Console.WriteLine($"\nPalavra termina em {estados[estadoAtual]}");

                if (estadoAtual == estadoFinal) //se a palavra terminar no estado final, palavra aceita
                    Console.WriteLine("Palavra aceita");
                else
                    Console.WriteLine("Palavra não aceita");

                Console.WriteLine("\nProgramador responsável: Artur Barbosa Pinto\n");

                Console.WriteLine("Aperte qualquer tecla para retornar");
                Console.ReadKey();
                Console.Clear(); //limpando a tela
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message + "\n"); //exibir erro na tela
                Console.WriteLine("Aperte qualquer tecla para retornar");
                Console.ReadKey();
                Console.Clear(); //limpando a tela
            }
        }

        static void APD()
        {
            try
            {
                Console.Clear(); //limpando a tela
                string[] estados = { "q0", "q1", "q2", "q3" }; //array dos estados
                int estadoInicial = 0; //definindo o estado inicial do automato (q0)
                int estadoFinal = 3; //definindo o estado final do automato (q3)
                int estadoAtual; //estado atual do automato
                int estadoTemp; //estado temporario para execucao do codigo
                string palavraAux; //palavcra temporaria para execucao do codigo

                Console.WriteLine("Autômato de Pilha Determinístico (APD)\n");
                Console.WriteLine("Estados: {q0, q1, q2, q3}\nAlfabeto: {a, b}\nAlfabeto da pilha: {F, X, Z}\nFunção de Transição:\r\n(q0, a, λ) = [q1, XF]\r\n(q0, b, λ) = [q1, ZF]\r\n(q1, a, λ) = [q1, X]\r\n(q1, b, λ) = [q1, Z]\r\n(q1, λ, λ) = [q2, λ]\r\n(q2, a, X) = [q2, λ]\r\n(q2, b, Z) = [q2, λ]\r\n(q2, λ, F) = [q3, λ]\nEstado Inicial: q0\nEstados Finais: {q3}\n");
                Console.WriteLine("Digite a palavra:");
                string palavra = Console.ReadLine();
                if (!Regex.IsMatch(palavra, @"^[ab]+$")) //se a palavra não pertencer ao alfabeto, exibir um erro
                    throw new Exception("A palavra digitada contém símbolos que não pertencem ao alfabeto!");

                palavraAux = palavra.Insert(palavra.Length / 2, "λ"); //inserindo lambda no meio e no final da palavra
                palavraAux = palavraAux.Insert(palavraAux.Length, "λ");

                Pilha p = new Pilha(palavraAux.Length); //criando uma nova pilha

                Console.WriteLine($"\nPassos da Função de Transição da palavra {palavra}\n");
                estadoAtual = estadoInicial; //comecando do estado inicial
                Console.WriteLine($"  [{estados[estadoAtual]}, {palavra}, {p.ImprimirPilha()}]"); //imprimindo na tela o comeco da funcao de transicao

                for (int i = 0; i < palavraAux.Length; i++)
                {// para cada símbolo da palavra, fazer a funcao de transicao
                    if (palavraAux[i] != 'λ') //removendo cada simbolo da palavra para exibir a funcao de transicao
                        palavra = palavra.Remove(palavra.IndexOf(palavraAux[i]), 1);

                    estadoTemp = FuncaoTransicao(estadoAtual, palavraAux[i], estados, p, (palavra.Length == 0 ? "λ" : palavra)); //pegando o estado atual do automato
                    if (estadoTemp == 100) //estado 100 -> sem funcao de transicao
                    {
                        Console.WriteLine($"\nNão há transição para \u03b4({estados[estadoAtual]}, {palavraAux[i]}, {p.Topo()})"); //mostrando a funcao que nao existe
                        Console.WriteLine("Palavra não aceita");
                        Console.WriteLine("\nProgramador responsável: Artur Barbosa Pinto\n");
                        Console.WriteLine("Aperte qualquer tecla para retornar");
                        Console.ReadKey();
                        Console.Clear(); //limpando a tela
                        return;
                    }
                    estadoAtual = estadoTemp; //atualizando o estado atual
                }

                Console.WriteLine($"\nPalavra termina em {estados[estadoAtual]}");

                if (estadoAtual == estadoFinal) //se a palavra terminar no estado final, palavra aceita
                    Console.WriteLine("Palavra aceita");
                else
                    Console.WriteLine("Palavra não aceita");

                Console.WriteLine("\nProgramador responsável: Artur Barbosa Pinto\n");

                Console.WriteLine("Aperte qualquer tecla para retornar");
                Console.ReadKey();
                Console.Clear(); //limpando a tela
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message + "\n"); //exibir erro na tela
                Console.WriteLine("Aperte qualquer tecla para retornar");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static int FuncaoTransicao(int estadoAnterior, char simboloLido, string[] estados) //AFD
        {//transicao dos estados
            /*
            (q0, 0) = q0
            (q0, 1) = q1
            (q1, 0) = q2
            (q1, 1) = q1
            (q2, 0) = q0
            (q2, 1) = q1
            */
            int novoEstado = 100;
            switch (estadoAnterior)
            {
                case 0: //(q0)
                    switch (simboloLido)
                    {
                        case '0':
                            novoEstado = 0; //(q0, 0) = q0
                            break;
                        case '1':
                            novoEstado = 1; //(q0, 1) = q1
                            break;
                    }
                    break;

                case 1: //(q1)
                    switch (simboloLido)
                    {
                        case '0':
                            novoEstado = 2; //(q1, 0) = q2
                            break;
                        case '1':
                            novoEstado = 1; //(q1, 1) = q1
                            break;
                    }
                    break;

                case 2: //(q2)
                    switch (simboloLido)
                    {
                        case '0':
                            novoEstado = 0; //(q2, 0) = q0
                            break;
                        case '1':
                            novoEstado = 1; //(q2, 1) = q1
                            break;
                    }
                    break;
            }

            Console.WriteLine($"\u03b4({estados[estadoAnterior]}, {simboloLido}) = {estados[novoEstado]}"); //imprimindo o passo da funcao de transicao
            return novoEstado; //retornando o novo estado
        }

        static int FuncaoTransicao(int estadoAnterior, char simboloLido, string[] estados, Pilha pilha, string palavra) //APD
        {//transicao dos estados
            /*
            (q0, a, λ) = [q1, XF]
            (q0, b, λ) = [q1, ZF]
            (q1, a, λ) = [q1, X]
            (q1, b, λ) = [q1, Z]
            (q1, λ, λ) = [q2, λ]
            (q2, a, X) = [q2, λ]
            (q2, b, Z) = [q2, λ]
            (q2, λ, F) = [q3, λ]
            */

            int novoEstado = 100;
            switch (estadoAnterior)
            {
                case 0: //q0
                    switch (simboloLido)
                    {
                        case 'a':
                            pilha.Empilhar('F');
                            pilha.Empilhar('X');
                            novoEstado = 1; //(q0, a, λ) = [q1, XF]
                            break;

                        case 'b':
                            pilha.Empilhar('F');
                            pilha.Empilhar('Z');
                            novoEstado = 1; //(q0, b, λ) = [q1, ZF]
                            break;
                    }
                    break;

                case 1: //q1
                    switch (simboloLido)
                    {
                        case 'a':
                            pilha.Empilhar('X');
                            novoEstado = 1; //(q1, a, λ) = [q1, X]
                            break;

                        case 'b':
                            pilha.Empilhar('Z');
                            novoEstado = 1; //(q1, b, λ) = [q1, Z]
                            break;

                        case 'λ':
                            novoEstado = 2; //(q1, λ, λ) = [q2, λ]
                            break;
                    }
                    break;
                case 2: //q2
                    switch (simboloLido)
                    {
                        case 'a':
                            if (pilha.Topo() == 'X') //(q2, a, X) = [q2, λ]
                            {
                                pilha.Desempilhar();
                                novoEstado = 2;
                            }
                            break;

                        case 'b':
                            if (pilha.Topo() == 'Z') //(q2, b, Z) = [q2, λ]
                            {
                                pilha.Desempilhar();
                                novoEstado = 2;
                            }
                            break;

                        case 'λ':
                            if (pilha.Topo() == 'F') //(q2, λ, F) = [q3, λ]
                            {
                                pilha.Desempilhar();
                                novoEstado = 3;
                            }
                            break;
                    }
                    break;
            }

            if (novoEstado == 100)
                return novoEstado; //se nao tiver funcao de transicao, retornar o estado 100

            Console.WriteLine($"\u22a2 [{estados[novoEstado]}, {palavra}, {pilha.ImprimirPilha()}]"); //imprimindo o passo da funcao de transicao
            return novoEstado; //retornadno o estado atual
        }
    }

    //classe da pilha
    class Pilha
    {
        private char[] vetor;
        private int topo;

        //construtor
        public Pilha(int tamanho)
        {
            vetor = new char[tamanho]; //criando um vetor com o tamanho definido 
            topo = 0;
        }

        public void Empilhar(char x) //Push
        {
            vetor[topo] = x; //adicionando o valor no topo da pilha
            topo++; //aumentando o topo
        }

        public char Desempilhar() //Pop
        {
            topo--; //diminuindo o topo
            return vetor[topo]; //retornando o valor do topo da pilha
        }

        public char Topo() //Peek/Top
        {
            if (topo == 0) //se a pilha tiver vazia, retornar lambda
                return 'λ';

            return vetor[topo - 1]; //retornando o valor do topo da pilha
        }

        public string ImprimirPilha()
        {
            if (topo == 0) //se a pilha tiver vazia, imprimir lambda
                return "λ";

            string ret = "";
            for (int i = topo - 1; i >= 0; i--)
            {
                ret += vetor[i];
            }
            return ret; //imprimindo todos os simbolos da pilha
        }
    }
}