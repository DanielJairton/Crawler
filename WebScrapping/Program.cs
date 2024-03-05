using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using static Program;
using WebScrapping.Send;
using WebScrapping.Models;
using WebScrapping.Data;
using OpenQA.Selenium.DevTools.V120.Network;

class Program
{
    // Lista para armazenar produtos já verificados
    static List<Produto> produtosVerificados = new List<Produto>();
    
    //static List<string> listaProdutosVerificados = new List<string>{};

    private static string emailDestinatario = "";

    //string emailDestinatario = "";

    static void Main(string[] args)
    {

        // Definir o intervalo de tempo para 5 minutos (300.000 milissegundos)
        int intervalo = 300000;

        // Criar um temporizador que dispara a cada 5 minutos
        Timer timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

        // Manter a aplicação rodando
        /*
        //Console.WriteLine("Pressione qualquer tecla para sair...");
        //Console.Read();
        */

        //Manter a aplicação rodando
        Thread.Sleep(Timeout.Infinite);
    }

    static async void VerificarNovoProduto(object state)
    {
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        try
        {
            // Criar um objeto HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Adicionar as credenciais de autenticação básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Fazer a requisição GET à API
                HttpResponseMessage response = await client.GetAsync(url);

                // Verificar se a requisição foi bem-sucedida (código de status 200)
                if (response.IsSuccessStatusCode)
                {
                    // Ler o conteúdo da resposta como uma string
                    string responseData = await response.Content.ReadAsStringAsync();

                    
                    //Teste sse o email do destinatário já foi informadp
                    if (emailDestinatario == "")
                    {
                        // Ler o email do destinatário
                        Console.WriteLine("Digite o Email do destinátario:");

                        //Variável para o loop while
                        int testDestinataior = 0;
                        // o loop é parado quando uma linha não vazia é inserida
                        while (testDestinataior == 0)
                        {
                            emailDestinatario = Console.ReadLine();
                            if (emailDestinatario == "")
                            {
                                Console.WriteLine("Erro: texto vazio, por favor digite o email do destinátario:");
                            }
                            //Se a linha não é vazia para o loop
                            else
                            {
                                testDestinataior = 1;
                            }
                        }

                        
                    }

                    //Console.Write("\n\nNovo ciclo\n\n");

                    // Processar os dados da resposta
                    List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                    foreach (Produto produto in novosProdutos)
                    {
                        if (!produtosVerificados.Exists(p => p.Id == produto.Id))
                        //if (!listaProdutosVerificados.Exists(p => p == Convert.ToString(produto.Id)))
                        {
                            // Se é um novo produto, faça algo com ele
                            Console.WriteLine($"Produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
                            // Adicionar o produto à lista de produtos verificados
                            produtosVerificados.Add(produto);
                            //listaProdutosVerificados.Add(Convert.ToString(produto.Id));

                            // Registra um log no banco de dados apenas se o produto for novo
                            //if (!ProdutoJaRegistrado(produto.Id))

                            //Não checa no banco de dados se a registros
                            if (1 > 0)
                            {
                                RegistrarLog("rDj1", "DanielJ", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.Id);

                                MercadoLivreScraper mercadoLivreScraper = new MercadoLivreScraper();
                                //Pega as string de preçom, nome e link
                                List<string> listaMercadoLivre = mercadoLivreScraper.ObterPreco(produto.Nome, produto.Id);
                                string precoObtidoMercadoLivre = listaMercadoLivre[0];
                                string linkProdutoMercadoLivre = listaMercadoLivre[1];
                                string nomeProdutoMercadoLivre = listaMercadoLivre[2];

                                MagazineLuizaScraper magazineLuizaScraper = new MagazineLuizaScraper();
                                //Pega as string de preço, nome e link
                                List<string> listaMagazineLuiza = magazineLuizaScraper.ObterPreco(produto.Nome, produto.Id);
                                string precoObtidoMagazineLuiza = listaMagazineLuiza[0];
                                string linkProdutoMagazineLuiza = listaMagazineLuiza[1];
                                string nomeProdutoMagazineLuiza = listaMagazineLuiza[2];


                                try
                                {
                                    //Compara os preços e receber a string com a melhor compra
                                    string melhorCompraEmail = CompararPrecos(precoObtidoMercadoLivre, linkProdutoMercadoLivre, precoObtidoMagazineLuiza, linkProdutoMagazineLuiza, produto.Nome);
                                    RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Benchmarking", "Sucesso", produto.Id);

                                    try
                                    {

                                        //Mandar email e registrar o Log
                                        //O email outlook tem um limite de emails diários, que quando ultrapassado precisa receber um código enviado ao celular
                                        SendEmail.EnviarEmail(nomeProdutoMercadoLivre, precoObtidoMercadoLivre, linkProdutoMercadoLivre, nomeProdutoMagazineLuiza,
                                            precoObtidoMagazineLuiza, linkProdutoMagazineLuiza, produto.Nome, melhorCompraEmail, emailDestinatario);

                                        //Registrar sucesso ao enviar Email
                                        RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Enviar Email", "Sucesso", produto.Id);
                                    }
                                    catch (Exception exEnviarEmail)
                                    {
                                        //Registrar e mostrar erro
                                        RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Enviar Email", "Erro", produto.Id);
                                        Console.WriteLine($"Erro ao enviar email: {exEnviarEmail.Message}");
                                    }

                                }
                                catch (Exception exCompararPrecos)
                                {
                                    //Registrar e mostrar erro
                                    RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Benchmarking", "Erro", produto.Id);
                                    Console.WriteLine($"Erro ao comparar preços: {exCompararPrecos.Message}");
                                }

                            }
                        }
                    }
                }
                else
                {
                    // Imprimir mensagem de erro caso a requisição falhe
                    Console.WriteLine($"Erro: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            // Imprimir mensagem de erro caso ocorra uma exceção
            Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
        }
    }

    // Método para processar os dados da resposta e obter produtos
    static List<Produto> ObterNovosProdutos(string responseData)
    {
        // Desserializar os dados da resposta para uma lista de produtos
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }

    // Método para verificar se o produto já foi registrado no banco de dados
    static bool ProdutoJaRegistrado(int idProduto)
    {
        using (var context = new LogContext())
        {
            return context.LOGROBO.Any(log => log.IdProdutoAPI == idProduto);
        }
    }

    // Método para registrar um log no banco de dados
    static void RegistrarLog(string codigoRobo, string usuarioRobo, DateTime dateLog, string etapa, string informacaoLog, int idProdutoAPI)
    {
        using (var context = new LogContext())
        {
            var log = new LOGROBO
            {
                CodigoRobo = codigoRobo,
                UsuarioRobo = usuarioRobo,
                DateLog = dateLog,
                Etapa = etapa,
                InformacaoLog = informacaoLog,
                IdProdutoAPI = idProdutoAPI
            };
            context.LOGROBO.Add(log);
            context.SaveChanges();
        }
    }

    //Compara preços e retorna uma string com a mensagem apropriada
    public static string CompararPrecos(string precoTextoMercadoLivreTexto, string linkMercadoLivre, string precoTextoMagazineLuiza, string linkMagazineLuiza, string nomeProduto)
    {

        if (precoTextoMercadoLivreTexto == null || precoTextoMagazineLuiza == null)
        {
            Console.WriteLine("string nula");
        }

        Console.WriteLine();
        Console.WriteLine($"Preço Mercado Livre:{precoTextoMercadoLivreTexto} | Preço Magazine Luiza:{precoTextoMagazineLuiza}");

        //Preparando string para conversão
        precoTextoMercadoLivreTexto = precoTextoMercadoLivreTexto.Replace("R", "").Replace("$", "").Replace(".", "");
        //precoTextoMercadoLivreTexto = precoTextoMercadoLivreTexto.Trim(new Char[] { ' ', 'R', '$', '.' });
        precoTextoMagazineLuiza = precoTextoMagazineLuiza.Replace("R", "").Replace("$", "").Replace(".", "");
        //precoTextoMagazineLuiza = precoTextoMagazineLuiza.Trim(new Char[] { ' ', 'R', '$', '.' });

        double valorMercadoLivre = Convert.ToDouble(precoTextoMercadoLivreTexto);
        double valorMagazineLuiza = Convert.ToDouble(precoTextoMagazineLuiza);

        if (valorMercadoLivre < valorMagazineLuiza)
        {
            Console.WriteLine($"O Mercado Livre tem {nomeProduto} de menor preço: {linkMercadoLivre}");
            return ($"O Mercado Livre tem o menor preço: {linkMercadoLivre}");
        }
        else if (valorMercadoLivre > valorMagazineLuiza)
        {
            Console.WriteLine($"A Magazine Luiza tem {nomeProduto} de menor preço: {linkMagazineLuiza}");
            return ($"A Magazine Luiza tem o menor preço: {linkMagazineLuiza}");
        }
        else if (valorMercadoLivre == valorMagazineLuiza)
        {
            Console.WriteLine($"Os preços são iguais: {valorMagazineLuiza} = {valorMercadoLivre}");
            return ($"Os preços de {nomeProduto} são iguais: {valorMagazineLuiza} = {valorMercadoLivre}");
        }
        return null;
    }

}
