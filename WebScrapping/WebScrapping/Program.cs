using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

class Progam
{
    //namespace RaspandoAPIInsomnia
    //{
        //Classse de contexto banco de dados
        public class LogContext : DbContext
        {
            public DbSet<Log> Logs { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server = PC03LAB2524\\SENAI; Database= webscrapingDb; User Id=sa; Password=senai.123");
            }
        }
        

    //}

    //Classe de modelopara os logs
    public class Log
    {
        [Key]
        public int IdLog { get; set; }
        public string CodRob { get; set; }
        public string UsuRob { get; set; }
        public DateTime DateLog { get; set; }
        public string Processo { get; set; }
        public string IngLog { get; set; }
        public int idProd { get; set; }
    }

    //Lista para armazenar os produtos já verificados
    static List<Produto> produtosVerificados = new List<Produto>();

    static void Main(string[] args)
    {
        // Definir o intervalo de tempo para r minutos 300.000 milisegundos
        //mudar de volta para 300000 antes de entregar
        //int intervalo = 300000;
        int intervalo = 60000;

        // Criar um temporizador que dispara a cada 5 minutos
        Timer timer = new Timer(verificarNovoProduto, null, 0,intervalo);

        //Manter a aplicação rodando
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static async void verificarNovoProduto(object state)
    {
        string username = "11164448";
        string senha = "60-dayfreetrial";
        string url = "Http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";

        try
        {
            //Criar um objeto HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Adicionar as credenciais e autentificação básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Fazer a requisição GET e API
                HttpResponseMessage response = await client.GetAsync(url);

                //Verificar se a requisição foi bem-sucedida (codigo de status 200)
                if (response.IsSuccessStatusCode)
                {
                    // Ler o conteúdo da resposta como uma string
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Processa os dados da resposta
                    List<Produto> novosProdutos = ObterNovosProdutos(responseData);
                    foreach (Produto produto in novosProdutos)
                    {
                        if (!produtosVerificados.Exists(p => p.id == produto.id))
                        {
                            //Se é um novo produto, faça algo com ele
                            Console.WriteLine($"Novo produto encontrado: ID {produto.id}, Nome: {produto.Nome}");
                            // Adicionar o produto´à lista de produtos verificados
                            produtosVerificados.Add(produto);

                            //Registra um log no banco de dados
                            RegistrarLog("D001", "DanielJ", DateTime.Now, "ConsultaAPI - Verificar Produto", "Sucesso", produto.id);
                        }
                    }


                    // Imprimir os dados da resposta
                    Console.WriteLine(responseData);
                }
                else
                {
                    //Imprimi mensagem de erro caso a requisição falhe
                    Console.WriteLine($"Erro: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex) 
        {
            //Imprimir mensagem de erro caso ocorra uma exceção
            Console.WriteLine($"Erro ao fazer requisição: {ex.Message}");
        }
    }

    // Método para processar os dados da resposta para uma lista de produtos
    static List<Produto> ObterNovosProdutos(string responseData)
    {
        // Deserializar os dados da resposta para uma lista de produtos
        List<Produto> produtos = JsonConvert.DeserializeObject<List<Produto>>(responseData);
        return produtos;
    }

    // Método para registrar um log no banco de dados
    static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {
        using (var context = new LogContext())
        {
            var log = new Log
            {
                CodRob = codRob,
                UsuRob = usuRob,
                DateLog = dateLog,
                Processo = processo,
                IngLog = infLog,
                idProd = idProd
            };
            context.Logs.Add(log);
            context.SaveChanges();
        }
    }

    // Classe para representar um produto
    public class Produto 
    {
        public int id { get; set; }
        public string Nome { get; set; }
    }

    
}
