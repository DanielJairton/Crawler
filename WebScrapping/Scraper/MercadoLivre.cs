using HtmlAgilityPack;
using System;
using WebScrapping.Models;

public class MercadoLivreScraper
{
    public List<string> ObterPreco(string descricaoProduto, int idProduto)
    {
        // URL da pesquisa no Mercado Livre com base na descrição do produto
        string url = $"https://lista.mercadolivre.com.br/{descricaoProduto}";

        try
        {
            // Inicializa o HtmlWeb do HtmlAgilityPack
            HtmlWeb web = new HtmlWeb();

            // Carrega a página de pesquisa do Mercado Livre
            HtmlDocument document = web.Load(url);

            // Encontra o elemento que contém o preço do primeiro produto
            HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//span[@class='andes-money-amount__fraction']");

            //Encontra o link do produto
            HtmlNode firstProductLinkNode = document.DocumentNode.SelectSingleNode("//a[@class='ui-search-item__group__element ui-search-link__title-card ui-search-link']");

            if (firstProductLinkNode != null)
            {
                Console.WriteLine("Link Mercado Livre produto pesquisa obtido");
            }
            else 
            {
                Console.WriteLine("Link Mercado Livre produto pesquisa não obtido");
                return null;
            }

            // Verifica se o elemento foi encontrado
            if (firstProductPriceNode != null )
            {
                // Obtém o preço do primeiro produto
                string firstProductPrice = firstProductPriceNode.InnerText.Trim();

                //Obter link do produto
                string firstProductLink = firstProductLinkNode.Attributes["href"].Value;

                //Obter texto do firstProductLinkNode
                string firstProductName = firstProductLinkNode.InnerText;
                //Console.Write($"\n\nNome Mercado Livre{firstProductName}\n\n");

                Console.WriteLine();
                Console.WriteLine($"Link produto Mercado Livre: {firstProductLink}");
                Console.WriteLine();

                // Registra o log com o ID do produto
                RegistrarLog("rDj1", "DanielJ", DateTime.Now, "WebScraping - Mercado Livre", "Sucesso", idProduto);

                Console.WriteLine("Preço Mercado Livre Obtido");

                //Lista para retornar
                List<string> lista = new List<string>
                {
                    $"{firstProductPrice}",
                    $"{firstProductLink}",
                    $"{firstProductName}"
                };

                // Retorna o preço
                return lista;
            }
            else
            {
                Console.WriteLine("Preço não encontrado.");

                // Registra o log com o ID do produto
                RegistrarLog("rDj1", "DanielJ", DateTime.Now, "WebScraping - Mercado Livre", "Erro: Preço não encontrado", idProduto);

                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Web Scraping - Mercado Livre", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    public void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {
        using (var context = new LogContext())
        {
            var log = new Log
            {
                CodRob = codRob,
                UsuRob = usuRob,
                DateLog = dateLog,
                Processo = processo,
                InfLog = infLog,
                IdProd = idProd
            };
            context.Logs.Add(log);
            context.SaveChanges();
        }
    }
}